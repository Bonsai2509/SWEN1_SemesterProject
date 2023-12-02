using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Requests
{
    internal class RequestReader
    {
        private readonly System.Net.Sockets.TcpClient _client;

        public RequestReader(System.Net.Sockets.TcpClient client)
        {
            _client = client;
        }

        public Request ReadRequest()
        {
            using var reader = new StreamReader(_client.GetStream(), leaveOpen: true);
            Method httpMethod;
            string targetString;
            string[] target;
            string version;

            var headers = new Dictionary<string, string>();

            var contentLength = 0;
            string payload = null;
            string requestParam = null;

            try 
            { 
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    return null;
                }

                line = line.Trim();

                var firstLine = line.Split(' ');

                httpMethod = Utility.GetMethod(firstLine[0]);
                targetString = firstLine[1];
                version = firstLine[2];

                int QuestionmarkIndex;

                if (httpMethod == Method.Get && (QuestionmarkIndex = targetString.IndexOf("?")) != -1)
                {
                    requestParam = targetString[(QuestionmarkIndex + 1)..];
                    targetString = targetString[..QuestionmarkIndex];
                }

                target = targetString.Split('/');

                /*Console.WriteLine(httpMethod);
                Console.WriteLine(targetString);
                Console.WriteLine(version);
                Console.Write(requestParam);
                foreach ( var item in target)
                {
                    Console.WriteLine(item);
                }*/
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            //read all http headers
            try
            {
                string? line;
                while (!string.IsNullOrWhiteSpace(line = reader.ReadLine()))
                {
                    var header = line.Split(":");
                    headers.Add(header[0].Trim(), header[1].Trim());
                    if (header[0] == "Content-Length") contentLength = int.Parse(header[1]);
                }
                /*foreach (KeyValuePair<string, string> header in headers)
                {
                    Console.WriteLine("Key: {0}, Value: {1}\n",
                        header.Key, header.Value);
                }*/
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            //read http body if it exists (if header content-type exists)
            if (contentLength > 0 && headers.ContainsKey("Content-Type"))
            {
                const int bufferSize = 1024;
                var buffer = new char[bufferSize];
                var httpBody = new StringBuilder();
                var totalBytesRead = 0;
                while (totalBytesRead < contentLength)
                    try
                    {
                        var bytesRead = reader.Read(buffer, 0, bufferSize);
                        if (bytesRead == 0) break;
                        totalBytesRead += bytesRead;
                        httpBody.Append(buffer, 0, bytesRead);
                    }
                    catch (IOException)
                    {
                        break;
                    }

                payload = httpBody.ToString();
            }
            Console.WriteLine(payload);
            var req = new Request(httpMethod, target, version, headers, payload, requestParam);
            return req;
        }
    }
}
