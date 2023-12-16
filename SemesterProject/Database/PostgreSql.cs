using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using static System.Net.Mime.MediaTypeNames;

namespace SemesterProject.Database
{
    internal class PostgreSql
    {
        private string _dbName;
        private string _server;
        private string _port;
        private string _userName;
        private string _password;
        private NpgsqlDataSource _datasource;
        public NpgsqlConnection connection;

        public string connectionString;

        public PostgreSql()
        {
            _server = "localhost";
            _port = "5432";
            _userName = "postgres";
            _password = "pass123";
            _dbName = "MTCG";
            Start();
            CreateTabelsIfNotExist();
        }

        public void Start()
        {
            connectionString = $"Server={_server}; Port={_port}; Username={_userName}; Password={_password}; Database={_dbName}; Pooling=false";
            _datasource = NpgsqlDataSource.Create(connectionString);
            connection = _datasource.OpenConnection();
        }

        public void DisposeDbConnection()
        {
            connection.Close();
            connection.Dispose();
        }

        private void CreateTabelsIfNotExist()
        {
            CreateUserTable();
            CreateStackTable();
            CreateTradeTable();
            CreatePackageTable();
        }

        private void CreateUserTable()
        {
            using var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS ""user""(
            elo double precision NOT NULL,
            wins integer NOT NULL,
            loses integer NOT NULL,
            draws integer NOT NULL,
            coins integer NOT NULL,
            username character varying(255) NOT NULL,
            password character varying(255) NOT NULL,
            bio character varying(255),
            image character varying(255),
            token character varying(100) NOT NULL,
            isAdmin boolean NOT NULL,
            hasDeck boolean NOT NULL,
            name character varying(255) NOT NULL,
            CONSTRAINT user_pkey PRIMARY KEY (username)
                )", connection);
            command.ExecuteNonQuery();
        }

        private void CreateStackTable()
        {
            using var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS stack(
                cardindex integer NOT NULL,
                inDeck boolean NOT NULL,
                inTrade boolean NOT NULL,
                cardid uuid NOT NULL,
                username character varying(255) NOT NULL,
                CONSTRAINT stack_pkey PRIMARY KEY (cardid),
                CONSTRAINT username FOREIGN KEY (username)
                    REFERENCES ""user"" (username) MATCH SIMPLE
                    ON UPDATE CASCADE
                    ON DELETE CASCADE
            )", connection);
            command.ExecuteNonQuery();
        }

        private void CreateTradeTable()
        {
            using var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS trades(
                tradeid uuid NOT NULL,
                cardid uuid NOT NULL,
                tradedtype character varying(100) NOT NULL,
                tradeddmg integer NOT NULL,
                wantedtype character varying(100) NOT NULL,
                wanteddmg integer NOT NULL,
                CONSTRAINT trades_pkey PRIMARY KEY (tradeid),
                CONSTRAINT cardid FOREIGN KEY (cardid)
                    REFERENCES stack (cardid) MATCH SIMPLE
                    ON UPDATE CASCADE
                    ON DELETE CASCADE
            )", connection);
            command.ExecuteNonQuery();
        }

        private void CreatePackageTable()
        {
            using var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS ""package""
            (
                packageid uuid NOT NULL,
                card1id uuid NOT NULL,
                card2id uuid NOT NULL,
                card3id uuid NOT NULL,
                card4id uuid NOT NULL,
                card5id uuid NOT NULL,
                CONSTRAINT package_pkey PRIMARY KEY (packageid)
            )", connection);
            command.ExecuteNonQuery();
        }
    }
}
