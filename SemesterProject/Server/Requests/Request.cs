using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Requests
{
    internal class Request
    {
        public Request(Method method, string target, string version, Dictionary<string, string> headers, string payload, string requestParam)
        {
            Method = method;
            Target = target;
            Version = version;
            Headers = headers;
            Payload = payload;
            RequestParam = requestParam;
        }

        public Method Method { get; }
        public string Target { get; }
        public string Version { get; }
        public Dictionary<string, string> Headers { get; }
        public string Payload { get; }
        public string RequestParam { get; }
    }
}
