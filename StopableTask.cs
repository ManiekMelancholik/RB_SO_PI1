using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RB_SO_PI1
{
    public class StopableTask
    {
        private static readonly long miliInSec = 1000; 
        private Thread t;
        private ManualResetEvent mre;
        private readonly long maxTime;//in miliseconds
        private long milisecLeft;
        private ProgressBar progresBar;
        public delegate void task(int i);
        int num;
        private bool keepRuning;
        public StopableTask(task threadTask, int executionTimeInSec, ProgressBar progBar, int n)
        {
            keepRuning = true;
            num = n;
            progresBar = progBar;
            mre = new ManualResetEvent(false);
            maxTime = executionTimeInSec * miliInSec;
            milisecLeft = maxTime;

            t = new Thread(
                ()=> 
                {
                    while (keepRuning)
                    {
                        mre.WaitOne();
                        threadTask.Invoke(num);
                    }
                });
            t.IsBackground = true;
            t.Start();
        }

        
        public void End()
        {
            keepRuning = false;
            mre.Set();
        }
        public void Reset()
        {
            mre.Set();
        }

        public bool Stop(long milisecUsed)
        {

            milisecLeft -= milisecUsed;

            // progresBar.Value += 10;
            if (milisecLeft <= 0)
            {
                progresBar.Dispatcher.Invoke(
               () =>
               {
                   progresBar.Value = progresBar.Maximum;
                   progresBar.Foreground = Brushes.GreenYellow;
               });
            }
            else
            {
                progresBar.Dispatcher.Invoke(
              () =>
              {
                  progresBar.Value = (100 *(maxTime- milisecLeft))/(maxTime);
              });
            }

            mre.Reset();
            if (milisecLeft <= 0)
                return true;


            return false;
        }
        

    }
}
