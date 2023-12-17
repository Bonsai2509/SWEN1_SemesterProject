using SemesterProject.Cards.CardTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Cards
{
    public class CardBuilder
    {
        public ECardType[] CardType { get; }
        public string[] Name { get; }
        public string[] Description { get; }
        public int[] Damage { get; }
        public EDamageType[] Element { get; }

        public CardBuilder() 
        {
            Name = new string[] { "Dragon", "DuplicateCard", "FireElve", "FireSpell", "Goblin", "Knight", "Kraken", "Ork", "StealCard", "WaterSpell", "Wizard", "Priest", "Imp", "HolyGrenade", "VoidFlame"};
            CardType = new ECardType[] { ECardType.monster, ECardType.special, ECardType.monster, ECardType.spell, ECardType.monster, ECardType.monster, ECardType.monster, ECardType.monster, ECardType.special, ECardType.spell, ECardType.monster, ECardType.monster, ECardType.monster, ECardType.spell, ECardType.spell};
            Damage = new int[] { 40, 15, 20, 20, 10, 25, 45, 30, 15, 20, 25, 20, 15, 50, 30};
            Element = new EDamageType[] {EDamageType.fire, EDamageType.normal, EDamageType.fire, EDamageType.fire, EDamageType.normal, EDamageType.normal, EDamageType.water, EDamageType.normal, EDamageType.normal, EDamageType.water, EDamageType.normal, EDamageType.holy, EDamageType.demonic, EDamageType.holy, EDamageType.demonic};
            Description = new string[]
            {
                "Although the Dragon is a mighty creature Fire Elves are immune to it's attacks due to their agility and familiarity with dragons.",
                "A powerful card that analyzes your deck to replicate the most effective card, ensuring dominance by duplicating the card most likely to triumph over numerous enemy cards.",
                "Skilled in fiery environments but has a distinct weakness against water spells, rendering its attacks ineffective in aquatic confrontations.",
                "A potent fire magic attack, but utterly ineffective against the mighty kraken, whose aquatic nature nullifies this spell.",
                "Overwhelmed by fear when facing dragons, this creature is unable to muster the courage to attack these fearsome beasts.",
                "Clad in heavy armor, the knight is susceptible to drowning when struck by water spells, a fatal weakness in their defense.",
                "A formidable sea monster that rules the ocean depths but meets its match against the airborne, fire-breathing dragon.",
                "A strong and fearsome warrior on the ground, but easily outmatched by the wizard's ranged magical attacks.",
                "This card has a unique ability to steal the opponents most potent card. However it cannot deal any damage to the kraken.",
                "Harnessing the power of water, this spell is effective in many scenarios but proves futile against the kraken, whose aquatic nature provides immunity.",
                "A master of magic, but his spells are ineffectual against the knight's armor, leaving him unable to inflict any damage in this matchup.",
                "The presence of goblins disgusts the priest to the extent of incapacitation, rendering him unable to perform any attack.",
                "Powerless against the water element kraken, this small demon finds its abilities nullified in the presence of this formidable foe.",
                "A devastating explosive with a notable exception – it cannot reach the soaring heights of the flying dragon, leaving it unscathed.",
                "A mysterious and powerful flame that, however, falls under the wizard's control, rendering it harmless against him."
            };
        }

        public Card generateCard(int index)
        {
            if(index>(Name.Length-1) || index<0) throw new IndexOutOfRangeException("index");
            ECardType cardType = CardType[index];
            string name = Name[index];
            string description = Description[index];
            int damage = Damage[index];
            EDamageType element = Element[index];
            switch (name)
            {
                case "Dragon": return new Dragon(cardType, name, description, damage, element);
                case "DuplicateCard": return new DuplicateCard(cardType, name, description, damage, element);
                case "FireElve": return new FireElve(cardType, name, description, damage, element);
                case "FireSpell": return new FireSpell(cardType, name, description, damage, element);
                case "Goblin": return new Goblin(cardType, name, description, damage, element);
                case "Knight": return new Knight(cardType, name, description, damage, element);
                case "Kraken": return new Kraken(cardType, name, description, damage, element);
                case "Ork": return new Ork(cardType, name, description, damage, element); 
                case "StealCard": return new StealCard(cardType, name, description, damage, element);
                case "WaterSpell": return new WaterSpell(cardType, name, description, damage, element);
                case "Wizard": return new Wizard(cardType, name, description, damage, element);
                case "Priest": return new Priest(cardType, name, description, damage, element);
                case "Imp": return new Imp(cardType, name, description, damage, element);
                case "HolyGrenade": return new HolyGrenade(cardType, name, description, damage, element);
                case "VoidFlame": return new VoidFlame(cardType, name, description, damage, element);
                default:break;
            }
            throw new NotImplementedException();
        }
    }
}
