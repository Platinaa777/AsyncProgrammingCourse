var scheduler = new StackTaskScheduler();

List<Task> tasks = new();
for (int i = 0; i < 40; ++i)
{
    tasks.Add(new Task(() =>
    {
        Thread.Sleep(2000);
        Console.WriteLine($"Задача с номером {Task.CurrentId} выполнилась выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}");
    }));
}

for (int i = 0; i < 40; ++i)
    tasks[i].Start(scheduler);

Task.WaitAll(tasks.ToArray());

class StackTaskScheduler : TaskScheduler
{
    private Stack<Task> stack = new();
    
    protected override IEnumerable<Task>? GetScheduledTasks()
    {
        lock (stack)
        {
            return stack;
        }
    }

    protected override void QueueTask(Task task)
    {
        lock (stack)
        {
            stack.Push(task);
        }

        Console.WriteLine($"TASK ID: {task.Id}");
        ThreadPool.QueueUserWorkItem(Execute, null);
    }

    private void Execute(object? state)
    {
        // Thread.Sleep(5000);
        Task? topTask = null;

        lock (stack)
        {
            if (stack.Count == 0)
                return;

            topTask = stack.Peek();
            stack.Pop();
        }
        
        if (topTask is null)
            return;

        Console.WriteLine($"Задача была поставлена на выполнение планировщиков: taskId={topTask.Id}");
        TryExecuteTask(topTask);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        Console.WriteLine($"Выполняем задачу синхронно: {task.Id}");
        return TryExecuteTask(task);
    }
}


























