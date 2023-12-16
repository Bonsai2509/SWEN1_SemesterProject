using Npgsql;
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
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json.Linq;

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
                case "deck": return GetDeckByToken(request);
                case "stats": return GetStatsByToken(request);
                case "scoreboard": return GetScoreboard(request);
                case "tradings": return GetTradingDeals(request);
            }
            Database.DisposeDbConnection();
            return new ResponseBuilder().BadRequest();
        }

        private Response GetUserByUsername(Request request)
        {
            string username = request.Target[1];
            if (!(new UserAuthorizer().AuthorizeUserByUsernameInTarget(username, request)))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                try
                {
                    using var command = new NpgsqlCommand(@"SELECT ""name"", ""bio"", ""image"" FROM ""user"" WHERE ""username""=@p1;", Connection);
                    command.Parameters.AddWithValue("p1", username);
                    command.Prepare();
                    using var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().NotFound();
                    }
                    string dbUsername = reader.GetString(0);
                    string dbBio = reader.GetString(1);
                    string dbImage = reader.GetString(2);
                    Database.DisposeDbConnection();
                    var data = new UserData(dbUsername, dbBio, dbImage);
                    return new ResponseBuilder().JsonResponse(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }

        private Response GetCardsByToken(Request request)
        {
            var security = new UserAuthorizer();
            if(!security.RequestContainsToken(request))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            var utils = new Utility();
            string token;
            token = utils.ExtractTokenFromString(request.Headers["Authorization"]);
            string username = utils.ExtractUsernameFromToken(token);
            if (!security.AuthorizeUserByToken(request))
            {
                Database.DisposeDbConnection();
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
                    while (reader.Read())
                    {
                        int index = reader.GetInt16(0);
                        var card = cardBuilder.generateCard(index);
                        var cardSchema = new CardData(
                            reader.GetGuid(1),
                            card.Name,
                            card.Description,
                            card.Damage
                            );
                        cardList.Add(cardSchema);
                    }
                    if (cardList.Count == 0)
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().NotFound();
                    }
                    else
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().JsonResponseCardList(cardList);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                catch (NotImplementedException)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }

        }

        private Response GetDeckByToken(Request request)
        {
            var security = new UserAuthorizer();
            var utils = new Utility();
            string token;
            if (!security.RequestContainsToken(request))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            token = utils.ExtractTokenFromString(request.Headers["Authorization"]);
            if (!security.AuthorizeUserByToken(request))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                var cardList = new List<CardData>();
                try
                {
                    string username = utils.ExtractUsernameFromToken(token);
                    using var command = new NpgsqlCommand(@"SELECT ""cardindex"", ""cardid""  FROM ""stack"" WHERE ""username""=@p1 AND ""inDeck""=@p2;", Connection);
                    command.Parameters.AddWithValue("p1", username);
                    command.Parameters.AddWithValue("p2", true);
                    command.Prepare();
                    var cardBuilder = new CardBuilder();
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int index = reader.GetInt16(0);
                        var card = cardBuilder.generateCard(index);
                        var cardSchema = new CardData(
                            reader.GetGuid(1),
                            card.Name,
                            card.Description,
                            card.Damage
                            );
                        cardList.Add(cardSchema);
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                catch (NotImplementedException)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
                if (request.RequestParam == "format=plain")
                {
                    Database.DisposeDbConnection();
                    return GetDeckByTokenPlain(cardList);
                }
                else
                {
                    Database.DisposeDbConnection();
                    return GetDeckByTokenJson(cardList);
                }
            }
        }
        private Response GetDeckByTokenJson(List<CardData> cardList)
        {
            if (cardList.Count == 0) return new ResponseBuilder().NoContent();
            else return new ResponseBuilder().JsonResponseCardList(cardList);
        }
        private Response GetDeckByTokenPlain(List<CardData> cardList)
        {
            int cardCounter = 1;
            string plainText = null;
            foreach (CardData card in cardList)
            {
                plainText = $"{plainText} Card {cardCounter}: UUID: {card.CardId}, Name: {card.CardName}, Damage: {card.CardDamage}, Description: {card.CardDescription}\n";
                cardCounter++;
            }
            if (plainText != null)
            {
                return new ResponseBuilder().PlainTextResponse(plainText);
            }
            else
            {
                return new ResponseBuilder().NoContent();
            }
        }

        private Response GetStatsByToken(Request request)
        {
            var security = new UserAuthorizer();
            var utils = new Utility();
            string token;
            try
            {
                token = utils.ExtractTokenFromString(request.Headers["Authorization"]);
            }
            catch (ArgumentOutOfRangeException)
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().BadRequest();
            }
            if (!security.AuthorizeUserByToken(request))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                try
                {
                    using var command = new NpgsqlCommand(@"SELECT ""username"", ""elo"", ""wins"", ""loses""  FROM ""user"" WHERE ""token""=@p1;", Connection);
                    command.Parameters.AddWithValue("p1", token);
                    command.Prepare();
                    using var reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().NotFound();
                    }
                    string dbUsername = reader.GetString(0);
                    double elo = reader.GetDouble(1);
                    int wins = reader.GetInt32(2);
                    int loses = reader.GetInt32(3);
                    double winLoseRatio = ((loses == 0) ? wins : wins / loses);
                    Database.DisposeDbConnection();
                    var data = new UserStats(dbUsername, elo, wins, loses, winLoseRatio);
                    return new ResponseBuilder().JsonResponse(data);
                }
                catch (Exception)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }

        private Response GetScoreboard(Request request)
        {
            var security = new UserAuthorizer();
            if (!security.AuthorizeUserByToken(request))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                var userList = new List<UserScores>();
                try
                {
                    int userCount = 0;
                    using var command = new NpgsqlCommand(@"SELECT ""username"", ""elo"" FROM ""user"" ORDER BY ""elo"" DESC;", Connection);
                    command.Prepare();
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        userCount++;
                        var user = new UserScores(
                            reader.GetString(0),
                            reader.GetDouble(1)
                            );
                        userList.Add(user);
                    }
                    if (userCount == 0)
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().NotFound();
                    }
                    else
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().JsonResponseUserScoreList(userList);
                    }
                }
                catch (Exception)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }

        private Response GetTradingDeals(Request request)
        {
            var security = new UserAuthorizer();
            if (!security.AuthorizeUserByToken(request))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                var tradeList = new List<TradeDetails>();
                try
                {
                    int tradeCount = 0;
                    using var command = new NpgsqlCommand(@"SELECT ""tradeid"", ""cardid"", ""tradedtype"", ""tradeddmg"" FROM ""trades"";", Connection);
                    command.Prepare();
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tradeCount++;
                        var trade = new TradeDetails(
                            reader.GetGuid(0),
                            reader.GetGuid(1),
                            reader.GetString(2),
                            reader.GetInt32(3)
                            );
                        tradeList.Add(trade);
                    }
                    if (tradeCount == 0)
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().NotFound();
                    }
                    else
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().JsonResponseUserTradeList(tradeList);
                    }
                }
                catch (Exception)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }
    }
}
