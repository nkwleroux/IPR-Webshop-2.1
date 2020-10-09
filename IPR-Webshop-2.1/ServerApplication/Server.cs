using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ServerApplication
{
    public class Server
    {
        public static readonly int PORT_DEFAULT = 2000;
        // Object to listen for new tcp connections.
        private TcpListener tcpListener;
        // List with all the accepted clients.
        private List<ServerClient> clients;
        // Status
        public bool Running;
        public Server()
        {
            this.clients = new List<ServerClient>();
            this.tcpListener = new TcpListener(IPAddress.Any, PORT_DEFAULT);
        }
        private void AcceptClient(IAsyncResult ar)
        {
            if (!Running)
                return;

            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(ar);
            // Adds the new accepted tcpClient to the list
            clients.Add(new ServerClient(tcpClient));
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), null);   
        }
        // Starts the server
        public void StartServer()
        {
            this.Running = true;
            // Start to listen for connections
            this.tcpListener.Start();
            // Async way of accepting new tcp connections
            this.tcpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), null);

            Debug.WriteLine("Start server");
        }
        // Stops the server and all the connected client streams.
        public void StopServer()
        {
            this.Running = false;

            foreach(ServerClient client in clients)
            {
                // Disposes the TcpClient and NetworkStream
                client.Disconect();
            }
            clients.Clear();

            // Stop listening to new tcp connections
            this.tcpListener.Stop();

            Debug.WriteLine("Stop server");
        }
    }
}
