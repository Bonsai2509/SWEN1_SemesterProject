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
        private string _host;
        private string _port;
        private string _userName;
        private string _password;
        private NpgsqlDataSource _datasource;
        private NpgsqlConnection _connection;

        public string connectionString;

        public PostgreSql(string dbname, string host, string port, string username, string password)
        {
            _host = host;
            _port = port;
            _userName = username;
            _password = password;
            _dbName = dbname;
            Start();
            CreateTabelsIfNotExist();
        }

        public void Start()
        {
            connectionString = $"Server={_host}; Port={_port}; Username={_userName}; Password={_password}; Database={_dbName};";
            _datasource = NpgsqlDataSource.Create(connectionString);
            _connection = _datasource.OpenConnection();
        }

        private void CreateTabelsIfNotExist()
        {
            CreateUserTable();
        }

        private void CreateUserTable()
        {
            using var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS user(
            username character varying(255)[] NOT NULL,
                bio character varying(255)[],
                image character varying(255)[],
                elo integer NOT NULL,
                wins integer NOT NULL,
                losses integer NOT NULL,
                draws integer NOT NULL,
                coins integer NOT NULL,
                CONSTRAINT user_pkey PRIMARY KEY (username)
                )", _connection);
            command.ExecuteNonQuery();
        }

        private void CreateStackTable()
        {
            using var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS stack(
                username character varying(255)[] NOT NULL,
                cardindex integer NOT NULL,
                inDeck boolean NOT NULL,
                inTrade boolean NOT NULL,
                cardid uuid NOT NULL,
                CONSTRAINT stack_pkey PRIMARY KEY (cardid),
                CONSTRAINT username FOREIGN KEY (username)
                    REFERENCES user (username) MATCH SIMPLE
                    ON UPDATE CASCADE
                    ON DELETE CASCADE
            )", _connection);
            command.ExecuteNonQuery();
        }
        
        private void CreateTradeTable()
        {
            using var command = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS stack(
                tradeid uuid NOT NULL,
                cardid uuid NOT NULL,
                cardtype character varying(100) NOT NULL,
                minDmg integer NOT NULL,
                CONSTRAINT trade_pkey PRIMARY KEY (tradeid),
                CONSTRAINT cardid FOREIGN KEY (cardid)
                    REFERENCES stack (cardid) MATCH SIMPLE
                    ON UPDATE CASCADE
                    ON DELETE CASCADE
            )", _connection);
            command.ExecuteNonQuery();
        }
    }
}
