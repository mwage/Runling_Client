using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using Launcher;
using System;
using System.Collections.Generic;
using System.Net;
using Network.Synchronization.Data;
using UnityEngine;
using LogType = DarkRift.LogType;

namespace Server.Scripts
{
    public class ServerManager : Singleton<ServerManager>
    {
        protected ServerManager()
        {
        }

        public IPAddress Address { get; set; } = IPAddress.Any;
        public ushort Port { get; set; } = 4297;
        public IPVersion IpVersion { get; set; } = IPVersion.IPv4;
        public DarkRiftServer Server { get; private set; }

        public Dictionary<Client, Player> Players { get; } = new Dictionary<Client, Player>();
        public List<Player> PendingPlayers { get; } = new List<Player>();
        public System.Random Random = new System.Random();

        public ServerSpawnData.LoggingSettings.LogWriterSettings[] LogWriters { get; set; } 
            = new ServerSpawnData.LoggingSettings.LogWriterSettings[0];
        public const string LogFileString = @"Logs/{0:d-M-yyyy}/{0:HH-mm-ss tt}.txt";
        private const string DataDirectory = @"/data";

        private void Update()
        {
            //Execute all queued dispatcher tasks
            Server?.ExecuteDispatcherTasks();
        }

        public void SendToAll(Message message, SendMode sendMode)
        {
            foreach (var client in Players.Keys)
            {
                client?.SendMessage(message, sendMode);
            }
        }

        public void Create()
        {
            if (Server != null)
            {
                throw new InvalidOperationException("The server has already been created! (Is CreateOnEnable enabled?)");
            }
               
            var spawnData = new ServerSpawnData(Address, Port, IpVersion);

            //Data settings
            spawnData.Data.Settings["directory"] = DataDirectory;

            //Logging settings
            spawnData.Plugins.PluginTypes.Add(typeof(UnityConsoleWriter));

            var fileWriter = new ServerSpawnData.LoggingSettings.LogWriterSettings
            {
                Name = "FileWriter1",
                Type = "FileWriter",
                LogLevels = new[] {LogType.Trace, LogType.Info, LogType.Warning, LogType.Error, LogType.Fatal}
            };
            fileWriter.Settings["file"] = LogFileString;
            
            var consoleWriter = new ServerSpawnData.LoggingSettings.LogWriterSettings
            {
                Name = "UnityConsoleWriter1",
                Type = "UnityConsoleWriter",
                LogLevels = new[] {LogType.Info, LogType.Warning, LogType.Error, LogType.Fatal}
            };

            var debugWriter = new ServerSpawnData.LoggingSettings.LogWriterSettings
            {
                Name = "DebugWriter1",
                Type = "DebugWriter",
                LogLevels = new[] {LogType.Warning, LogType.Error, LogType.Fatal}
            };

            spawnData.Logging.LogWriters.Add(fileWriter);
            spawnData.Logging.LogWriters.Add(consoleWriter);
            spawnData.Logging.LogWriters.Add(debugWriter);

            // Plugins
//            spawnData.Plugins.PluginTypes.Add(typeof(DbConnector));

            Server = new DarkRiftServer(spawnData);
            Server.Start();
        }

        private void OnDisable()
        {
            Close();
        }

        public void Close()
        {
            foreach (var client in Server.ClientManager.GetAllClients())
            {
                client.Disconnect();
            }
            Server.Dispose();
            Application.Quit();
        }
    }
}
