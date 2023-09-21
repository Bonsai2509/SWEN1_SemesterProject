using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject
{
    internal class Game
    {
        public Game(List<Player> PlayerList)
        {
            Players = PlayerList;
        }

        public List<Player> Players { get; set; }

        public void Battle()
        {
            List<Card>? Player1Deck = Players[0].Deck;
            List<Card>? Player2Deck = Players[1].Deck;
        }
    }
}
