using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    public class CardData
    {
        public CardData(Guid cardId, string cardName, string cardDescription, float cardDamage)
        {
            CardId = cardId;
            CardName = cardName;
            CardDescription = cardDescription;
            CardDamage = cardDamage;
        }
        public Guid CardId { get; }
        public string CardName { get; set; }
        public string CardDescription { get; set; }
        public float CardDamage { get; }
    }
}
