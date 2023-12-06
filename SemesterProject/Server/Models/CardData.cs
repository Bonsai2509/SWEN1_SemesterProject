using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class CardData
    {
        public CardData(Guid cardId, string cardName, float cardDamage)
        {
            CardId = cardId;
            CardName = cardName;
            CardDamage = cardDamage;
        }
        public Guid CardId { get; }
        public string CardName { get; set; }
        public float CardDamage { get; }
    }
}
