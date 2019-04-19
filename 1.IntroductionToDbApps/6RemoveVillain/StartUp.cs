using _1InitialSetup;
using System;
using System.Data.SqlClient;

namespace _6RemoveVillain
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());
            string villainName;
            int releasedMinionsCount;

            string selectVillainName = @"SELECT Name FROM Villains WHERE Id = @villainId";
            string deleteFromMinionsVillains = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
            string deleteFromVillains = @"DELETE FROM Villains WHERE Id = @villainId";

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWithDb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectVillainName, connection))
                {
                    command.Parameters.AddWithValue("@villainId", villainId);

                    villainName = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine("No such villain was found.");
                        return;
                    }
                }

                releasedMinionsCount = ExecuteNonQuery(deleteFromMinionsVillains, villainId, connection);

                ExecuteNonQuery(deleteFromVillains, villainId, connection);

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{releasedMinionsCount} minions were released.");
            }
        }

        private static int ExecuteNonQuery(string cmdText, int villainId, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);

                return command.ExecuteNonQuery();
            }
        }
    }
}
