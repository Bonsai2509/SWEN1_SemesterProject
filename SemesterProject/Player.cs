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
        public Player(string Username, string Password, List<Card> Deck, List<Card> Stack, int Coins = 20, int Elo=100, int GameCount=0, int Wins=0)
        {
            this.Username = Username;
            this.Password = Password;
            this.coins = Coins;
            this.Elo = Elo;
            this.GameCount = GameCount;
            this.Wins = Wins;
            this.Deck = Deck;
            this.Stack = Stack;
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int coins { get; set; } = 0;
        public int Elo { get; }
        public int GameCount { get; }
        public int Wins { get; }
        public List<Card> Deck { get; set; }
        public List<Card> Stack { get; set; }


        public void ChangeElo(bool win)
        {
            //add sophisticated elo caluclator for loss/win
        }
        public void BuyPack()
        {
            if(coins>=5)
            {
                for(int i=0; i < 5; i++) 
                {
                    //add random Card
                }
            }
        }
        public void ManageCards(int StackIndex, int DeckIndex)
        {
            if(StackIndex<Stack.Count)
            {
                if(Deck.Count <= 4)
                {
                    Deck.Add(Stack[StackIndex]);
                }
                else if(DeckIndex < 5)
                {
                    Deck[DeckIndex] = Stack[StackIndex];
                }
            }
        }

    }
}
