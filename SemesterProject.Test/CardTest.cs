using SemesterProject.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Test
{
    [TestFixture]
    internal class CardTest
    {
        [Test]
        public void CreateCardThrowsIndexOutOfRangeException()
        {
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                var cardCreator = new CardBuilder();
                cardCreator.generateCard(-1);
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        [TestCase(11)]
        [TestCase(12)]
        [TestCase(13)]
        [TestCase(14)]
        public void CreateCard(int index)
        {
            var cardCreator = new CardBuilder();
            var card = cardCreator.generateCard(index);
            Assert.That(cardCreator.Name[index], Is.EqualTo(card.Name));
            Assert.That(cardCreator.Damage[index], Is.EqualTo(card.Damage));
            Assert.That(cardCreator.Description[index], Is.EqualTo(card.Description));
            Assert.That(cardCreator.CardType[index], Is.EqualTo(card.CardType));
            Assert.That(cardCreator.Element[index], Is.EqualTo(card.Element));
        }
    }
}
