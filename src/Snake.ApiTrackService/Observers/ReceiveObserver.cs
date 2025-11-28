using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.ApiTrackService.Observers
{
    public class ReceiveObserver:IReceiveObserver
    {
        public Task PreReceive(ReceiveContext context)
        {
            // called immediately after the message was delivery by the transport
            Console.WriteLine("PreReceive");
            return Task.FromResult(0);
        }

        public Task PostReceive(ReceiveContext context)
        {
            // called after the message has been received and processed
            Console.WriteLine("PostReceive");
            return Task.FromResult(0);
        }

        public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
            where T : class
        {
            // called when the message was consumed, once for each consumer
            Console.WriteLine("PostConsume");
            return Task.FromResult(0);
        }

        public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan elapsed, string consumerType, Exception exception) where T : class
        {
            // called when the message is consumed but the consumer throws an exception
            Console.WriteLine("ConsumeFault");
            return Task.FromResult(0);
        }

        public Task ReceiveFault(ReceiveContext context, Exception exception)
        {
            // called when an exception occurs early in the message processing, such as deserialization, etc.
            Console.WriteLine("ReceiveFault");
            return Task.FromResult(0);
        }
    }
}
