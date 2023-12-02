using SemesterProject.Server;
using SemesterProject.Database;

namespace SemesterProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpServer(10001);
            server.Start();
            //var postgre = new PostgreSql("MTCG", "localhost", "5432", "postgres", "pass123");
        }
    }
}