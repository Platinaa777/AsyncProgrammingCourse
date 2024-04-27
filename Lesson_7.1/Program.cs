namespace Lesson_7._1
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine($"Id основного потока: {Thread.CurrentThread.ManagedThreadId}");

            Thread thread = new Thread(DrawSymbols);
            thread.Start();

            Thread.Sleep(500);

            while (true)
            {
                Console.Write($" {Thread.CurrentThread.ManagedThreadId} ");
                Thread.Sleep(350);
            }
        }

        private static void DrawSymbols()
        {
            Console.WriteLine($"Id вторичного потока: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(500);

            int loopCounter = 0;

            while (true)
            {
                Console.Write($" {Thread.CurrentThread.ManagedThreadId} ");
                Thread.Sleep(350);

                try
                {
                    if (loopCounter++ == 12)
                    {
                        throw new Exception("Превышено количество итераций!");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception was handled");
                    break;
                }
                
            }
        }
    }
}