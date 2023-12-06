﻿using Npgsql;
using SemesterProject.Server.Responses;
using SemesterProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SemesterProject.Server.Models;
using SemesterProject.Server.Security;
using SemesterProject.Cards;
using System.Reflection.Metadata;

namespace SemesterProject.Server.Requests.Handlers
{
    internal class GetHandler : Handler
    {
        public Response HandleGet(Request request)
        {
            switch (request.Target[0])
            {
                case "users": return GetUserByUsername(request);
                case "cards": return GetCardsByToken(request);
                /*case "deck": return GetDeckByToken(request);
                case "stats": return GetStatsByToken(request);
                case "scoreboard": return GetScoreboard(request);
                case "tradings": return GetTradingDeals(request);*/
            }
            return new ResponseBuilder().BadRequest();
        }

        private Response GetUserByUsername(Request request)
        {
            string username = request.Target[1];
            if (!(new UserAuthorizer().AuthorizeUserByUsernameInTarget(username, request)))
            {
                return new ResponseBuilder().Unauthorized();
            }
            else
            {

                try
                {
                    using var command = new NpgsqlCommand(@"SELECT ""username"", ""bio"", ""image"" FROM ""user"" WHERE ""username""=@p1;", Connection);
                    command.Parameters.AddWithValue("p1", username);
                    command.Prepare();
                    using var reader = command.ExecuteReader();
                    if(!reader.Read())
                    {
                        return new ResponseBuilder().NotFound();
                    }
                    string dbUsername = reader.GetString(0);
                    string dbBio = reader.GetString(1);
                    string dbImage = reader.GetString(2);
                    Database.DisposeDbConnection();
                    if (dbUsername == null)
                    {
                        return new ResponseBuilder().NotFound();
                    }
                    else
                    {
                        var data = new UserData(dbUsername, dbBio, dbImage);
                        return new ResponseBuilder().JsonResponse(data);
                    }
                }
                catch (Exception e)
                {
                    Database.DisposeDbConnection();
                    Console.WriteLine(e.Message);
                    return new ResponseBuilder().BadRequest();
                }
            }
        }

        private Response GetCardsByToken(Request request)
        {
            var security = new UserAuthorizer();
            
            if(!security.RequestContainsToken(request))
            {
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                var utils = new Utility();
                string token = utils.ExtractTokenFromString(request.Headers["Authorization"]);
                string username = utils.ExtractUsernameFromToken(token);
                if(!security.AuthorizeUserByToken(request))
                {
                    return new ResponseBuilder().Unauthorized();
                }
                else
                {
                    var cardList = new List<CardData>();
                    using var command = new NpgsqlCommand(@"SELECT ""cardindex"", ""cardid""  FROM ""stack"" WHERE ""username""=@p1;", Connection);
                    command.Parameters.AddWithValue("p1", username);
                    command.Prepare();
                    var cardBuilder = new CardBuilder();
                    using var reader = command.ExecuteReader();
                    try
                    {
                        while(reader.Read())
                        {
                            int index = reader.GetInt16(0);
                            var card = cardBuilder.generateCard(index);
                            var cardSchema = new CardData(
                                reader.GetGuid(1),
                                card.Name,
                                card.Damage
                                );
                            cardList.Add(cardSchema);
                        }
                        return new ResponseBuilder().JsonResponseCardList(cardList);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return new ResponseBuilder().InternalServerError();
                    }
                    catch(NotImplementedException)
                    {
                        return new ResponseBuilder().InternalServerError();
                    }
                }
            }
        }

        /*private Response GetDeckByToken(Request request)
        {

        }

        private Response GetStatsByToken(Request request)
        {

        }

        private Response GetScoreboard(Request request)
        {

        }

        private Response GetTradingDeals(Request request)
        {

        }*/
    }
}
