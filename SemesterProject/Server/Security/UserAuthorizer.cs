using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Server;
using SemesterProject.Server.Requests;
using SemesterProject.Database;
using SemesterProject.Server.Requests.Handlers;
using Npgsql;
using SemesterProject.Server.Responses;

namespace SemesterProject.Server.Security
{
    internal class UserAuthorizer : Handler
    {
        public bool RequestContainsToken(Request request)
        {
           return request.Headers.ContainsKey("Authorization");
        }

        public bool AuthorizeUserByUsernameInTarget(string username, Request request)
        {
            if (!RequestContainsToken(request)) return false;
            var _utility = new Utility();
            string token = _utility.ExtractTokenFromString(request.Headers["Authorization"]);
            string usernameInToken = _utility.ExtractUsernameFromToken(token);

            //check if user with name and token exist
            using var command = new NpgsqlCommand(@"SELECT * FROM ""user"" WHERE ""username""=@p1 AND ""token""=@p2;", Connection);
            command.Parameters.AddWithValue("p1", username);
            command.Parameters.AddWithValue("p2", token);
            command.Prepare();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                Database.DisposeDbConnection();
                return true;
            }
            else
            {
                Database.DisposeDbConnection();
                return false;
            }
        }
        public bool AuthorizeUserByToken(Request request)
        {
            if(!RequestContainsToken(request)) return false;
            var _utility = new Utility();
            string token = _utility.ExtractTokenFromString(request.Headers["Authorization"]);
            string usernameInToken = _utility.ExtractUsernameFromToken(token);


            //check if user with token exist
            using var command = new NpgsqlCommand(@"SELECT * FROM ""user"" WHERE ""username""=@p1;", Connection);
            command.Parameters.AddWithValue("p1", usernameInToken);
            command.Prepare();
            using var reader = command.ExecuteReader();
            if(reader.Read())
            {
                Database.DisposeDbConnection();
                return true;
            }
            else
            {
                Database.DisposeDbConnection();
                return false;
            }
        }
    }
}
