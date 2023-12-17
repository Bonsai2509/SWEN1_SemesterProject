using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Server.Models;

namespace SemesterProject.Server.Responses
{
    public class ResponseBuilder
    {
        public Response OK()
        {
            return new Response(Status.Ok);
        }
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
        public Response Forbidden()
        {
            return new Response(Status.Forbidden);
        }
        public Response Conflict()
        {
            return new Response(Status.Conflict);
        }
        public Response Created()
        {
            return new Response(Status.Created);
        }
        public Response NoContent()
        {
            return new Response(Status.NoContent);
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
        public Response JsonResponseUserTradeList(List<TradeDetails> data)
        {
            return new Response(data);
        }

        public Response PlainTextResponse(string plainText)
        {
            return new Response(plainText);
        }
    }
}
