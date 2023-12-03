using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SemesterProject.Server;

namespace SemesterProject.Server.Responses
{
    internal class Response
    {
        public bool hasBody { get; }
        public int Status { get; }
        public string StatusString { get; }
        public string ?Payload { get; set; }
        public string ?ContentType { get; set; }

        public Response(Status status)
        {
            hasBody = false;
            Status = (int)status;
            StatusString = status.ToString();
        }

        public Response(object payload, Status status = Server.Status.Ok) 
        {
            string json = JsonSerializer.Serialize(payload);
            ContentType = "application/json";
            Payload = json;
            hasBody = true;
            Status=(int)status;
            StatusString = status.ToString();
        }
    }
}
