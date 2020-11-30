# OPOS Assignments

## Assignment 1
##### Folder structure:

```
Assignments    
│   
│   Assignments.sln
└───Assignment1
│   │   Scheduler.cs
│   │   SchedulerThreadPool.cs
└───Assignment1.Demo
│   │   Program.cs
│   │   ThreadPoolScheduler.cs
└───Assignment1.Tests
│   │   SchedulerSchedulerTests.cs
```

## Run demo and tests
Open visual studio solution file  ``` Assignments.sln```.

Run Assignment.Demo project or run Assignment.Tests from test tool.

More info on running tests is here [Tests in C# and visual studio](https://docs.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2019#build-and-run-the-test)

## Use API

Its rather simple

First add reference in your project to the library provided in this repo: [Add reference to project](https://docs.microsoft.com/en-us/dotnet/core/tutorials/library-with-visual-studio#add-a-project-reference)

Next initialize Scheduler:
```
using Assignment1;
...
Scheduler a = new Scheduler(5);
...
```
After that create some tasks to schedule:
```
Task task = new Task(() => { Task.Delay(1000).Wait(); Console.WriteLine("omg windonzzz baka windonz"); });
```
And CancellationTokenSource:
```
CancellationTokenSource cts1 = new CancellationTokenSource(1);
```

And after that all you have to do is schedule your task(s):
```
a.ScheduleTask(ta, Scheduler.Priority.HIGH, cts1);
```

If you want to allow tasks to stop from scheduling you must add CancellationToken in task that you are willing to run.
```
CancellationTokenSource canc = new CancellationTokenSource(500);

Task t1 = new Task(() =>
{
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
```
And when you are ready to close task simply:
```
a.AbortTask(t1);
```
Or remove all tasks that are marked for canceling:
```
a.AbortAllThatRequireCancelation();
```

There is one more method that is given in this API and that is:
```
while (a.GetNumberOfTasks() > 0)
{
    Task.Delay(1000).Wait();
}
```
`GetNumberOfTasks` returns how much tasks are left to scheduler and/or are executing. So when that number reaches zero that means that scheduler is empty.

## API:

`public Scheduler(int maxTasksAllowed)` scheduler constructor, used to create new scheduler

`public void ScheduleTask(Task task, Priority priority, CancellationTokenSource cts)` method used to schedule tasks to scheduler

`public int GetNumberOfTasks()` method used to get number of tasks on queue waiting to be executed or that are already executing

`public void AbortTask(Task task)` method used to abort task that is executing

`public void AbortAllThatRequireCancelation()` method used to abort all tasks that are marked for cancellation

Also, following enum is used to define priorities for scheduler
```
public enum Priority : UInt16
{
    HIGH = 2,
    NORMAL = 7,
    LOW = 19,
}
```

## Repo
Repo link: [Link](https://github.com/markanesko/opos)
