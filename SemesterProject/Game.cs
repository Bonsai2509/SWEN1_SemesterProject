using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Cards;
using SemesterProject.Cards.CardTypes;

namespace SemesterProject
{
    enum EWinner
    {
        draw = 0, player1 = 1, player2 = 2
    }
    internal class Game
    {
        public Game(Player Player)
        {
            Player1 = Player;
        }

        public Player Player1 { get; set; }
        public Player? Player2 { get; set; }

        public string? battleLog;

        private void getOpponent()
        {
            //get opponent from database
        }

        public void Battle()
        {
            Random rnd = new Random();
            int roundCounter = 0;
            EWinner winner;
            EWinner finalWinner;
            List<Card> Player1Deck = Player1.Deck;
            List<Card> Player2Deck = Player2.Deck;

            battleLog = $"Battle between {Player1.Username} and {Player2.Username}:\n"

            int RandomIndex1 = 0;
            int RandomIndex2 = 0;

            Card? Player1ChosenCard;
            Card? Player2ChosenCard;

            while (Player1Deck != null && Player2Deck != null && roundCounter<100)
            {
                battleLog = $"{battleLog} Round {roundCounter + 1}:\n";
                RandomIndex1 = rnd.Next(Player1Deck.Count);
                RandomIndex2 = rnd.Next(Player2Deck.Count);

                Player1ChosenCard = Player1Deck[RandomIndex1];
                Player2ChosenCard = Player2Deck[RandomIndex2];

                if(Player1ChosenCard.CardType == ECardType.special)
                {
                    switch(Player1ChosenCard.Name)
                    {
                        case "DuplicateCard": DuplicateCard(Player1Deck);
                            break;
                        case "StealCard": StealCard(Player1Deck, Player2Deck, RandomIndex2);
                            break;
                        default:break;
                    }
                }
                
                if(Player2ChosenCard.CardType == ECardType.special)
                {
                    switch (Player2ChosenCard.Name)
                    {
                        case "DuplicateCard":
                            DuplicateCard(Player2Deck);
                            break;
                        case "StealCard":
                            StealCard(Player2Deck, Player1Deck, RandomIndex1);
                            break;
                        default: break;
                    }
                }

                winner = BattleRound(Player1ChosenCard, Player2ChosenCard);

                switch(winner)
                {
                    case EWinner.player1: Player1Deck.Add(Player2ChosenCard);
                                          Player2Deck.Remove(Player2ChosenCard);
                    break;
                    case EWinner.player2: Player2Deck.Add(Player1ChosenCard);
                                          Player1Deck.Remove(Player1ChosenCard);
                    break;
                    default: break;
                }
                if(Player1Deck.Count() < Player2Deck.Count())
                {
                    finalWinner = EWinner.player2;
                }
                else if(Player2Deck.Count() < Player1Deck.Count())
                {
                    finalWinner = EWinner.player1;
                }
                else
                {
                    finalWinner = EWinner.draw;
                }
                
                roundCounter++;
            }
            //continue implementing logic after battle (Elo gain or draw)
        }

        private EWinner BattleRound(Card Player1Card, Card Player2Card)
        {
            int player1Damage = Player1Card.CalcDmgAmount(Player2Card);
            int player2Damage = Player2Card.CalcDmgAmount(Player1Card);
            if (player1Damage < player2Damage)
            {
                battleLog = $"{battleLog}{Player1.Username} {Player1Card.Name} ({player1Damage}) VS {Player2.Username} {Player2Card.Name} ({player2Damage}): {Player2.Username} wins!\n";
                return EWinner.player2;
            }
            else if (player2Damage < player1Damage)
            {
                battleLog = $"{battleLog}{Player1.Username} {Player1Card.Name} ({player1Damage}) VS {Player2.Username} {Player2Card.Name} ({player2Damage}): {Player1.Username} wins!\n";
                return EWinner.player1;
            }
            battleLog = $"{battleLog}{Player1.Username} {Player1Card.Name} ({player1Damage}) VS {Player2.Username} {Player2Card.Name} ({player2Damage}): It is a draw!\n";
            return EWinner.draw;
        }

        private void DuplicateCard(List<Card> Deck) 
        {
            int damage = 0;
            Card duplicate;
            foreach(Card card in Deck)
            {
                if (card.Damage > damage)
                {
                    damage = card.Damage;
                    duplicate = card;
                }
            }
        }

        private void StealCard(List<Card> DeckMe, List<Card> DeckOpponent, int notToSteal)
        {
            int damage = 0;
            int index = 0;
            Card? toSteal = null;
            foreach (Card card in DeckOpponent)
            {
                if (card.Damage > damage && index != notToSteal)
                {
                    damage = card.Damage;
                    toSteal = card;
                }
                index++;
            }
            if(toSteal != null)
            {
                DeckMe.Add(toSteal);
            }
        }
    }
}
