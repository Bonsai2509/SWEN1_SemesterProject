using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum ECardType
{
    monster=0, spell=1
}

enum EDamageType
{
    normal = 0, fire = 1, water = 2
}


namespace SemesterProject
{
    abstract internal class Card
    {
        public Card(ECardType type, string name, int damage, EDamageType elem)
        {
            this.CardType = type;
            Name = name;
            Damage = damage;
            Element = elem;
        }
        public ECardType CardType { get; }

        

        public string Name { get; private set; }
        private int Damage { get;  }
        public EDamageType Element { get; set; }


        public int CalcDmgAmount(ECardType EnemyType, EDamageType EnemyElement)
        {
            int CalcDmg = 0;

            if(CardType == ECardType.spell || EnemyType == ECardType.spell)
            {
                switch(Element)
                {
                    case EDamageType.normal:
                        if (EnemyElement == EDamageType.water)
                        {
                            CalcDmg = 2 * Damage;
                        }
                        else if (EnemyElement == EDamageType.fire)
                        { 
                            CalcDmg = Damage/2;
                        }
                        else
                        {
                            CalcDmg = Damage;
                        }
                        break;
                     case EDamageType.fire:
                        if (EnemyElement == EDamageType.normal)
                        {
                            CalcDmg = 2 * Damage;
                        }
                        else if (EnemyElement == EDamageType.water)
                        {
                            CalcDmg = Damage / 2;
                        }
                        else
                        {
                            CalcDmg = Damage;
                        }
                        break;
                    case EDamageType.water:
                        if (EnemyElement == EDamageType.fire)
                        {
                            CalcDmg = 2 * Damage;
                        }
                        else if (EnemyElement == EDamageType.normal)
                        {
                            CalcDmg = Damage / 2;
                        }
                        else
                        {
                            CalcDmg = Damage;
                        }
                        break;
                    default:break;
                }
            }
            else
            {
                CalcDmg = Damage;
            }
            return CalcDmg;
        }
    }
}