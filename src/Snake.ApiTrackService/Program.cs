using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.ApiTrackService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start SnakeServiceServer service...");
            var server = new SnakeServiceServer();
            server.Start();

            Console.ReadLine();
        }
    }
}
