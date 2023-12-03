﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Responses
{
    internal class ResponseBuilder
    {
        public Response BadRequest()
        {
            return new Response(Status.BadRequest);
        }
        public Response MethodNotAllowed()
        {
            return new Response(Status.MethodNotAllowed);
        }
        public Response NotFound()
        {
            return new Response(Status.NotFound);
        }
        public Response Unauthorized()
        {
            return new Response(Status.Unauthorized);
        }

        public Response JsonResponse(object data)
        {
            return new Response(data);
        }
    }
}
