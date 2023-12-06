using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class UserScores
    {
        public string Username { get; }
        public int Elo { get; }

        public UserScores(string username, int elo)
        {
            Username = username;
            Elo = elo;
        }
    }
}
