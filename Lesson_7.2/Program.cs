
namespace Lesson_7._2
{
    internal class Program
    {
        private static void Main()
        {
            Task task = Task.Run(Operation);

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Метод Main работает в основном потоке.");
                Thread.Sleep(400);
            }

            if (task.IsFaulted)
            {
                Console.WriteLine($"В задаче {task.Id} произошло исключение {task.Exception?.InnerExceptions[0].Message}");
            }

            Console.WriteLine($"\nПриложение корректно завершило свою работу!!!");
            Console.ReadKey();
        }

        private static void Operation()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"        Задача работает во вторичном потоке.");
                Thread.Sleep(400);

                if(i == 5)
                {
                    throw new Exception("Превышено количество итераций!");
                }
            }
        }
    }
}















