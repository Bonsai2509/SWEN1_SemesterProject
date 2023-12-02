using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Server.Responses;

namespace SemesterProject.Server.Requests
{
    internal class RequestRouter
    {
        public Response HandleRequest(Request request)
        {
            var response = new Response();
            switch(request.Target[0])
            {
                case "": Get(request);
                    break;
            }
        }
    }

}
