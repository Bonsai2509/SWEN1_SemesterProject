using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using SemesterProject.Server;
using SemesterProject.Server.Requests;

namespace SemesterProject.Server
{
    internal class TcpServer
    {
        private readonly System.Net.Sockets.TcpListener _server;
        private bool _isListening;

        public TcpServer(int port)
        {
            _server = new System.Net.Sockets.TcpListener(IPAddress.Loopback, port);
        }

        public void Start()
        {
            _server.Start();
            _isListening = true;

            while (_isListening)
            {
                var client = _server.AcceptTcpClient();
                Thread thread = new Thread(() =>HandleClient(client));
                thread.Start();
            }
        }
        private void HandleClient(System.Net.Sockets.TcpClient client)
        {
            var _reqReader = new RequestReader(client);
            var request = _reqReader.ReadRequest();
        }

        private Res
    }
}
