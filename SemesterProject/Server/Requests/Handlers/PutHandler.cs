using Newtonsoft.Json;
using Npgsql;
using SemesterProject.Server.Models;
using SemesterProject.Server.Responses;
using SemesterProject.Server.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Requests.Handlers
{
    internal class PutHandler : Handler
    {
        public Response HandlePut(Request request)
        {
            switch(request.Target[0])
            {
                case "users": return PutUser(request);
                case "deck": return PutDeck(request);
            }
            return new ResponseBuilder().BadRequest();
        }

        private Response PutUser(Request request)
        {
            string username = request.Target[1];
            if (!(new UserAuthorizer().AuthorizeUserByUsernameInTarget(username, request)))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                UserData userData = JsonConvert.DeserializeObject<UserData>(request.Payload);
                try
                {
                    using var command = new NpgsqlCommand(@"UPDATE ""user"" SET ""username""=@p1, ""bio""=@p2, ""image""=@p3 WHERE ""username"" = @p4;", Connection);
                    command.Parameters.AddWithValue("p1", userData.Name);
                    command.Parameters.AddWithValue("p2", userData.Bio);
                    command.Parameters.AddWithValue("p3", userData.Image);
                    command.Parameters.AddWithValue("p4", username);
                    command.Prepare();
                    int affected = command.ExecuteNonQuery();
                    if(affected==0)
                    {
                        return new ResponseBuilder().NotFound();
                    }
                    Database.DisposeDbConnection();
                    return new ResponseBuilder().OK();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }

        private Response PutDeck(Request request)
        {
            if (!(new UserAuthorizer().AuthorizeUserByToken(request)))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                string requestPayload = request.Payload.TrimStart('[').TrimEnd(']');
                string[] cardUUIDs = requestPayload.Split(',');
                if(cardUUIDs.Length != 4)
                {
                    Database.DisposeDbConnection();
                    return new ResponseBuilder().BadRequest();
                }
                for(int i = 0; i < cardUUIDs.Length; i++)
                {
                    cardUUIDs[i] = cardUUIDs[i].Trim(' ').Trim('"');
                }
                Console.WriteLine(cardUUIDs[0]);
                try
                {
                    int cardCount = 0;
                    Utility utils = new Utility();
                    string username = utils.ExtractUsernameFromToken(utils.ExtractTokenFromString(request.Headers["Authorization"]));
                    using var command = new NpgsqlCommand(@"SELECT * FROM ""stack"" WHERE ""username"" = @p5 AND (""cardid""=@p1 OR ""cardid""=@p2 OR ""cardid""=@p3 OR ""cardid""=@p4) AND ""inTrade""=@p6;", Connection);
                    command.Parameters.AddWithValue("p1", Guid.Parse(cardUUIDs[0]));
                    command.Parameters.AddWithValue("p2", Guid.Parse(cardUUIDs[1]));
                    command.Parameters.AddWithValue("p3", Guid.Parse(cardUUIDs[2]));
                    command.Parameters.AddWithValue("p4", Guid.Parse(cardUUIDs[3]));
                    command.Parameters.AddWithValue("p5", username);
                    command.Parameters.AddWithValue("p6", false);
                    command.Prepare();
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cardCount++;
                    }
                    Console.WriteLine(cardCount);
                    if(cardCount != cardUUIDs.Length) { Database.DisposeDbConnection(); return new ResponseBuilder().Forbidden(); }
                    reader.Close();
                    command.Dispose();
                    using var command2 = new NpgsqlBatch(Connection)
                    {
                        BatchCommands =
                        {
                            new NpgsqlBatchCommand(@"UPDATE ""stack"" SET ""inDeck""=@p1 WHERE ""username"" = @p2;")
                            {
                                Parameters =
                                {
                                    new NpgsqlParameter("p1", false),
                                    new NpgsqlParameter("p2", username)
                                }
                            },
                            new NpgsqlBatchCommand(@"UPDATE ""stack"" SET ""inDeck""=@p1 WHERE ""username"" = @p6 AND (""cardid""=@p2 OR ""cardid""=@p3 OR ""cardid""=@p4 OR ""cardid""=@p5);")
                            {
                                Parameters =
                                {
                                    new NpgsqlParameter("p1", true),
                                    new NpgsqlParameter("p2", Guid.Parse(cardUUIDs[0])),
                                    new NpgsqlParameter("p3", Guid.Parse(cardUUIDs[1])),
                                    new NpgsqlParameter("p4", Guid.Parse(cardUUIDs[2])),
                                    new NpgsqlParameter("p5", Guid.Parse(cardUUIDs[3])),
                                    new NpgsqlParameter("p6", username)
                                }
                            }
                        }
                    };
                    int affected = command2.ExecuteNonQuery();

                    if(affected != 8){ Database.DisposeDbConnection(); return new ResponseBuilder().InternalServerError(); }

                    Database.DisposeDbConnection();
                    return new ResponseBuilder().OK();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }
    }
}
