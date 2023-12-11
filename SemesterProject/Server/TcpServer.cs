using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using SemesterProject.Server;
using SemesterProject.Server.Requests;
using System.Data.SqlTypes;
using System.Data.Sql;
using System.Data.SqlClient;
using SemesterProject.Server.Responses;

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
            Response response = null;
            try
            {
                var _reqReader = new RequestReader(client);
                var request = _reqReader.ReadRequest();
                var router = new RequestRouter();
                response = router.HandleRequest(request);
            }
            catch(SqlException)
            {
                response = new ResponseBuilder().BadRequest();
            }
            catch(IOException)
            {
                response = new ResponseBuilder().InternalServerError();
            }
            catch (ArgumentException)
            {
                response = new ResponseBuilder().BadRequest();
            }
            catch (NullReferenceException)
            {
                response = new ResponseBuilder().InternalServerError();
            }
            Console.WriteLine(response.Status);
            Console.WriteLine(response.StatusString);
            Console.WriteLine(response.Payload);
            new ResponseSender().SendResponse(response, client);

            client.Close();
        }
    }
}
