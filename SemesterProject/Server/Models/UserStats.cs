using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class UserStats
    {
        public string UserName { get; }
        public int Elo { get; }
        public int Wins { get; }
        public int Loses { get; }
        public double WinLoseRatio { get; }

        public UserStats(string userName, int elo, int wins, int loses, double winRate)
        {
            UserName = userName;
            Elo = elo;
            Wins = wins;
            Loses = loses;
            WinLoseRatio = winRate;
        }
    }
}
