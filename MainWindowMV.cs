using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RB_SO_PI1
{
    //Radosław Boczoń
    //GitHub
    //https://github.com/ManiekMelancholik/RB_SO_PI1
    public class MainWindowMV
    {
        /// <summary>
        /// Controls, wait time - time given to processor for
        /// delay is higher because of posibility of epilepsy
        /// </summary>
        private static int waitTime = 2000;

        private static int colorChangeDelay = 500;

        private static int[] taskExecutionsTimes = { 10, 2, 19, 10, 40 };

        #region FIELDS
        private static MainWindow view;

        private static StopableTask[] tasks;

        private static Brush[] brushes1 =
        {
            Brushes.Red,
            Brushes.Blue,
            Brushes.Orange,
            Brushes.Green,
            Brushes.Yellow

        };
        
        

       // private static StopableTask t1;
       // private static StopableTask t2;
        static bool[] tab = { true, true, true, true, true };
        private static Label[] labels;
        private static ProgressBar[] progressBars;
        private static bool[] completedActons;

        #endregion
        #region CONSTRUCTORS
        public MainWindowMV() { }

        public MainWindowMV(bool f)
        {

            view = new MainWindow();

            labels = new Label[]
            {
                view.L1,
                view.L2,
                view.L3,
                view.L4,
                view.L5
            };

            progressBars = new ProgressBar[]
            {
                view.L1Bar,
                view.L2Bar,
                view.L3Bar,
                view.L4Bar,
                view.L5Bar
            };

            SetThreads();

            view.Show();

            view.Closed += new EventHandler((object o, EventArgs e)=>{
                foreach (StopableTask st in tasks)
                {
                    st.End();
                }
            });
        }
        #endregion
        #region METHODS/FUNCTIONS
        private void SetThreads()
        {
            tasks = new StopableTask[5];

            completedActons = new bool[tasks.Length];

            for (int k = 0; k < tasks.Length; k++)
            {
                completedActons[k] = false;



                tasks[k] = new StopableTask(
                (i) =>
                {
                    
                    if (tab[i])
                    {
                        view.Dispatcher.Invoke(() =>
                        {
                            labels[i].Background = brushes1[i];
                            //progressBars[i].Value++;
                        });
                    }
                    else
                    {
                        view.Dispatcher.Invoke(() =>
                        {
                            labels[i].Background = Brushes.Gray;
                        });
                    }
                    tab[i] = !tab[i];
                    Thread.Sleep(colorChangeDelay);
                },


                taskExecutionsTimes[k],
                progressBars[k],
                k);
            }

        }
        void Procesing()
        {


            int repeat = 0;
            int skipCount = 0;
            int iterator = 0;
            while (repeat<5)
            {
                if (completedActons[iterator])
                {
                    skipCount++;
                }
                else
                {
                    skipCount = 0;
                    tasks[iterator].Reset();
                    Thread.Sleep(waitTime);
                    completedActons[iterator] = tasks[iterator].Stop(waitTime);
                }
                iterator++;
                if (iterator >= tasks.Length)
                {
                    iterator = 0;
                }
            }
        }
        #endregion
        private ICommand _startApp;
        #region COMMANDS
        static bool started = false;
        public ICommand startApp
        {
            get
            {
                if(_startApp is null)
                {
                    _startApp = new CMM(
                        (ex) => {
                            if (!started)
                            {
                                
                                Thread t = new Thread(()=> { MainWindowMV context = this; context.Procesing(); });
                                t.IsBackground = true;
                                t.Start();
                                started = true;
                            }
                        },
                        (cex)=>{
                            return true;
                        }
                        );
                }
                return _startApp;
            }
        }




        #endregion
    }
}
