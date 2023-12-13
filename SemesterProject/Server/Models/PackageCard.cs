using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class PackageCard
    {
        public string Id { get; }
        public int Index { get; }

        public PackageCard(string id, int index)
        {
            Id = id;
            Index = index;
        }
    }
}
