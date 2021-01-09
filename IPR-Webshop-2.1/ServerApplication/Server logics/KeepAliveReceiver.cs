using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace ServerApplication
{
    class KeepAliveReceiver
    {
        private bool running;
        private Stopwatch stopwatch;
        private int interval;

        private Action onIntervalMissed;
        public KeepAliveReceiver(Action onIntervalMissed)
        {
            this.onIntervalMissed = onIntervalMissed;
            this.stopwatch = new Stopwatch();
            this.interval = int.MaxValue;
        }

        public void Run()
        {
            this.running = true;
            Thread thread = new Thread(new ThreadStart(this.runThread));
            thread.Start();
        }

        public void Stop()
        {
            this.running = false;
        }

        private void runThread()
        {
            while (running)
            {
                if(this.stopwatch.ElapsedMilliseconds > this.interval)
                {
                    this.onIntervalMissed();
                }
                Thread.Sleep(100);
            }
        }
        public void received(int interval)
        {
            if(this.stopwatch.IsRunning)
                this.stopwatch.Stop();

            this.stopwatch.Start();
            this.interval = interval;
        }
    }
}
