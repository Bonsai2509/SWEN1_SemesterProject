using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject
{
    internal class Player
    {
        public Player(string name, string pw, int money=20, List<Card> DeckData, List<Card> StackData)
        {
            Username = name;
            Password = pw;
            coins = money;
            Deck = DeckData;
            Stack = StackData;
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int coins { get; set; } = 0;
        public List<Card> Deck { get; set; }
        public List<Card> Stack { get; set; }

    }
}
