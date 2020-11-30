using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assignment1;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Assignment1.Tests
{
    [TestClass]
    public class SchedulerTests
    {
        [TestMethod]
        public void TestNumberOfQueued()
        {
            Scheduler a = new Scheduler(5);
            CancellationTokenSource cts1 = new CancellationTokenSource(10000);

            Task t1 = new Task(() =>
                {
                    Task.Delay(10000).Wait(); Console.WriteLine("baka windonzHIGH");
                }
            );

            a.ScheduleTask(t1, Scheduler.Priority.HIGH, cts1);

            Task.Delay(8000).Wait();

            int want = 1;

            int got = a.GetNumberOfTasks();

            Assert.AreEqual(want, got, "got what i expected");
        }

        [TestMethod]
        public void TestTimeExceeded()
        {
            Scheduler a = new Scheduler(5);
            CancellationTokenSource cts1 = new CancellationTokenSource(500);

            Task t1 = new Task(() =>
            {
                //Task.Delay(10000).Wait(); Console.WriteLine("baka windonzHIGH");
                for (int i = 0; i < 10; i++)
                {
                    if (cts1.Token.IsCancellationRequested)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("still not done windonzs");
                    }
                }

            }, cts1.Token
            );

            a.ScheduleTask(t1, Scheduler.Priority.HIGH, cts1);

            Task.Delay(2000).Wait();

            // tasks waits for 10 seconds
            // cancelation token is set to half second
            // after 2 seconds it is expected to have 0 tasks
            int want = 0;

            a.AbortAllThatRequireCancelation();
            
            int got = a.GetNumberOfTasks();

            Assert.AreEqual(want, got, "got what i expected");

        }
    }
}
