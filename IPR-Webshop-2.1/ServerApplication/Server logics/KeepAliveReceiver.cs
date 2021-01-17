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

        // Delecate which is reffering to a disconnection
        private Action onIntervalMissed;
        public KeepAliveReceiver(Action onIntervalMissed)
        {
            this.onIntervalMissed = onIntervalMissed;
            this.stopwatch = new Stopwatch();
            this.interval = int.MaxValue;
        }
        // this method will run our keep alive thread.
        public void Run()
        {
            this.running = true;
            Thread thread = new Thread(new ThreadStart(this.runThread));
            thread.Start();
        }
        // This will stop the current thread if running.
        public void Stop()
        {
            this.running = false;
        }

        private void runThread()
        {
            while (running)
            {
                // When our elapsed time is > than out interval.
                if(this.stopwatch.ElapsedMilliseconds > this.interval)
                {
                    this.onIntervalMissed();
                }
                Thread.Sleep(100);
            }
        }
        // When called our object will reset its previous interval and elapsed time.
        public void received(int interval)
        {
            if (this.stopwatch.IsRunning)
            {
                this.stopwatch.Stop();
                this.stopwatch.Reset();
            }

            this.stopwatch.Start();
            this.interval = interval;
        }
    }
}
