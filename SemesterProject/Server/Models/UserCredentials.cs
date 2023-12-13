using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class UserCredentials
    {
        public string Username { get; }
        public string Password { get; }

        public UserCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
