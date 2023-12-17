using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    public class UserScores
    {
        public string Username { get; }
        public double Elo { get; }

        public UserScores(string username, double elo)
        {
            Username = username;
            Elo = elo;
        }
    }
}
