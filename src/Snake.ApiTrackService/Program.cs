using System.IO;
using Topshelf;

namespace Snake.ApiTrackService
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

            HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();
                x.SetDescription(SnakeServiceConfiguration.ServiceDescription);
                x.SetDisplayName(SnakeServiceConfiguration.ServiceDisplayName);
                x.SetServiceName(SnakeServiceConfiguration.ServiceName);
                x.Service(factory =>
                {
                    var server = new SnakeServiceServer();
                    return server;
                });
            });
        }
    }
}
