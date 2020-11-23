using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Assignment1;

namespace Assignment1.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Scheduler a = new Scheduler(5);

            Task ta = new Task(() => { Task.Delay(1000).Wait(); Console.WriteLine("omg windonzzz"); });
            Task tb = new Task(() => { Task.Delay(5000).Wait(); Console.WriteLine("omg wsindonzzz"); });
            Task tc = new Task(() => { Task.Delay(1000).Wait(); Console.WriteLine("omg wiaandonzzz"); });


            a.ScheduleTask(ta, Scheduler.Priority.HIGH, 1);
            a.ScheduleTask(tb, Scheduler.Priority.HIGH, 1);
            a.ScheduleTask(tc, Scheduler.Priority.HIGH, 1);

            while (a.GetNumberOfTasks() > 0)
            {
                Task.Delay(1000).Wait();
            }

            

            Scheduler b = new Scheduler(4);

            for (int i = 0; i < 15; i++)
            {
                Task t = new Task(() =>
                {
                    Task.Delay(i * 100).Wait(); Console.WriteLine("omg windonzzz: " + i);
                });
                if (i % 2 == 0) 
                {
                    Console.WriteLine("added high");
                    b.ScheduleTask(t, Scheduler.Priority.HIGH, 1);
                } 
                else if (i % 3 == 0)
                {
                    Console.WriteLine("added normal");
                    b.ScheduleTask(t, Scheduler.Priority.NORMAL, 1);
                }
                else
                {
                    Console.WriteLine("added low");
                    b.ScheduleTask(t, Scheduler.Priority.LOW, 1);
                }
                
            }

            while (b.GetNumberOfTasks() > 0)
            {
                Task.Delay(1000).Wait();
            }
            Console.WriteLine("done? windonz");


            //SchedulerThreadPool lcts = new SchedulerThreadPool(2);
            //List<Task> tasks = new List<Task>();

            //// Create a TaskFactory and pass it our custom scheduler.
            //TaskFactory factory = new TaskFactory(lcts);
            //CancellationTokenSource cts = new CancellationTokenSource();

            //// Use our factory to run a set of tasks.
            //Object lockObj = new Object();
            //int outputItem = 0;

            //for (int tCtr = 0; tCtr <= 4; tCtr++)
            //{
            //    int iteration = tCtr;
            //    Task t = factory.StartNew(() => {
            //        for (int i = 0; i < 1000; i++)
            //        {
            //            lock (lockObj)
            //            {
            //                Console.Write("{0} in task t-{1} on thread {2}   ",
            //                              i, iteration, Thread.CurrentThread.ManagedThreadId);
            //                outputItem++;
            //                if (outputItem % 3 == 0)
            //                    Console.WriteLine();
            //            }
            //        }
            //    }, cts.Token);
            //    tasks.Add(t);
            //}

            //Task.WaitAll(tasks.ToArray());
            //cts.Dispose();
            //Console.WriteLine("\n\nSuccessful completion.");

        }                     
    }
}
