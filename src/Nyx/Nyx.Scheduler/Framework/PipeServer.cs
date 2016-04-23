using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NamedPipeWrapper;

namespace Nyx.Scheduler.Framework
{
    public class PipeServer : PipeCommunicator
    {
        private NamedPipeServer<PipeMessage> _server;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public PipeServer()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancelToken = _cancellationTokenSource.Token;
        }

        public CancellationToken CancelToken { get; }

        private NamedPipeServer<PipeMessage> SetupServer()
        {
            var server = new NamedPipeServer<PipeMessage>(PipeName);
            server.Error += Server_Error; ;
            server.ClientConnected += Server_ClientConnected;
            server.ClientMessage += Server_ClientMessage;
            return server;
        }

        private void Server_ClientMessage(NamedPipeConnection<PipeMessage, PipeMessage> connection, PipeMessage message)
        {
            if (message.ShutdownRequest)
            {
                Console.WriteLine("Shutdown requested by client " + connection.Id);
                _cancellationTokenSource.Cancel();
            }
        }

        private void Server_ClientConnected(NamedPipeConnection<PipeMessage, PipeMessage> connection)
        {
            Console.WriteLine("Client {0} is now connected.", connection.Id);
            connection.PushMessage(new PipeMessage() { Ack = true });
        }

        private void Server_Error(Exception exception)
        {
            Console.Error.WriteLine(exception.ToString());
            _server.Stop();
        }

        public IDisposable Listen()
        {
            _server = SetupServer();
            return new ServerManager(this);
        }

        public void Start()
        {
            _server.Start();
        }

        public void Stop()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                _server.PushMessage(new PipeMessage { ShuttedDown = true });
                Console.WriteLine("Waiting 3 seconds...");
                Task.Delay(TimeSpan.FromSeconds(3)).Wait();
            }
        }

        private class ServerManager : IDisposable
        {
            private readonly PipeServer _server;

            public ServerManager(PipeServer pipeServer)
            {
                _server = pipeServer;
                _server.Start();
            }

            public void Dispose()
            {
                _server.Stop();
            }
        }
    }
}
