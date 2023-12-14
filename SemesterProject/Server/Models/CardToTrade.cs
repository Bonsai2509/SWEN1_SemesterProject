using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class Trade
    {
        public Guid Id { get; }
        public Guid CardToTrade { get; }
        public string Type { get; }
        public float MinimumDamage { get; }

        public Trade(Guid id, Guid cardToTrade, string type, float minimumDamage)
        {
            Id = id;
            CardToTrade = cardToTrade;
            Type = type;
            MinimumDamage = minimumDamage;
        }
    }
}
