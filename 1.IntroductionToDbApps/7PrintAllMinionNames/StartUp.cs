using _1InitialSetup;
using System;
using System.Data.SqlClient;

namespace _7PrintAllMinionNames
{
    public class StartUp
    {
        public static int firstId;
        public static int lastId;

        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWithDb))
            {
                connection.Open();

                InitializeFirstId(connection);
                InitializeLastId(connection);

                int halfDiff = (lastId) / 2;
                bool isOdd = false;

                if (lastId % 2 != 0)
                {
                    isOdd = true;
                }

                while (lastId >= (halfDiff + 1) && firstId <= (halfDiff))
                {
                    Console.WriteLine(GetMinionName(firstId, connection));
                    Console.WriteLine(GetMinionName(lastId, connection));

                    firstId++;
                    lastId--;
                }

                if (isOdd)
                {
                    Console.WriteLine(GetMinionName(halfDiff + 1, connection));
                }
            }
        }

        private static void InitializeLastId(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("SELECT MAX(Id) FROM Minions", connection))
            {
                lastId = (int)command.ExecuteScalar();
            }
        }

        private static void InitializeFirstId(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("SELECT MIN(Id) FROM Minions", connection))
            {
                firstId = (int)command.ExecuteScalar();
            }
        }

        private static string GetMinionName(int id, SqlConnection connection)
        {
            string selectMinionName = @"SELECT Name FROM Minions WHERE Id = @minionId";

            using (SqlCommand command = new SqlCommand(selectMinionName, connection))
            {
                command.Parameters.AddWithValue("@minionId", id);
                return (string)command.ExecuteScalar();
            }
        }
    }
}
