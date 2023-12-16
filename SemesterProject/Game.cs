using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SemesterProject.Cards;
using SemesterProject.Cards.CardTypes;

namespace SemesterProject
{
    enum EWinner
    {
        draw = 0, player = 1, opponent = 2
    }
    internal class Game
    {
        public Game(Player player, Player opponent)
        {
            Player = player;
            Opponent = opponent;
        }

        public Player Player { get; }
        public Player Opponent { get; }

        public string? battleLog;
        public EWinner finalWinner;

        public void Battle()
        {
            Random rnd = new Random();
            int roundCounter = 0;
            EWinner winner;
            
            List<Card> playerDeck = Player.Deck;
            List<Card> opponentDeck = Opponent.Deck;

            battleLog = $"Battle between {Player.Username} and {Opponent.Username}:\n";

            int randomIndex1 = 0;
            int randomIndex2 = 0;

            Card? playerChosenCard;
            Card? opponentChosenCard;

            while (playerDeck.Count != 0 && opponentDeck.Count != 0 && roundCounter<100)
            {
                battleLog = $"{battleLog} Round {roundCounter + 1}:\n";
                randomIndex1 = rnd.Next(playerDeck.Count);
                randomIndex2 = rnd.Next(opponentDeck.Count);

                playerChosenCard = playerDeck[randomIndex1];
                opponentChosenCard = opponentDeck[randomIndex2];

                bool removePlayerCard = false;
                bool removeOpponentCard = false;

                if(playerChosenCard.CardType == ECardType.special)
                {
                    removePlayerCard = true;
                    switch(playerChosenCard.Name)
                    {
                        case "DuplicateCard":
                            battleLog = $"{battleLog}{Player.Username} has used a Duplication Card! ";
                            DuplicateCard(playerDeck, opponentDeck);
                            break;
                        case "StealCard":
                            battleLog = $"{battleLog}{Player.Username} has used a Steal Card! ";
                            StealCard(playerDeck, opponentDeck, randomIndex2);
                            break;
                        default:break;
                    }
                }
                
                if(opponentChosenCard.CardType == ECardType.special)
                {
                    removeOpponentCard= true;
                    switch (opponentChosenCard.Name)
                    {
                        case "DuplicateCard":
                            battleLog = $"{battleLog}{Opponent.Username} has used a Duplication Card! ";
                            DuplicateCard(opponentDeck, playerDeck);
                            break;
                        case "StealCard":
                            battleLog = $"{battleLog}{Opponent.Username} has used a Steal Card! ";
                            StealCard(opponentDeck, playerDeck, randomIndex1);
                            break;
                        default: break;
                    }
                }

                winner = BattleRound(playerChosenCard, opponentChosenCard);

                switch(winner)
                {
                    case EWinner.player: playerDeck.Add(opponentChosenCard);
                                          opponentDeck.Remove(opponentChosenCard);
                    break;
                    case EWinner.opponent: opponentDeck.Add(playerChosenCard);
                                          playerDeck.Remove(playerChosenCard);
                    break;
                    default: break;
                }
                if(playerDeck.Count() < opponentDeck.Count())
                {
                    finalWinner = EWinner.opponent;
                }
                else if(opponentDeck.Count() < playerDeck.Count())
                {
                    finalWinner = EWinner.player;
                }
                else
                {
                    finalWinner = EWinner.draw;
                }
                
                if(removePlayerCard) { playerDeck.Remove(playerChosenCard); }
                if (removeOpponentCard) { opponentDeck.Remove(opponentChosenCard); }

                roundCounter++;
            }
            
            switch(finalWinner)
            {
                case EWinner.player:
                    Player.Wins++;
                    battleLog = $"{battleLog}{Player.Username} has won the battle!\n";
                    break;
                case EWinner.opponent:
                    Player.Loses++;
                    battleLog = $"{battleLog}{Opponent.Username} has won the battle!\n";
                    break;
                case EWinner.draw:
                    Player.Draws++;
                    battleLog = $"{battleLog}The battle was a draw!\n";
                    break;
                default:break;
            }
            CalculateNewPlayerElo();
        }

        private EWinner BattleRound(Card PlayerCard, Card OpponentCard)
        {
            int playerDamage = PlayerCard.CalcDmgAmount(OpponentCard);
            int opponentDamage = OpponentCard.CalcDmgAmount(PlayerCard);
            if (playerDamage < opponentDamage)
            {
                battleLog = $"{battleLog}{Player.Username} uses {PlayerCard.Name} with {playerDamage} damage VS {Opponent.Username} uses {OpponentCard.Name} with {opponentDamage} damage: {Opponent.Username} wins!\n";
                return EWinner.opponent;
            }
            else if (opponentDamage < playerDamage)
            {
                battleLog = $"{battleLog}{Player.Username} uses {PlayerCard.Name} with {playerDamage} damage VS {Opponent.Username} uses {OpponentCard.Name} with {opponentDamage} damage: {Player.Username} wins!\n";
                return EWinner.player;
            }
            battleLog = $"{battleLog}{Player.Username} uses {PlayerCard.Name} with {playerDamage} damage VS {Opponent.Username} uses {OpponentCard.Name} with {opponentDamage}damage: It is a draw!\n";
            return EWinner.draw;
        }

        private EWinner SimulatedRound(Card PlayerCard, Card OpponentCard)
        {
            int playerDamage = PlayerCard.CalcDmgAmount(OpponentCard);
            int opponentDamage = OpponentCard.CalcDmgAmount(PlayerCard);
            if (playerDamage < opponentDamage)
            {
                return EWinner.opponent;
            }
            else if (opponentDamage < playerDamage)
            {
                return EWinner.player;
            }
            return EWinner.draw;
        }

        private void DuplicateCard(List<Card> specialCardDeck, List<Card> enemyDeck) 
        {
            int beatenCards = 0;
            Card duplicate = null;
            foreach (Card myCard in specialCardDeck)
            {
                int wins = 0;
                foreach(Card enemyCard in enemyDeck)
                {
                    if(SimulatedRound(myCard, enemyCard)==EWinner.player){ wins++; }
                }
                if(wins>beatenCards)
                {
                    beatenCards=wins;
                    duplicate = myCard;
                }
            }
            if(duplicate!=null)
            {
                battleLog = $"{battleLog}The duplicated Card is {duplicate.Name}!\n";
                specialCardDeck.Add(duplicate);
            }
            else
            {
                battleLog = $"{battleLog}There was no Card worth duplicating so nothing happens!\n";
            }
        }

        private void StealCard(List<Card> specialCardDeck, List<Card> deckOpponent, int notToSteal)
        {
            int beatenCards = 0;
            Card steal = null;
            foreach (Card enemyCard in deckOpponent)
            {
                int wins = 0;
                foreach (Card myCard in specialCardDeck)
                {
                    if (SimulatedRound(myCard, enemyCard) == EWinner.opponent) { wins++; }
                }
                if (wins > beatenCards)
                {
                    beatenCards = wins;
                    steal = enemyCard;
                }
            }
            if (steal != null)
            {
                battleLog = $"{battleLog}The stolen Card is {steal.Name}!\n";
                deckOpponent.Remove(steal);
                specialCardDeck.Add(steal);
            }
            else
            {
                battleLog = $"{battleLog}There was no Card worth stealing so nothing happens!\n";
            }
        }

        //sophisticated Elo system
        private void CalculateNewPlayerElo()
        {
            double playerWinProb = 1.0 * 1.0 / (1 + 1.0 * Math.Pow(10,1.0 * (Opponent.Elo - Player.Elo) / 400));
            double eloChange = 0;
            switch (finalWinner)
            {
                case EWinner.player:
                    eloChange = 30 * (1 - playerWinProb);
                    battleLog = $"{battleLog}{Player.Username} has gained {eloChange} elo!\n";
                    Player.Elo += eloChange; 
                    break;
                case EWinner.opponent:
                    eloChange = 30 * (0 - playerWinProb);
                    battleLog = $"{battleLog}{Player.Username} has lost {Math.Abs(eloChange)} elo!\n";
                    Player.Elo += eloChange;
                    break;
                case EWinner.draw:
                    battleLog = $"{battleLog}Since the battle was a draw the elo does not change!\n";
                    break;
                default:break;
            }
        }
    }
}
