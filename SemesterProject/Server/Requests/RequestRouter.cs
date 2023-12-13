using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Server.Responses;
using SemesterProject.Server.Requests.Handlers;
using SemesterProject.Server.Responses;

namespace SemesterProject.Server.Requests
{
    internal class RequestRouter
    {
        public Response HandleRequest(Request request)
        {
            if (request == null) return new ResponseBuilder().BadRequest();
            switch (request.Method)
            {
                case Method.Get: return new GetHandler().HandleGet(request);
                case Method.Delete: return new DeleteHandler().HandleDelete(request);
                case Method.Post: return new PostHandler().HandlePost(request);
                case Method.Put: return new PutHandler().HandlePut(request);
            }
            return new ResponseBuilder().MethodNotAllowed();
        }
    }
}
