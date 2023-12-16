﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Cards.CardTypes
{
    internal class Goblin : Card
    {
        public Goblin(ECardType type, string name, string description, int damage, EDamageType elem) : base(type, name, description, damage, elem)
        {
        }
        public override int CalcDmgAmount(Card EnemyCard)
        {
            int CalcDmg = 0;
            ECardType enemyType = EnemyCard.CardType;
            EDamageType enemyElement = EnemyCard.Element;
            if (EnemyCard.Name == "Dragon")
            {
                return 0;
            }
            else if (CardType == ECardType.spell || CardType == ECardType.special || enemyType == ECardType.spell || enemyType == ECardType.special)
            {
                CalcDmg = CheckElementDmg(EnemyCard);
            }
            else
            {
                CalcDmg = Damage;
            }
            return CalcDmg;
        }
    }
}
