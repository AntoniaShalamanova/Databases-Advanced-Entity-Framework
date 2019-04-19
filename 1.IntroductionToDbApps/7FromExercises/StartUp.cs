using _1InitialSetup;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _7FromExercises
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            string selectMinionsName = @"SELECT Name FROM Minions";

            List<string> names = new List<string>();

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringWithDb))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectMinionsName, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add((string)reader[0]);
                        }
                    }
                }
            }

            for (int i = 0; i < names.Count / 2; i++)
            {
                Console.WriteLine(names[i]);
                Console.WriteLine(names[names.Count - i - 1]);
            }

            if (names.Count % 2 != 0)
            {
                Console.WriteLine(names[names.Count / 2]);
            }
        }
    }
}
