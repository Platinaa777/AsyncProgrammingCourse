

using System.Collections.Concurrent;

int[] arr = new int[10_000_000];

Parallel.For(0, arr.Length, (int i) =>
{
    arr[i] = i;
});

ConcurrentBag<int> list = new();

Parallel.ForEach(arr, (int x) =>
{
    if (Is2Degree(x))
    {
        list.Add(x);
    }
});

foreach (var item in list)
{
    Console.WriteLine(item);
}

bool Is2Degree(int x)
{
    if (x <= 0)
        return false;
    
    while (x > 1)
    {
        if (x % 2 != 0)
            return false;
        x /= 2;
    }

    return true;
}