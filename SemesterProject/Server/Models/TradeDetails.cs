using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class TradeDetails
    {
        public Guid TradeId { get; }
        public Guid CardId { get; }
        public string CardType { get; }
        public float CardDmg { get; }

        public TradeDetails(Guid tradeId, Guid cardId, string cardType, float cardDmg)
        {
            TradeId = tradeId;
            CardId = cardId;
            CardType = cardType;
            CardDmg = cardDmg;
        }
    }
}
