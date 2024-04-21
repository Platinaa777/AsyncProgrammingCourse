

// var task = Task.Run(() => WriteChar('!'));
//
// while (task.Status != TaskStatus.RanToCompletion)
// {
//     Console.Write('$');
//     Thread.Sleep(20);
// }
//
// Console.Write("Main finished");
//
//
//
// static void WriteChar(object symbol)
// {
//     for (int i = 0; i < 160; i++)
//     {
//         Console.Write((char)symbol);
//         Thread.Sleep(20);
//     }
// }

// -----------------------------------------------------


// List<int> list = new();
// for (int i = 0; i < 100000; i++)
// {
//     list.Add(i);
// }
//
// var task = Task.Run(() =>  SortArray(false, list.ToArray()))
//     .ContinueWith(arr =>
//     {
//         foreach (var x in arr.Result)
//         {
//             Console.Write(x + ", ");
//         }
//     });
//
// task.Wait();
//
// static int[] SortArray(bool isAscending, params int[] array)
// {
//     if (isAscending)
//         Array.Sort(array);
//     else
//         Array.Sort(array, (x, y) => y - x);
//
//     return array;
// }
        

// -------------------------------------------

var task = new Task<double>(() => FindLastFibonacciNumber(30), TaskCreationOptions.LongRunning);
var task2 = task.ContinueWith(res => Console.WriteLine(res.Result));

task.Start();
task2.Wait();

static double FindLastFibonacciNumber(int number)
{
    Func<int, double> fib = null;
    fib = (x) => x > 1 ? fib(x - 1) + fib(x - 2) : x;
    return fib.Invoke(number);
}