using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Responses
{
    internal class ResponseSender
    {
        public void SendResponse(Response response, System.Net.Sockets.TcpClient client)
        {
            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            writer.WriteLine($"HTTP/1.1 {response.Status} {response.StatusString}");
            if (!response.hasBody)
            {
                writer.WriteLine();
            }
            else
            {
                writer.WriteLine($"Content-Type: {response.ContentType}\r\n");
                var payload = Encoding.UTF8.GetBytes(response.Payload);
                writer.WriteLine($"Content-Length: {payload.Length}\r\n");
                writer.WriteLine(Encoding.UTF8.GetString(payload));
            }
        }
    }
}
