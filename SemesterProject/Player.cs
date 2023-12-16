using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Cards;

namespace SemesterProject
{
    internal class Player
    {
        public Player(double elo, int wins, int loses, int draws, string username, List<Card> deck)
        {
            Elo = elo;
            Wins = wins;
            Loses = loses;
            Draws = draws;
            Username = username;
            Deck = deck;
        }
        public string Username { get; }
        public double Elo { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Draws { get; set; }
        public List<Card> Deck { get; }
    }
}
