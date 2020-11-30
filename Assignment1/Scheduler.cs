using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment1
{
    public class Scheduler : TaskScheduler
    {
        public enum Priority : UInt16
        {
            HIGH = 2,
            NORMAL = 7,
            LOW = 19,
        }

        private int _maxTasksAllowed;

        private readonly List<(Task task, Priority priority, CancellationTokenSource cT)> _queuedTasks = new List<(Task, Priority, CancellationTokenSource)>();

        private readonly List<Task> _executingTasks = new List<Task>();

        

        public Scheduler(int maxTasksAllowed)
        {
            _maxTasksAllowed = maxTasksAllowed;
        }

        public void PrintSomething()
        {
            Console.WriteLine("windonz...");
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            lock (_queuedTasks)
            {
                AbortAllThatRequireCancelation();
                if (_executingTasks.Count < _maxTasksAllowed)
                {
                    Task toRun = GetHighestPriorityTask();
                    if (TryDequeue(toRun) && !_executingTasks.Contains(toRun))
                    {
                        _executingTasks.Add(toRun);
                        PushTaskToThreadPool(toRun);
                    }
                }
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected override bool TryDequeue(Task task)
        {
            lock (_queuedTasks) return _queuedTasks.Remove(FindTask(task));
        }

        private (Task, Priority, CancellationTokenSource) FindTask(Task task)
        {
            return _queuedTasks.Find((x) => x.task.GetHashCode() == task.GetHashCode());
        }

        private void PrettyPrintPriority(Priority pr)
        {
            if (pr == Priority.HIGH)
            {
                Console.WriteLine("HIGH");
            }
            else if (pr == Priority.NORMAL)
            {
                Console.WriteLine("NORMAL");
            }
            else
            {
                Console.WriteLine("LOW");
            }
        }
    
        private void PrintTaskPriority(Task task)
        {
            PrettyPrintPriority(_queuedTasks.Find((x) => x.task.GetHashCode() == task.GetHashCode()).priority);
        }
        private void PushTaskToThreadPool(Task task)
        {
            task.Start();
            task.ContinueWith(_ => {
                lock (_executingTasks) _executingTasks.Remove(task);
                // signal task scheduler to run new tasks
                QueueTask(task);
            });
        }
        
        private void DelteFromExecutingCallback(Task task)
        {
            lock (_executingTasks) _executingTasks.Remove(task);
        }

        private Task GetHighestPriorityTask()
        {
            lock (_queuedTasks)
            {
                if (_queuedTasks.FindAll((x) => x.priority == Priority.HIGH).Count != 0)
                {
                    return _queuedTasks.FindAll((x) => x.priority == Priority.HIGH).First().task;
                }
                else if (_queuedTasks.FindAll((x) => x.priority == Priority.NORMAL).Count != 0)
                {
                    return _queuedTasks.FindAll((x) => x.priority == Priority.NORMAL).First().task;
                }
                else if (_queuedTasks.FindAll((x) => x.priority == Priority.LOW).Count != 0)
                {
                    return _queuedTasks.FindAll((x) => x.priority == Priority.LOW).First().task;
                }
                else
                {
                    return null;
                }
            }
        }

        public void ScheduleTask(Task task, Priority priority, CancellationTokenSource cts  )
        {

            lock (_queuedTasks)
            {
                _queuedTasks.Add((task, priority, cts));
            }

            QueueTask(task);
        }

        public int GetNumberOfTasks()
        {
            return _queuedTasks.Count + _executingTasks.Count;
        }

        public void AbortAllThatRequireCancelation()
        {
            lock (_queuedTasks)
            {
                _queuedTasks.FindAll((x) => x.cT.IsCancellationRequested == true).ForEach((x) => 
                {
                    AbortSingleTask(x);
                });
            }
        }


        private void AbortSingleTask((Task, Priority, CancellationTokenSource) toPrevent)
        {
            toPrevent.Item3.Cancel();
            TryDequeue(toPrevent.Item1);
            DelteFromExecutingCallback(toPrevent.Item1);
        }

        public void AbortTask(Task task)
        {
            lock (_queuedTasks)
            {
                (Task, Priority, CancellationTokenSource) toPrevent = FindTask(task);
                AbortSingleTask(toPrevent);
            }
        }
    }
}
