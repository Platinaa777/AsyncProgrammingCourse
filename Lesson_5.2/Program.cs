





SynchronizationContext.SetSynchronizationContext(new MySynchronizationContext());

var task = CalculateFactorial(15);

Console.WriteLine(await task);


async Task<int> CalculateFactorial(int x)
{
    var task = Task.Run(() =>
    {
        Thread.Sleep(1000); // for completion in thread pool
        int multi = 1;
        while (x > 1)
        {
            multi *= x;
            x -= 1;
        }

        return multi;
    });
    Console.WriteLine($"BEFORE AWAIT | ID THREAD: {Thread.CurrentThread.ManagedThreadId}, NAME: {Thread.CurrentThread.Name ?? "Name is null"}, FROM THREAD POOL: {Thread.CurrentThread.IsThreadPoolThread}, TASK ID {Task.CurrentId}");
    var res = await task;
    Console.WriteLine($"AFTER AWAIT | ID THREAD: {Thread.CurrentThread.ManagedThreadId}, NAME: {Thread.CurrentThread.Name}, FROM THREAD POOL: {Thread.CurrentThread.IsThreadPoolThread}, TASK ID {Task.CurrentId}");
    return res;
}

class MySynchronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        ThreadPool.QueueUserWorkItem(new WaitCallback(d), state);
    }
}