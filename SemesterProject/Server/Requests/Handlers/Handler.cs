using Npgsql;
using SemesterProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Requests.Handlers
{
    internal class Handler
    {
        public PostgreSql Database { get; }
        public NpgsqlConnection Connection { get; }

        public Handler() 
        {
            Database = new PostgreSql();
            Connection = Database.connection;
        }
    }
}
