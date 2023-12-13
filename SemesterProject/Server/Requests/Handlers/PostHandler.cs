using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    internal class PostHandler : Handler
    {

        public Response HandlePost(Request request)
        {
            switch (request.Target[0])
            {
                case "users": return PostUser(request);
                case "sessions": return PostSession(request);
                case "packages": return PostPackages(request);
                //case "transactions": return PostTransaction(request);
                //case "battles": return PostBattle(request);
                //case "tradings": return PostTrade(request);
            }
            return new ResponseBuilder().BadRequest();
        }
        private Response PostUser(Request request)
        {
            UserCredentials userCred = JsonConvert.DeserializeObject<UserCredentials>(request.Payload);
            try
            {
                //check if user already exists
                using var checkIfUserExists = new NpgsqlCommand(@"SELECT * FROM ""user"" WHERE ""username""=@p1;", Connection);
                checkIfUserExists.Parameters.AddWithValue("p1", userCred.Username);
                using var reader = checkIfUserExists.ExecuteReader();
                if (reader.Read())
                {
                    return new ResponseBuilder().Conflict();
                }
                checkIfUserExists.Dispose();
                reader.Close();

                //create user in db
                using var command = new NpgsqlCommand(@"INSERT INTO ""user"" (""elo"", ""wins"", ""loses"", ""draws"", ""coins"", ""username"", ""bio"", ""image"", ""token"", ""password"", ""isAdmin"") VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11);", Connection);
                command.Parameters.AddWithValue("p1", 1000);
                command.Parameters.AddWithValue("p2", 0);
                command.Parameters.AddWithValue("p3", 0);
                command.Parameters.AddWithValue("p4", 0);
                command.Parameters.AddWithValue("p5", 20);
                command.Parameters.AddWithValue("p6", userCred.Username);
                command.Parameters.AddWithValue("p7", "no bio yet");
                command.Parameters.AddWithValue("p8", "no image yet");
                command.Parameters.AddWithValue("p9", $"{userCred.Username}-mtcg");
                command.Parameters.AddWithValue("p10", userCred.Password);
                command.Parameters.AddWithValue("p11", (userCred.Username=="admin" ? true : false));
                command.Prepare();
                int affected = command.ExecuteNonQuery();
                if (affected == 0)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                Database.DisposeDbConnection();
                return new ResponseBuilder().Created();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new ResponseBuilder().InternalServerError();
            }
            finally { Database.DisposeDbConnection(); }
        }

        private Response PostSession(Request request)
        {
            UserCredentials userCred = JsonConvert.DeserializeObject<UserCredentials>(request.Payload);
            try
            {
                using var checkIfUserExists = new NpgsqlCommand(@"SELECT ""token"" FROM ""user"" WHERE ""username""=@p1 AND ""password""=@p2;", Connection);
                checkIfUserExists.Parameters.AddWithValue("p1", userCred.Username);
                checkIfUserExists.Parameters.AddWithValue("p2", userCred.Password);
                using var reader = checkIfUserExists.ExecuteReader();
                if (!reader.Read())
                {
                    return new ResponseBuilder().Unauthorized();
                }
                string dbResult = reader.GetString(0);
                string token = new Utility().ExtractTokenFromString(dbResult);
                Database.DisposeDbConnection();
                return new ResponseBuilder().PlainTextResponse(token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new ResponseBuilder().InternalServerError();
            }
            finally { Database.DisposeDbConnection(); }
        }

        private Response PostPackages(Request request)
        {
            if (!(new UserAuthorizer().AuthorizeUserByToken(request)))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                Utility utils = new Utility();
                string token = utils.ExtractTokenFromString(request.Headers["Authorization"]);
                try
                {
                    //check if user is admin
                    using var checkIfUserIsAdmin = new NpgsqlCommand(@"SELECT * FROM ""user"" WHERE ""token""=@p1 AND ""isAdmin""=@p2;", Connection);
                    checkIfUserIsAdmin.Parameters.AddWithValue("p1", token);
                    checkIfUserIsAdmin.Parameters.AddWithValue("p2", true);
                    checkIfUserIsAdmin.Prepare();
                    using var reader = checkIfUserIsAdmin.ExecuteReader();
                    if(!reader.Read())
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().Forbidden();
                    }
                    checkIfUserIsAdmin.Dispose();
                    reader.Close();

                    PackageCard[] cards = JsonConvert.DeserializeObject<PackageCard[]>(request.Payload);
                    if(cards.Length != 5)
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().BadRequest();
                    }

                    //check if cards already exist
                    using var checkIfAnyCardExists = new NpgsqlCommand(@"SELECT * FROM ""stack"" WHERE ""cardid""=@p1 OR ""cardid""=@p2 OR ""cardid""=@p3 OR ""cardid""=@p4 OR ""cardid""=@p5;", Connection);
                    checkIfAnyCardExists.Parameters.AddWithValue("p1", Guid.Parse(cards[0].Id));
                    checkIfAnyCardExists.Parameters.AddWithValue("p2", Guid.Parse(cards[1].Id));
                    checkIfAnyCardExists.Parameters.AddWithValue("p3", Guid.Parse(cards[2].Id));
                    checkIfAnyCardExists.Parameters.AddWithValue("p4", Guid.Parse(cards[3].Id));
                    checkIfAnyCardExists.Parameters.AddWithValue("p5", Guid.Parse(cards[4].Id));
                    using var reader2 = checkIfAnyCardExists.ExecuteReader();
                    if(reader.Read())
                    {
                        return new ResponseBuilder().Conflict();
                    }
                    checkIfAnyCardExists.Dispose();
                    reader.Close();

                    //create package
                    using var createPackage = new NpgsqlCommand(@"INSERT INTO ""package"" (""packageid"", ""card1id"", ""card2id"", ""card3id"", ""card4id"", ""card5id"") VALUES (@p1, @p2, @p3, @p4, @p5, @p6);", Connection);
                    createPackage.Parameters.AddWithValue("p1", Guid.NewGuid());
                    createPackage.Parameters.AddWithValue("p2", Guid.Parse(cards[0].Id));
                    createPackage.Parameters.AddWithValue("p3", Guid.Parse(cards[1].Id));
                    createPackage.Parameters.AddWithValue("p4", Guid.Parse(cards[2].Id));
                    createPackage.Parameters.AddWithValue("p5", Guid.Parse(cards[3].Id));
                    createPackage.Parameters.AddWithValue("p6", Guid.Parse(cards[4].Id));
                    int affected = createPackage.ExecuteNonQuery();
                    if (affected == 0)
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().InternalServerError();
                    }
                    createPackage.Dispose();

                    //insert cards into stack
                    foreach (var card in cards)
                    {
                        using var insertCardIntoStack = new NpgsqlCommand(@"INSERT INTO ""stack"" (""cardindex"", ""inDeck"", ""inTrade"", ""cardid"", ""username"") VALUES (@p1, @p2, @p3, @p4, @p5)", Connection);
                        insertCardIntoStack.Parameters.AddWithValue("p1", card.Index);
                        insertCardIntoStack.Parameters.AddWithValue("p2", false);
                        insertCardIntoStack.Parameters.AddWithValue("p3", false);
                        insertCardIntoStack.Parameters.AddWithValue("p4", Guid.Parse(card.Id));
                        insertCardIntoStack.Parameters.AddWithValue("p5", "admin");
                        int affectedRows = insertCardIntoStack.ExecuteNonQuery();
                        if (affectedRows == 0)
                        {
                            Database.DisposeDbConnection();
                            return new ResponseBuilder().InternalServerError();
                        }
                        insertCardIntoStack.Dispose();
                    }

                    Database.DisposeDbConnection();
                    return new ResponseBuilder().Created();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }

        /*private Response PostTransaction(Request request) 
        {
            
        }

        /*private Response PostBattle(Request request)
        {

        }

        /*private Response PostTrade(Request request) 
        {
            if(request.Target.count>1)
            {
                CarryOutTrade
            }
            else
            {
                
            }
        }

        /*private Response CreateTrade(Request request) 
        {
            
        }

        /*private Response CarryOutTrade(Request request)
        {

        }*/

    }
}
