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
        // logging object
        private LogField log;
        // label for indicating the serverstate
        private ServerStatusLabel ServerStatusLabel;
        // Status
        public bool Running;
        public Server(LogField log, ServerStatusLabel serverStatusLabel)
        {
            this.log = log;
            this.ServerStatusLabel = serverStatusLabel;

            // Sets the label to init
            this.ServerStatusLabel.SetStatus(ServerStates.Init);

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
            clients.Add(new ServerClient(tcpClient));
            // Start async for accepting clients
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), null);   
        }
        // Starts the server
        public void StartServer()
        {
            log.PrintLine(SOURCE_LABEL, "starting server!");

            this.Running = true;
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
            log.PrintLine(SOURCE_LABEL, "stopping server!");

            this.Running = false;
            // foreach client connected to the server
            foreach(ServerClient client in clients)
            {
                // Disposes the TcpClient and NetworkStream
                client.Disconect();
            }
            clients.Clear();
            // Stop listening to new tcp connections
            this.tcpListener.Stop();
            // Indicate user of serverstate
            this.ServerStatusLabel.SetStatus(ServerStates.Stopped);
        }
    }
}
