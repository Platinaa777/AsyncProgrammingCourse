
var scheduler = new DelayTaskScheduler();

var task = new Task(() =>
    {
        Console.WriteLine($"\nЗадача ыполнилась выполнилась в потоке {Thread.CurrentThread.ManagedThreadId} из пула потоков - {Thread.CurrentThread.IsThreadPoolThread}");
    });

task.Start(scheduler);

while (!task.IsCompleted)
{
    Console.Write($"* ");
    Thread.Sleep(100);
}

class DelayTaskScheduler : TaskScheduler
{
    protected override IEnumerable<Task>? GetScheduledTasks()
    {
        return Enumerable.Empty<Task>();
    }

    protected override void QueueTask(Task task)
    {
        Timer timer = new Timer(DelayCallBack, task, 2000, 0);
    }

    private void DelayCallBack(object? state)
    {
        TryExecuteTask((Task)state!);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return false;
    }
}