

int number = int.Parse(Console.ReadLine()!);

Console.WriteLine(await F(number));

async Task<int> F(int x)
{
    TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

    try
    {
        _ = Task.Run(() => Calculate(x))
            .ContinueWith(res =>
            {
                Console.WriteLine("Was calculated");
                tcs.SetResult(res.Result);
            });
    }
    catch (Exception e)
    {
        tcs.TrySetException(e);
    }

    Console.WriteLine("return value");
    return await tcs.Task;
}


int Calculate(int x)
{
    int sum = 0;
    while (x > 0)
    {
        sum += x;
        x -= 1;
    }

    return sum;
}


