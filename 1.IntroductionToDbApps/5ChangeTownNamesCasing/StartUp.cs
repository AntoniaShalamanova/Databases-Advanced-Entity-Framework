using _1InitialSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _5ChangeTownNamesCasing
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string countryName = Console.ReadLine();
            int rowsAffected;

            string updateQuery = @"UPDATE Towns
                                       SET Name = UPPER(Name)
                                       WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

            string selectQuery = @" SELECT t.Name 
                                    FROM Towns as t
                                    JOIN Countries AS c ON c.Id = t.CountryCode
                                    WHERE c.Name = @countryName";

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWithDb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);

                    rowsAffected = command.ExecuteNonQuery();
                }

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No town names were affected.");
                    return;
                }

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@countryName", countryName);

                    Console.WriteLine($"{rowsAffected} town names were affected.");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<string> townsAffected = new List<string>();

                        while (reader.Read())
                        {
                            townsAffected.Add((string)reader[0]);
                        }

                        Console.WriteLine($"[{string.Join(", ", townsAffected)}]");
                    }
                }
            }
        }
    }
}
