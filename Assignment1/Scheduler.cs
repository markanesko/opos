using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment1
{
    public class Scheduler : System.Threading.Tasks.TaskScheduler
    {
        UInt32 counter;

        public enum Priority : UInt16
        {
            HIGH = 2,
            NORMAL = 7,
            LOW = 19,
        }

        private int _maxTasksAllowed;

        private readonly List<(Task task, Priority priority)> _queuedTasks = new List<(Task, Priority)>();

        private readonly Queue<Task> _executingTasks = new Queue<Task>();

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
            Console.WriteLine("queue trasks");
            lock(_queuedTasks)
            {
                _queuedTasks.Add((task, Priority.HIGH));

                if (_executingTasks.Count < _maxTasksAllowed)
                {
                    PushTaskToThreadPool();
                }
            }

            //throw new NotImplementedException();
        }
        
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine("execute inline");
            return false;
        }

        protected override bool TryDequeue(Task task)
        {
            lock (_queuedTasks) return _queuedTasks.Remove(FindTask(task));
        }

        private (Task, Priority) FindTask(Task task)
        {
            return _queuedTasks.Find((x) => x.task.GetHashCode() == task.GetHashCode());
        }

        private void PushTaskToThreadPool()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                try
                {
                    Task toRun;
                    lock (_queuedTasks)
                    {
                        if(_queuedTasks.Count == 0)
                        {
                            return;
                        }

                        toRun = GetHighestPriorityTask();
                        if (TryDequeue(toRun))
                        {
                            Console.WriteLine("successfull");
                        }
                    }
                    Console.WriteLine("trying to execute task");
                    base.TryExecuteTask(toRun);
                }
                catch (Exception e)
                {   
                    // TODO: Deal with exceptions
                    Console.WriteLine(e);
                }

            }, null);

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

        public void ScheduleTask(Task task, Priority priority, UInt32 maxDurationTime)
        {
            lock (_queuedTasks)
            {
                _queuedTasks.Add((task, priority));
            }

            QueueTask(task);
        }

        public int GetNumberOfTasks()
        {
            return _queuedTasks.Count + _executingTasks.Count;
        }
    }
}
