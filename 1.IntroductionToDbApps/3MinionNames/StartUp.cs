using _1InitialSetup;
using System;
using System.Data.SqlClient;

namespace _3MinionNames
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWithDb))
            {
                connection.Open();

                string selectVillainQuery = @"SELECT Name FROM Villains WHERE Id = @Id";

                string selectMinionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                     m.Name, 
                                                     m.Age
                                              FROM MinionsVillains AS mv
                                              JOIN Minions As m ON mv.MinionId = m.Id
                                              WHERE mv.VillainId = @Id
                                              ORDER BY m.Name";

                using (SqlCommand villainCommand = new SqlCommand(selectVillainQuery, connection))
                {
                    villainCommand.Parameters.AddWithValue("@Id", villainId);

                    string villainName = (string)villainCommand.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {villainName}");

                    using (SqlCommand minionsCommand = new SqlCommand(selectMinionsQuery, connection))
                    {
                        minionsCommand.Parameters.AddWithValue("@Id", villainId);

                        using (SqlDataReader reader = minionsCommand.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("(no minions)");
                                return;
                            }

                            while (reader.Read())
                            {
                                Int64 rowNumber = (Int64)reader[0];
                                string name = (string)reader[1];
                                int age = (int)reader[2];

                                Console.WriteLine($"{rowNumber}. {name} {age}");
                            }
                        }
                    }
                }
            }
        }
    }
}
