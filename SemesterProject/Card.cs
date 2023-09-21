using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject
{
    internal class Card
    {
        public Card(string type, string name, int damage, string damagetype)
        {
            CardType = type;
            Name = name;
            Damage = damage;
            DamageType = damagetype;
        }
        public string CardType { get; private set; }
        public string Name { get; private set; }
        public int Damage { get; set; } = 0;
        public string DamageType { get; set; };
    }
}
