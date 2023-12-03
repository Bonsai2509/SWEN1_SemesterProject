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

namespace SemesterProject.Server.Requests.Handlers
{
    internal class GetHandler
    {
        private NpgsqlConnection CreateDbConnection()
        {
            return new PostgreSql().connection;
        }
        public Response HandleGet(Request request)
        {
            switch (request.Target[0])
            {
                case "users": return GetUserByUsername(request);
                    /*case "cards": return GetCardsByToken(request);
                    case "deck": return GetDeckByToken(request);
                    case "stats": return GetStatsByToken(request);
                    case "scoreboard": return GetScoreboard(request);
                    case "tradings": return GetTradingDeals(request);*/
            }
            return new ResponseBuilder().BadRequest();
        }
        //taken from stackoverflow
        private Response GetUserByUsername(Request request)
        {
            string username = request.Target[1];
            if (!(new UserAuthorizer().AuthorizeUserByUsernameInTarget(username, request)))
            {
                return new ResponseBuilder().Unauthorized();
            }
            else
            {

                using var connection = CreateDbConnection();
                try
                {
                    using var command = new NpgsqlCommand("SELECT \"username\", \"bio\", \"image\" FROM \"user\" WHERE \"username\"=@p1;", connection);
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
                    Console.WriteLine(e.Message);
                    return new ResponseBuilder().BadRequest();
                }
            }
        }

        private Response GetCardsByToken(Request request)
        {

        }

        private Response GetDeckByToken(Request request)
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

        }
    }
}
