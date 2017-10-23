using System;
using System.Net;
using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using Launcher;

namespace GameServer.Scripts
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
        public Type[] PluginTypes { get; set; }

        public ServerSpawnData.LoggingSettings.LogWriterSettings[] LogWriters { get; set; } 
            = new ServerSpawnData.LoggingSettings.LogWriterSettings[0];
        public const string LogFileString = @"Logs/{0:d-M-yyyy}/{0:HH-mm-ss tt}.txt";
        private const string DataDirectory = @"/data";

        private void Update()
        {
            //Execute all queued dispatcher tasks
            Server?.ExecuteDispatcherTasks();
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
            
            //Plugins
            if (PluginTypes != null)
            {
                spawnData.Plugins.PluginTypes.AddRange(PluginTypes);
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
        }
    }
}
