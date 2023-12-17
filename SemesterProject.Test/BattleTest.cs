using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemesterProject.Cards;
using SemesterProject;

namespace SemesterProject.Test
{
    [TestFixture]
    internal class BattleTest
    {
        [Test]
        public void OneBattleRoundPlayerWIns()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(0));
            
            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(4));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);
            game.Battle();
            Assert.That(game.finalWinner, Is.EqualTo(EWinner.player));
            Assert.Pass("Player wins");
        }

        [Test]
        public void OneBattleRoundOpponentWIns()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(4));

            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(0));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);
            game.Battle();
            Assert.That(game.finalWinner, Is.EqualTo(EWinner.opponent));
            Assert.Pass("Opponent wins");
        }

        [Test]
        public void OneBattleRoundDraw()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(4));

            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(4));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);

            Assert.That(game.BattleRound(playerDeck[0], opponentDeck[0]), Is.EqualTo(EWinner.draw));
            Assert.Pass("BattleRound was a draw");
        }

        [Test]
        public void Battle100RoundsDrawTest()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(4));

            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(4));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);
            game.Battle();
            Assert.That(game.finalWinner, Is.EqualTo(EWinner.draw));
            Assert.Pass("Battle was a draw");
        }

        [Test(Description =
            "Should not be able to steal the card, the enemy is using this turn. Since they only have 1 card it should not be able to steal. Goblin should result in a draw so the decks should not change")]
        public void StealCardFailTest()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(8));

            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(4));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);
            game.StealCard(playerDeck, opponentDeck, 0);
            Assert.That(playerDeck.Count(), Is.EqualTo(1));
            Assert.That(opponentDeck.Count(), Is.EqualTo(1));
            Assert.Pass("StealCard does not steal the opponents drawn card for the battleround");
        }

        [Test(Description =
            "Should be able to steal a card. Player should end up with 2 cards in deck and opponent should have 0")]
        public void StealCardSucessTest()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(8));

            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(0));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);
            game.StealCard(playerDeck, opponentDeck, 1);
            Assert.That(playerDeck.Count(), Is.EqualTo(2));
            Assert.That(opponentDeck.Count(), Is.EqualTo(0));
            Assert.Pass("StealCard sucess");
        }
        
        [Test(Description =
            "Should not duplicate any card since it only duplicates a card that can win against the most enemy cards. Since the round is a draw it should not duplicate it at all")]
        public void DuplicateCardFailTest()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(1));

            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(1));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);
            game.DuplicateCard(playerDeck, opponentDeck);
            Assert.That(playerDeck.Count(), Is.EqualTo(1));
            Assert.Pass("Duplicate Card failed when expected");
        }

        [Test(Description =
            "Should Duplicate the Dragon in the Deck since it wins against the goblin in the opponents deck")]
        public void DuplicateCardSucessTest()
        {
            List<Card> playerDeck = new List<Card>();
            playerDeck.Add(new CardBuilder().generateCard(1));
            playerDeck.Add(new CardBuilder().generateCard(0));

            List<Card> opponentDeck = new List<Card>();
            opponentDeck.Add(new CardBuilder().generateCard(4));

            Player player = new Player(1000, 0, 0, 0, "player", playerDeck);
            Player opponent = new Player(1000, 0, 0, 0, "opponent", opponentDeck);

            Game game = new Game(player, opponent);
            game.DuplicateCard(playerDeck, opponentDeck);
            Assert.That(playerDeck.Count(), Is.EqualTo(3));
            Assert.Pass("Duplicate Card failed when expected");
        }
    }
}
