using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ClientApplication
{
    class KeepAliveService
    {
        public static readonly int INTERVAL_OVERLAP = 200;
        public static readonly int INTERVAL = 10000;
        private bool running { get; set; }
        private Crypto crypto;
        public KeepAliveService (Crypto crypto)
        {
            this.crypto = crypto;
            
        }

        public void Run()
        {
            this.running = true;
            Thread thread = new Thread(new ThreadStart(this.RunThread));
            thread.Start();
        }

        public void Stop()
        {
            this.running = false;
        }

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
