using SemesterProject.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SemesterProject.Test
{
    [TestFixture]
    internal class DamageTest
    {
        [TestCase("Dragon", "FireElve")]
        [TestCase("DuplicateCard", "Kraken")]
        [TestCase("FireElve", "WaterSpell")]
        [TestCase("FireSpell", "Kraken")]
        [TestCase("Goblin", "Dragon")]
        [TestCase("HolyGrenade", "Dragon")]
        [TestCase("Imp", "Kraken")]
        [TestCase("Knight", "WaterSpell")]
        [TestCase("Kraken", "Dragon")]
        [TestCase("Ork", "Wizard")]
        [TestCase("Priest", "Goblin")]
        [TestCase("StealCard", "Kraken")]
        [TestCase("VoidFlame", "Wizard")]
        [TestCase("WaterSpell", "Kraken")]
        [TestCase("Wizard", "Knight")]
        public void CardSpecialitiesTest(string card1name, string card2name)
        {
            var cardBuilder = new CardBuilder();
            Card card1 = null;
            Card card2 = null;
            switch(card1name)
            {
                case "Dragon": 
                    card1 = cardBuilder.generateCard(0);
                    break;
                case "DuplicateCard": 
                    card1 = cardBuilder.generateCard(1);
                    break;
                case "FireElve": 
                    card1 = cardBuilder.generateCard(2);
                    break;
                case "FireSpell":
                    card1 = cardBuilder.generateCard(3);
                    break;
                case "Goblin":
                    card1 = cardBuilder.generateCard(4);
                    break;
                case "HolyGrenade":
                    card1 = cardBuilder.generateCard(13);
                    break;
                case "Imp":
                    card1 = cardBuilder.generateCard(12);
                    break;
                case "Knight":
                    card1 = cardBuilder.generateCard(5);
                    break;
                case "Kraken":
                    card1 = cardBuilder.generateCard(6);
                    break;
                case "Ork":
                    card1 = cardBuilder.generateCard(7);
                    break;
                case "Priest":
                    card1 = cardBuilder.generateCard(11);
                    break;
                case "StealCard":
                    card1 = cardBuilder.generateCard(8);
                    break;
                case "VoidFlame":
                    card1 = cardBuilder.generateCard(14);
                    break;
                case "WaterSpell":
                    card1 = cardBuilder.generateCard(9);
                    break;
                case "Wizard":
                    card1 = cardBuilder.generateCard(10);
                    break;
                default:break;
            }
            switch (card2name)
            {
                case "Dragon":
                    card2 = cardBuilder.generateCard(0);
                    break;
                case "FireElve":
                    card2 = cardBuilder.generateCard(2);
                    break;
                case "Goblin":
                    card2 = cardBuilder.generateCard(4);
                    break;
                case "Knight":
                    card2 = cardBuilder.generateCard(5);
                    break;
                case "Kraken":
                    card2 = cardBuilder.generateCard(6);
                    break;
                case "WaterSpell":
                    card2 = cardBuilder.generateCard(9);
                    break;
                case "Wizard":
                    card2 = cardBuilder.generateCard(10);
                    break;
                default:break;
            }
            int damage = card1.CalcDmgAmount(card2);
            Assert.That(damage, Is.EqualTo(0));
            Assert.Pass("Speciality passed");
        }
    }
}
