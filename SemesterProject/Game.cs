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
            Random Rnd = new Random();
            int RoundCounter = 0;
            List<Card> Player1Deck = Players[0].Deck;
            List<Card> Player2Deck = Players[1].Deck;

            int RandomIndex1 = 0;
            int RandomIndex2 = 0;

            Card? Player1ChosenCard;
            Card? Player2ChosenCard;

            while (Player1Deck != null && Player2Deck != null && RoundCounter<100)
            {
                RandomIndex1 = Rnd.Next(Player1Deck.Count);
                RandomIndex2 = Rnd.Next(Player2Deck.Count);

                Player1ChosenCard = Player1Deck[RandomIndex1];
                Player2ChosenCard = Player2Deck[RandomIndex2];

                if (Player1ChosenCard.CalcDmgAmount(Player2ChosenCard.CardType, Player2ChosenCard.Element) < Player2ChosenCard.CalcDmgAmount(Player1ChosenCard.CardType, Player1ChosenCard.Element))
                {
                    Player2Deck.Add(Player1ChosenCard);
                    Player1Deck.RemoveAt(RandomIndex1);
                }
                else if(Player2ChosenCard.CalcDmgAmount(Player1ChosenCard.CardType, Player1ChosenCard.Element) < Player1ChosenCard.CalcDmgAmount(Player2ChosenCard.CardType, Player2ChosenCard.Element))
                {
                    Player1Deck.Add(Player2ChosenCard);
                    Player2Deck.RemoveAt(RandomIndex2);
                }
                RoundCounter++;
            }
            //continue implementing logic after battle (Elo gain or draw)
        }
    }
}
