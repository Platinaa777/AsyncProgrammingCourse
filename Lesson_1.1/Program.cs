
// ---------------------------------------------------------


using System.Runtime.CompilerServices;

// ThreadPool.QueueUserWorkItem(new WaitCallback(WriteChat), '*');
//
// for (int i = 0; i < 160; i++)
// {
//     Console.Write('!');
//     Thread.Sleep(100);
// }
//
// void WriteChat(object? c)
// {
//     for (int i = 0; i < 160; i++)
//     {
//         Console.Write((char)c!);
//         Thread.Sleep(100);
//     }    
// }

// -------------------------------------------------------------

// ThreadWorker wr = new ThreadWorker(obj =>
// {
//     if (obj is null)
//         throw new ArgumentException();
//     
//     for (int i = 0; i < 10; i++)
//     {
//         Console.Write(obj);
//         Thread.Sleep(100);        
//     }
// });
//
// wr.Start(null);
//
// for (int i = 0; i < 10; i++)
// {
//     Console.Write('t');
//     Thread.Sleep(100);        
// }
//
// wr.Wait();
//
// class ThreadWorker
// {
//     private Action<object?> Action { get; }
//
//     public ThreadWorker(Action<object?> action)
//     {
//         Action = action;
//     }
//
//     public bool IsCompleted { get; private set; } = false;
//     public bool IsSuccess { get; private set; } = false;
//     public bool IsFaulted { get; private set; } = false;
//     public Exception? Exception { get; private set; } = null;
//
//     public void Start(object? state)
//     {
//         ThreadPool.QueueUserWorkItem(new WaitCallback(Execute), state);
//     }
//
//     public void Wait()
//     {
//         while (IsCompleted == false)
//         {
//             Thread.Sleep(100);
//         }
//
//         if (Exception is not null)
//             throw Exception;
//
//         Console.WriteLine("End...");
//     }
//
//     private void Execute(object? state)
//     {
//         try
//         {
//             Action(state);
//             IsSuccess = true;
//         }
//         catch (Exception e)
//         {
//             IsFaulted = true;
//             Exception = e;
//         }
//         finally
//         {
//             IsCompleted = true;
//         }
//     }
// }


// ------------------------------------------------------

ThreadWorker<int> wr = new ThreadWorker<int>(obj =>
{
    int num = (int)obj!;

    for (int i = 0; i < 100; i++)
    {
        Console.Write(num + "|");
        Thread.Sleep(5);
    }

    return num * 10;
});

wr.Start(5);

Console.WriteLine($"Result={wr.Result}");

class ThreadWorker<TResult>
{
    private Func<object?, TResult> Action { get; }
    private TResult? _result = default;

    public ThreadWorker(Func<object?, TResult> action)
    {
        Action = action;
        _result = default;
    }

    public bool IsCompleted { get; private set; }
    public bool Success { get; private set; }
    public Exception? Exception { get; private set; }

    public TResult Result
    {
        get
        {
            if (Exception is not null)
                throw Exception;

            while (IsCompleted == false)
            {
                Thread.Sleep(5);                
            }
            
            return _result!;
        }
    }

    public void Start(object? state)
    {
        ThreadPool.QueueUserWorkItem(new WaitCallback(Execute), state);
    }

    private void Execute(object? state)
    {
        try
        {
            _result = Action(state);
            Success = true;
        }
        catch (Exception e)
        {
            Success = false;
            Exception = e;
        }
        finally
        {
            IsCompleted = true;
        }
    }
}