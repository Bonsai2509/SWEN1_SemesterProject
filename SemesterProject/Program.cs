using SemesterProject.Server;

namespace SemesterProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpServer(10001);
            server.Start();
        }
    }
}