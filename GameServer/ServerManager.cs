using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using Game.Scripts;
using Game.Scripts.Network.Data;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using LogType = DarkRift.LogType;

namespace Server.Scripts
{
    public class ServerManager : Singleton<ServerManager>
    {
        protected ServerManager()
        {
        }

        // Settings to change in editor
        public bool EventsFromDispatcher = true;
        public bool LogToFile = true;
        public bool LogToUnityConsole = true;
        public bool LogToDebug = true;

        public IPAddress Address { get; set; } = IPAddress.Any;
        public ushort Port { get; set; } = 4297;
        public IPVersion IpVersion { get; set; } = IPVersion.IPv4;
        public DarkRiftServer Server { get; private set; }

        public Dictionary<IClient, Player> Players { get; } = new Dictionary<IClient, Player>();
        public List<Player> PendingPlayers { get; } = new List<Player>();
        public System.Random Random = new System.Random();

        private const string LogFileString = @"Logs/{0:d-M-yyyy}/{0:HH-mm-ss tt}.txt";
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
                throw new InvalidOperationException("The server has already been created!");
            }
          
            var spawnData = new ServerSpawnData(Address, Port, IpVersion);

            //Server settings
            spawnData.Server.MaxStrikes = 5;
            spawnData.Server.UseFallbackNetworking = true;      //Unity is broken, work around it...
            spawnData.EventsFromDispatcher = EventsFromDispatcher;

            //Plugin search settings
            spawnData.PluginSearch.PluginTypes.AddRange(UnityServerHelper.SearchForPlugins());
            spawnData.PluginSearch.PluginTypes.Add(typeof(UnityConsoleWriter));

            //Data settings
            spawnData.Data.Directory = DataDirectory;

            //Logging settings
            spawnData.Plugins.LoadByDefault = true;

            if (LogToFile)
            {
                var fileWriter = new ServerSpawnData.LoggingSettings.LogWriterSettings();
                fileWriter.Name = "FileWriter1";
                fileWriter.Type = "FileWriter";
                fileWriter.LogLevels = new[] { LogType.Trace, LogType.Info, LogType.Warning, LogType.Error, LogType.Fatal };
                fileWriter.Settings["file"] = LogFileString;
                spawnData.Logging.LogWriters.Add(fileWriter);
            }

            if (LogToUnityConsole)
            {
                var consoleWriter = new ServerSpawnData.LoggingSettings.LogWriterSettings();
                consoleWriter.Name = "UnityConsoleWriter1";
                consoleWriter.Type = "UnityConsoleWriter";
                consoleWriter.LogLevels = new[] { LogType.Info, LogType.Warning, LogType.Error, LogType.Fatal };
                spawnData.Logging.LogWriters.Add(consoleWriter);
            }

            if (LogToDebug)
            {
                var debugWriter = new ServerSpawnData.LoggingSettings.LogWriterSettings();
                debugWriter.Name = "DebugWriter1";
                debugWriter.Type = "DebugWriter";
                debugWriter.LogLevels = new[] { LogType.Warning, LogType.Error, LogType.Fatal };
                spawnData.Logging.LogWriters.Add(debugWriter);
            }
           
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
