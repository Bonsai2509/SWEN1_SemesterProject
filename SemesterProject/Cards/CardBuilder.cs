using SemesterProject.Cards.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Cards
{
    internal class CardBuilder
    {
        public ECardType[] CardType { get; }
        public string[] Name { get; }
        public int[] Damage { get; }
        public EDamageType[] Element { get; }

        public CardBuilder() 
        {
            Name = new string[] { "Dragon", "DuplicateCard", "FireElve", "FireSpell", "Goblin", "Knight", "Kraken", "Ork", "StealCard", "WaterSpell", "Wizard" };
            CardType = new ECardType[] { ECardType.monster, ECardType.special, ECardType.monster, ECardType.spell, ECardType.monster, ECardType.monster, ECardType.monster, ECardType.monster, ECardType.special, ECardType.spell, ECardType.monster };
            Damage = new int[] { 40, 15, 20, 20, 10, 25, 45, 30, 15, 20, 25};
            Element = new EDamageType[] {EDamageType.fire, EDamageType.normal, EDamageType.fire, EDamageType.fire, EDamageType.normal, EDamageType.normal, EDamageType.water, EDamageType.normal, EDamageType.normal, EDamageType.water, EDamageType.normal};
        }

        public Card generateCard(int index)
        {
            if(index>(Name.Length-1) || index<0) throw new ArgumentOutOfRangeException("index");
            ECardType cardType = CardType[index];
            string name = Name[index];
            int damage = Damage[index];
            EDamageType element = Element[index];
            switch (name)
            {
                case "Dragon": return new Dragon(cardType, name, damage, element);
                case "DuplicateCard": return new DuplicateCard(cardType, name, damage, element);
                case "FireElve": return new FireElve(cardType, name, damage, element);
                case "FireSpell": return new FireSpell(cardType, name, damage, element);
                case "Goblin": return new Goblin(cardType, name, damage, element);
                case "Knight": return new Knight(cardType, name, damage, element);
                case "Kraken": return new Kraken(cardType, name, damage, element);
                case "Ork": return new Ork(cardType, name, damage, element); 
                case "StealCard": return new StealCard(cardType, name, damage, element);
                case "WaterSpell": return new WaterSpell(cardType, name, damage, element);
                case "Wizard": return new Wizard(cardType, name, damage, element);
                default:break;
            }
            throw new NotImplementedException();
        }
    }
}
