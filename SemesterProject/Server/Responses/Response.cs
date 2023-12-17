using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SemesterProject.Server;
using SemesterProject.Server.Models;

namespace SemesterProject.Server.Responses
{
    public class Response
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
            string json = JsonConvert.SerializeObject(payload);
            ContentType = "application/json";
            Payload = json;
            hasBody = true;
            Status=(int)status;
            StatusString = status.ToString();
        }
        //needed a specific ones for objects since "List<object> payload" threw an error
        public Response(List<CardData> payload, Status status = Server.Status.Ok) 
        {
            string json = JsonConvert.SerializeObject(payload);
            ContentType = "application/json";
            Payload = json;
            hasBody = true;
            Status=(int)status;
            StatusString = status.ToString();
        }
        public Response(List<UserScores> payload, Status status = Server.Status.Ok) 
        {
            string json = JsonConvert.SerializeObject(payload);
            ContentType = "application/json";
            Payload = json;
            hasBody = true;
            Status=(int)status;
            StatusString = status.ToString();
        }
        public Response(string payload, Status status = Server.Status.Ok) 
        {
            ContentType = "text/plain";
            Payload = payload;
            hasBody = true;
            Status=(int)status;
            StatusString = status.ToString();
        }
    }
}
