using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment1.Demo
{
    class ThreadPoolSchedluer
    {
        public void RunThreadPoolScheduler()
        {
            SchedulerThreadPool lcts = new SchedulerThreadPool(2);
            List<Task> tasks = new List<Task>();

            // Create a TaskFactory and pass it our custom scheduler.
            TaskFactory factory = new TaskFactory(lcts);
            CancellationTokenSource cts = new CancellationTokenSource();

            // Use our factory to run a set of tasks.
            Object lockObj = new Object();
            int outputItem = 0;

            for (int tCtr = 0; tCtr <= 4; tCtr++)
            {
                int iteration = tCtr;
                Task t = factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        lock (lockObj)
                        {
                            Console.Write("{0} in task t-{1} on thread {2}   ",
                                          i, iteration, Thread.CurrentThread.ManagedThreadId);
                            outputItem++;
                            if (outputItem % 3 == 0)
                                Console.WriteLine();
                        }
                    }
                }, cts.Token);
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
            cts.Dispose();
            Console.WriteLine("\n\nSuccessful completion.");
        }

        

    }
}
