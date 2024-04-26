
SynchronizationContext.SetSynchronizationContext(new VoidExceptionHandlerSyncronizationContext());

TestMethod();

async void TestMethod()
{
    throw new Exception("my exception");
}


class VoidExceptionHandlerSyncronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        try
        {
            d.Invoke(state);
        }
        catch (Exception e)
        {
            Console.WriteLine($"EXCEPTION WAS HANDLED BY {nameof(VoidExceptionHandlerSyncronizationContext)}");
            Console.WriteLine(e);
        }
    }

    public override void OperationStarted()
    {
        Console.WriteLine("Operation started...");
    }

    public override void OperationCompleted()
    {
        Console.WriteLine("Operation completed...");
    }
}

