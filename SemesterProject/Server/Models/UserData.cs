using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Models
{
    internal class UserData
    {
        public string Name { get; }
        public string Bio { get; }
        public string Image { get; }

        public UserData(string username, string bio, string image)
        {
            Name = username;
            Bio = bio;
            Image = image;
        }
    }
}
