using Snake.Client;
using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.DemoConsole
{
    class Program
    {
        private static async void RunTask(int count)
        {
            IList<Task> tasks = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                var task = Task.Run(() => 
                {
                    for (int index = 0; index < 100; index++)
                    {
                        LogProxy.Error(string.Format("{0}_{1}", "Exception: ", index), "Snake.DemoConsole", new Random().Next(1,5));
                        Console.WriteLine("Log{0} published", index);
                    }
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks.ToArray());
            Console.WriteLine("Completed...");
        }

        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Waiting for notification...");
                string cmdStr = Console.ReadLine();
                int count = StringHelper.Toint(cmdStr);
                if (count > 0)
                {
                    RunTask(count);
                }

                if (cmdStr.ToLower() == "exit")
                    break;
            }
            Console.WriteLine("Press ENTER to continue...");
            Console.ReadLine();
        }
    }
}
