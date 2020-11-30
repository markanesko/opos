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
                    
            CancellationTokenSource cts1 = new CancellationTokenSource(1);
            CancellationTokenSource cts2 = new CancellationTokenSource(1);
            CancellationTokenSource cts3 = new CancellationTokenSource(1);
            
            a.ScheduleTask(ta, Scheduler.Priority.HIGH, cts1);
            a.ScheduleTask(tb, Scheduler.Priority.HIGH, cts2);
            a.ScheduleTask(tc, Scheduler.Priority.HIGH, cts3);


            a.AbortAllThatRequireCancelation();

            while (a.GetNumberOfTasks() > 0)
            {
                Task.Delay(1000).Wait();
            }



            Scheduler b = new Scheduler(4);

            for (int i = 0; i < 15; i++)
            {
                int value = i;

                CancellationTokenSource cancelTokenSource = new CancellationTokenSource(5000);

                Task toStart = new Task(() =>
                    {
                        Task.Delay(value * 1000).Wait();
                        Console.WriteLine("baka windonz " + value.ToString());
                    }
                );

                Scheduler.Priority prior;

                if (value % 5 == 0)
                {
                    prior = Scheduler.Priority.NORMAL;
                }

                if (value % 7 == 0)
                {
                    prior = Scheduler.Priority.LOW;
                }
                else
                {
                    prior = Scheduler.Priority.HIGH;
                }


                b.ScheduleTask(toStart, prior, cancelTokenSource);

            }

            CancellationTokenSource canc = new CancellationTokenSource(500);

            Task t1 = new Task(() =>
            {
                //Task.Delay(10000).Wait(); Console.WriteLine("baka windonzHIGH");
                for (int i = 0; i < 10; i++)
                {
                    if (canc.Token.IsCancellationRequested)
                    {
                        break;
                    }
                    else
                    {
                        Task.Delay(1000).Wait();
                        Console.WriteLine("still not done windonzs");
                    }
                }

            }, canc.Token
            );

            b.ScheduleTask(t1, Scheduler.Priority.NORMAL, canc);

            Task.Delay(1000).Wait();

            b.AbortTask(t1);

            b.AbortAllThatRequireCancelation();


            while (b.GetNumberOfTasks() > 0)
            {
                Task.Delay(1000).Wait();
            }
            Console.WriteLine("done? windonz");

        }                     
    }
}
