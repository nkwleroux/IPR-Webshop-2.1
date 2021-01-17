using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Shared
{
    class KeepAliveService
    {
        // This value will be added to our interval for the server.
        public static readonly int INTERVAL_OVERLAP = 200;
        // This value represents out 
        public static readonly int INTERVAL = 1000;
        private bool running { get; set; }
        private Crypto crypto;
        public KeepAliveService (Crypto crypto)
        {
            this.crypto = crypto;
            
        }
        // This method will kickoff its internal start logic.
        public void Run()
        {
            this.running = true;
            Thread thread = new Thread(new ThreadStart(this.RunThread));
            thread.Start();
        }
        // This method will stop our thread if running.
        public void Stop()
        {
            this.running = false;
        }
        // This method will start our keep alive probe. this thread will send "im alive!" messages to our server.
        private void RunThread()
        {
            dynamic data = new
            {
                nextAliveTime = INTERVAL + INTERVAL_OVERLAP
            };

            dynamic message = DataProtocol.getJsonMessage("client/alive", data);

            while (running)
            {
                this.crypto.WriteTextMessage(message);
                Thread.Sleep(INTERVAL);
            }
        }
    }
}
