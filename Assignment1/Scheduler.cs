using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly int _maxDegreeOfParallelism;

        private readonly List<(Task task, Priority priority)> _allTasks = new List<(Task, Priority)>();

        private readonly Queue<Task> _executingTasks = new Queue<Task>();

        public Scheduler()
        {

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
            

            throw new NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }

        protected override bool TryDequeue(Task task)
        {
            lock (_allTasks) return _allTasks.Remove(findTask(task));
        }

        private (Task, Priority) findTask(Task task)
        {
            return _allTasks.Find((x) => x.task.GetHashCode() == task.GetHashCode());
        }

        public void ScheduleTask(Task task, Priority priority, UInt32 maxDurationTime)
        {

            _allTasks.Add((task, priority));

            QueueTask(task);
        }
    }
}
