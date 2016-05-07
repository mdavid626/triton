using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NamedPipeWrapper;

namespace Nyx.Scheduler.Framework
{
    public class PipeClient : PipeCommunicator
    {
        private readonly CancellationTokenSource _waitToken;
        private bool _ack;

        public PipeClient()
        {
            _waitToken = new CancellationTokenSource();
        }

        public void Shutdown()
        {
            var client = new NamedPipeClient<PipeMessage>(PipeName);
            client.Error += Client_Error;
            client.ServerMessage += Client_ServerMessage;

            Console.WriteLine("Staring client...");
            client.Start();

            Console.WriteLine("Waiting for connection...");
            client.WaitForConnection(TimeSpan.FromSeconds(1));

            // wait for ack
            Console.WriteLine("Waiting for ack from scheduler...");
            Task.Delay(TimeSpan.FromSeconds(2)).Wait();

            if (_ack)
            {
                Console.WriteLine("Sending shutdown request...");
                client.PushMessage(new PipeMessage() { ShutdownRequest = true });

                // wait for max 60 seconds
                try
                {
                    Console.WriteLine("Waiting for shutting down...");
                    Task.Delay(TimeSpan.FromSeconds(60)).Wait(_waitToken.Token);
                    Console.WriteLine("Timeout exceeded.");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Scheduler shutted down.");
                    // ignored
                }
            }
            else
            {
                Console.WriteLine("Scheduler not responding. Probably not running.");
            }
        }

        private void Client_ServerMessage(NamedPipeConnection<PipeMessage, PipeMessage> connection, PipeMessage message)
        {
            if (message.Ack)
            {
                _ack = true;
            }

            if (message.ShuttedDown)
            {
                _waitToken.Cancel(false);
            }
        }

        private void Client_Error(Exception exception)
        {
            throw exception;
        }
    }
}
