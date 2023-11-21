using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SemesterProject.Cards.CardTypes
{
    internal class Dragon : Card
    {
        public Dragon(ECardType type, string name, int damage, EDamageType elem) : base(type, name, damage, elem)
        {
        }

        public override int CalcDmgAmount(Card EnemyCard)
        {
            int CalcDmg = 0;
            ECardType enemyType = EnemyCard.CardType;
            EDamageType enemyElement = EnemyCard.Element;
            if(EnemyCard.Name == "FireElve")
            {
                return 0;
            }
            else if (CardType == ECardType.spell || CardType == ECardType.special || enemyType == ECardType.spell || enemyType == ECardType.special)
            {
                switch (Element)
                {
                    case EDamageType.normal:
                        if (enemyElement == EDamageType.water)
                        {
                            CalcDmg = 2 * Damage;
                        }
                        else if (enemyElement == EDamageType.fire)
                        {
                            CalcDmg = Damage / 2;
                        }
                        else
                        {
                            CalcDmg = Damage;
                        }
                        break;
                    case EDamageType.fire:
                        if (enemyElement == EDamageType.normal)
                        {
                            CalcDmg = 2 * Damage;
                        }
                        else if (enemyElement == EDamageType.water)
                        {
                            CalcDmg = Damage / 2;
                        }
                        else
                        {
                            CalcDmg = Damage;
                        }
                        break;
                    case EDamageType.water:
                        if (enemyElement == EDamageType.fire)
                        {
                            CalcDmg = 2 * Damage;
                        }
                        else if (enemyElement == EDamageType.normal)
                        {
                            CalcDmg = Damage / 2;
                        }
                        else
                        {
                            CalcDmg = Damage;
                        }
                        break;
                    default: break;
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
