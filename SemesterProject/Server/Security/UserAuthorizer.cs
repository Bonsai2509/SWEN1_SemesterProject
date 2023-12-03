using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Server;
using SemesterProject.Server.Requests;

namespace SemesterProject.Server.Security
{
    internal class UserAuthorizer
    {
        public bool RequestContainsToken(Request request)
        {
           return request.Headers.ContainsKey("Authorization");
        }

        public bool AuthorizeUserByUsernameInTarget(string username, Request request)
        {
            var _utility = new Utility();
            string token = _utility.ExtractTokenFromString(request.Headers["Authorization"]);
            string usernameInToken = _utility.ExtractUsernameFromToken(token);
            return (username == usernameInToken);
            
        }
    }
}
