using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum ECardType
{
    monster = 0, spell = 1, special = 2
}

enum EDamageType
{
    normal = 0, fire = 1, water = 2
}


namespace SemesterProject.Cards
{
    abstract internal class Card
    {
        public Card(ECardType type, string name, int damage, EDamageType elem)
        {
            CardType = type;
            Name = name;
            Damage = damage;
            Element = elem;
        }
        public ECardType CardType { get; }



        public string Name { get; private set; }
        public int Damage { get; }
        public EDamageType Element { get; set; }


        public virtual int CalcDmgAmount(Card EnemyCard)
        {
            int calcDmg = 0;
            EDamageType enemyElement = EnemyCard.Element;

            if (CardType == ECardType.spell || EnemyCard.CardType == ECardType.spell)
            {
                switch (Element)
                {
                    case EDamageType.normal:
                        if (enemyElement == EDamageType.water)
                        {
                            calcDmg = 2 * Damage;
                        }
                        else if (enemyElement == EDamageType.fire)
                        {
                            calcDmg = Damage / 2;
                        }
                        else
                        {
                            calcDmg = Damage;
                        }
                        break;
                    case EDamageType.fire:
                        if (enemyElement == EDamageType.normal)
                        {
                            calcDmg = 2 * Damage;
                        }
                        else if (enemyElement == EDamageType.water)
                        {
                            calcDmg = Damage / 2;
                        }
                        else
                        {
                            calcDmg = Damage;
                        }
                        break;
                    case EDamageType.water:
                        if (enemyElement == EDamageType.fire)
                        {
                            calcDmg = 2 * Damage;
                        }
                        else if (enemyElement == EDamageType.normal)
                        {
                            calcDmg = Damage / 2;
                        }
                        else
                        {
                            calcDmg = Damage;
                        }
                        break;
                    default: break;
                }
            }
            else
            {
                calcDmg = Damage;
            }
            return calcDmg;
        }
    }
}