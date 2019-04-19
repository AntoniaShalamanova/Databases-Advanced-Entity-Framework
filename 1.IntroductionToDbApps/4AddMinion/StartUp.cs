using _1InitialSetup;
using System;
using System.Data.SqlClient;

namespace _4AddMinion
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string[] minionInfo = Console.ReadLine().Split();

            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string minionTown = minionInfo[3];

            string villainName = Console.ReadLine().Split()[1];

            int? townId;
            int? villainId;
            int? minionId;

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWithDb))
            {
                connection.Open();

                //SqlTransaction transaction = connection.BeginTransaction();
                //try
                //{
                //    SqlCommand command = new SqlCommand();
                //    command.ExecuteScalar();
                //    transaction.Commit();
                //}
                //catch (Exception)
                //{
                //    transaction.Rollback();
                //    throw;
                //}

                townId = GetTownId(minionTown, connection);

                if (townId == null)
                {
                    AddTown(minionTown, connection);
                }

                townId = GetTownId(minionTown, connection);

                villainId = GetVillainId(villainName, connection);

                if (villainId == null)
                {
                    AddVillain(villainName, connection);
                }

                villainId = GetVillainId(villainName, connection);

                minionId = GetMinionId(minionName, connection);

                if (minionId == null)
                {
                    AddMinion(minionName, minionAge, townId, connection);
                }
                
                minionId = GetMinionId(minionName, connection);

                InsertIntoMinionsVillains(villainId, minionId, villainName, minionName, connection);
            }
        }

        private static int? GetMinionId(string minionName, SqlConnection connection)
        {
            string selectMinionId = @"SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectMinionId, connection))
            {
                command.Parameters.AddWithValue("@Name", minionName);

                return (int?)command.ExecuteScalar();
            }
        }

        private static int? GetVillainId(string villainName, SqlConnection connection)
        {
            string selectVillainId = @"SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectVillainId, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);

                return (int?)command.ExecuteScalar();
            }
        }

        private static int? GetTownId(string minionTown, SqlConnection connection)
        {
            string selectTownId = @"SELECT Id FROM Towns WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(selectTownId, connection))
            {
                command.Parameters.AddWithValue("@Name", minionTown);

                return (int?)command.ExecuteScalar();
            }
        }

        private static void InsertIntoMinionsVillains(int? villainId, int? minionId, string villainName, string minionName, SqlConnection connection)
        {
            string query = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@minionId, @villainId)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.Parameters.AddWithValue("@minionId", minionId);

                command.ExecuteNonQuery();

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
        }

        private static void AddMinion(string minionName, int minionAge, int? townId, SqlConnection connection)
        {
            string insertMinion = @"INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(insertMinion, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", minionAge);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }
        }

        private static void AddVillain(string villainName, SqlConnection connection)
        {
            string insertVillain = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(insertVillain, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);

                command.ExecuteNonQuery();

                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }

        private static void AddTown(string minionTown, SqlConnection connection)
        {
            string insertTown = @"INSERT INTO Towns (Name) VALUES (@townName)";

            using (SqlCommand command = new SqlCommand(insertTown, connection))
            {
                command.Parameters.AddWithValue("@townName", minionTown);

                command.ExecuteNonQuery();

                Console.WriteLine($"Town {minionTown} was added to the database.");
            }
        }
    }
}
