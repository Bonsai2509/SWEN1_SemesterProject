using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    public class UserData
    {
        public string Name { get; }
        public string Bio { get; }
        public string Image { get; }

        public UserData(string name, string bio, string image)
        {
            Name = name;
            Bio = bio;
            Image = image;
        }
    }
}
