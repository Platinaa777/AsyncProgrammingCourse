
SynchronizationContext.SetSynchronizationContext(new MySynchronizationContext());

var task = CalculateFactorial(5);

Console.WriteLine(await task);


async Task<int> CalculateFactorial(int x)
{
    var task = new Task<int>(() =>
    {
        int multi = 1;
        while (x > 1)
        {
            multi *= x;
            x -= 1;
        }

        return multi;
    });
    Console.WriteLine($"BEFORE AWAIT | ID THREAD: {Thread.CurrentThread.ManagedThreadId}, NAME: {Thread.CurrentThread.Name ?? "Name is null"}, FROM THREAD POOL: {Thread.CurrentThread.IsThreadPoolThread}");
    task.Start();
    var res = await task;
    Console.WriteLine($"AFTER AWAIT | ID THREAD: {Thread.CurrentThread.ManagedThreadId}, NAME: {Thread.CurrentThread.Name}, FROM THREAD POOL: {Thread.CurrentThread.IsThreadPoolThread}");
    return res;
}

class MySynchronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        var task = new Thread(() => d(state));
        task.Name = "FromMySyncronizationContext";
        task.Start();
    }
}