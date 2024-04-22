using Console = System.Console;
using Task = System.Threading.Tasks.Task;
using TaskScheduler = System.Threading.Tasks.TaskScheduler;
using ThreadPool = System.Threading.ThreadPool;

namespace TaskSchedulerFunctionality
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine($"Id потока метода Main - {Thread.CurrentThread.ManagedThreadId}");

            Task[] tasks = new Task[10];
            ReviewTaskScheduler reviewTaskScheduler = new ReviewTaskScheduler();

            QueueTaskTesting(tasks, reviewTaskScheduler);
            //TryExecuteTaskInlineTesting(tasks, reviewTaskScheduler);
            //TryDequeueTesting(tasks, reviewTaskScheduler);
            
            try
            {
                Task.WaitAll(tasks);
            }
            catch
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Несколько задач были отменены!");
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine($"Метод Main закончил свое выполнение");
            }

            Console.ReadKey();
        }

        private static void QueueTaskTesting(Task[] tasks, TaskScheduler scheduler)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}\n");
                });

                tasks[i].Start(scheduler);
            }
        }

        private static void TryExecuteTaskInlineTesting(Task[] tasks, TaskScheduler scheduler)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task<int>(() =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}\n");
                    return 1;
                });
            }

            foreach (var task in tasks)
            {
                task.Start(scheduler);
                task.Wait();
                //int result = ((Task<int>)task).Result;
            }
        }

        private static void TryDequeueTesting(Task[] tasks, TaskScheduler scheduler)
        {
            #region Скоординированная отмена

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            cts.CancelAfter(555);

            #endregion

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}\n");
                }, token);

                tasks[i].Start(scheduler);
            }
        }
    }
}

namespace TaskSchedulerFunctionality
{
    internal class ReviewTaskScheduler : TaskScheduler
    {
        private readonly LinkedList<Task> tasksList = new LinkedList<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return tasksList;
        }

        /// <summary>
        /// Метод вызывается методом Start класса Task
        /// </summary>
        /// <param name="task"></param>
        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"    [QueueTask] Задача #{task.Id} поставлена в очередь..");
            tasksList.AddLast(task);
            ThreadPool.QueueUserWorkItem(ExecuteTasks, null);
            // ExecuteTasks(null);
        }

        /// <summary>
        /// Метод вызывается методами ожидания, к примеру Wait, WaitAll...
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskWasPreviouslyQueued"></param>
        /// <returns></returns>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"        [TryExecuteTaskInline] Попытка выполнить задачу #{task.Id} синхронно..");

            lock (tasksList)
            {
                tasksList.Remove(task);
            }

            return base.TryExecuteTask(task);
        }

        /// <summary>
        /// Метод вызывается при отмене выполнения задачи
        /// </summary>
        /// <param name="task"></param> 
        /// <returns></returns>
        protected override bool TryDequeue(Task task)
        {
            Console.WriteLine($"            [TryDequeue] Попытка удалить задачу {task.Id} из очереди..");
            bool result = false;

            lock (tasksList)
            {
                result = tasksList.Remove(task);
            }

            if (result == true)
            {
                Console.WriteLine(
                    $"                [TryDequeue] Задача {task.Id} была удалена из очереди на выполнение..");
            }

            return result;
        }

        private void ExecuteTasks(object _)
        {
            while (true)
            {
                //Thread.Sleep(2000); // Убрать комментарий для проверки TryExecuteTaskInline
                Task task = null;

                lock (tasksList)
                {
                    if (tasksList.Count == 0)
                    {
                        break;
                    }

                    task = tasksList.First.Value;
                    tasksList.RemoveFirst();
                }

                if (task == null)
                {
                    break;
                }

                base.TryExecuteTask(task);
            }
        }
    }
}

