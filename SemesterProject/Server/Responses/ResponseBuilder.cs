using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Server.Models;

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
        public Response InternalServerError()
        {
            return new Response(Status.InternalServerError);
        }

        public Response JsonResponse(object data)
        {
            return new Response(data);
        }
        public Response JsonResponseCardList(List<CardData> data)
        {
            return new Response(data);
        }
        public Response JsonResponseUserScoreList(List<UserScores> data)
        {
            return new Response(data);
        }

        public Response PlainTextResponse(string plainText)
        {
            return new Response(plainText);
        }
    }
}
