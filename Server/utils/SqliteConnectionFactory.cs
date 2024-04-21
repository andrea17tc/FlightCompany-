using System;
using System.Data;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Configuration;

namespace Server.utils
{
    public class SqliteConnectionFactory : ConnectionFactory
    {
        public override IDbConnection createConnection(IDictionary<string, string> props)
        {
            //Mono Sqlite Connection

            //String connectionString = "C:\\Users\\Andrea\\source\\repos\\MPP\\mpp-proiect-csharp-andrea17tc\\CompanieZbor\\companieZbor.db";
            String connectionString = "Data Source=companieZbor.db;Version=3";
            //String connectionString = props["ConnectionString"];
            Console.WriteLine("SQLite ---Se deschide o conexiune la  ... {0}", connectionString);
            return new SqliteConnection(connectionString);

            // Windows SQLite Connection, fisierul .db ar trebuie sa fie in directorul debug/bin
            //String connectionString = "Data Source=tasks.db;Version=3";
            //return new SQLiteConnection(connectionString);
        }
    }
}
