using ServerApplication.Server_logics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ServerApplication
{
    public class Server
    {
        private static readonly string SOURCE_LABEL = "Server";
        public static readonly int PORT_DEFAULT = 2000;
        // Object to listen for new tcp connections.
        private TcpListener tcpListener;
        // List with all the accepted clients.
        private List<ServerClient> clients;
        // Database with all the products and users;
        public Database Database;
        // logging object
        public LogField Log;
        // label for indicating the serverstate
        private ServerStatusLabel ServerStatusLabel;
        // Object to controll the buttons in the server;
        private ServerButtons serverButtons;
        // Status
        public bool Running;
        public Server(LogField log, ServerStatusLabel serverStatusLabel, ServerButtons serverButtons)
        {
            this.Log = log;
            this.serverButtons = serverButtons;
            this.ServerStatusLabel = serverStatusLabel;

            this.Database = new Database();
            this.load();
            // Set the stop button diabled
            this.serverButtons.Button_Stop.IsEnabled = false;
            // Sets the label to init
            this.ServerStatusLabel.SetStatus(ServerStates.Init);
            // List of clients
            this.clients = new List<ServerClient>();
            this.tcpListener = new TcpListener(IPAddress.Any, PORT_DEFAULT);
            // Indicate user of serverstate
            this.ServerStatusLabel.SetStatus(ServerStates.Idle);
        }

        private void AcceptClient(IAsyncResult ar)
        {
            if (!Running)
                return;

            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
            // Adds the new accepted tcpClient to the list
            clients.Add(new ServerClient(tcpClient, this));
            // Start async for accepting clients
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), null);   
        }
        // Starts the server
        public void StartServer()
        {
            Log.PrintLine(SOURCE_LABEL, "starting server!");

            this.Running = true;
            this.serverButtons.Button_Start.IsEnabled = false;
            this.serverButtons.Button_Stop.IsEnabled = true;
            // Start to listen for connections
            this.tcpListener.Start();
            // Async way of accepting new tcp connections
            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), null);
            // Indicate user of serverstate
            this.ServerStatusLabel.SetStatus(ServerStates.Running);
        }
        // Stops the server and all the connected client streams.
        public void StopServer()
        {
            Log.PrintLine(SOURCE_LABEL, "stopping server!");

            this.Running = false;
            this.serverButtons.Button_Start.IsEnabled = true;
            this.serverButtons.Button_Stop.IsEnabled = false;
            // foreach client connected to the server
            for(int i = 0; i < this.clients.Count; i++)
            {
                ServerClient client = this.clients[clients.Count-1];
                // Disposes the TcpClient and NetworkStream
                client.Disconect();
            }
                
            // Stop listening to new tcp connections
            this.tcpListener.Stop();
            // Indicate user of serverstate
            this.ServerStatusLabel.SetStatus(ServerStates.Stopped);
        }
        public void save()
        {
            this.Database.Save("saved_data");
        }
        public void load()
        {
            this.Database.Load("saved_data");
        }
        public void OnDisposeServerClient(ServerClient serverClient)
        {
            this.clients.Remove(serverClient);
        }
        /// <summary>
        /// Sends user list to all current editor users.
        /// </summary>
        internal void SendUpdateUserList()
        {
            foreach (ServerClient serverClient in clients)
            {
                serverClient.SendUserList();
            }
        }
        /// <summary>
        /// sends product. list to all clients.
        /// </summary>
        public void sendUpdateProductList()
        {
            foreach(ServerClient serverClient in clients)
            {
                try
                {
                    serverClient.SendProductList(null);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
