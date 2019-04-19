using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Cinema.DataProcessor.ExportDto;
using Newtonsoft.Json;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(m => m.Rating >= rating && m.Projections.Any(p => p.Tickets.Count > 0))
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("F2"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("F2"),
                    Customers = m.Projections.SelectMany(p => p.Tickets).Select(t => new
                    {
                        FirstName = t.Customer.FirstName,
                        LastName = t.Customer.LastName,
                        Balance = t.Customer.Balance.ToString("F2")
                    })
                        .OrderByDescending(c => c.Balance)
                        .ThenBy(c => c.FirstName)
                        .ThenBy(c => c.LastName)
                        .ToArray(),

                })
                .OrderByDescending(m => decimal.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToArray();

            string jsonString = JsonConvert.SerializeObject(movies, Formatting.Indented);

            return jsonString;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            //Use the method provided in the project skeleton, which receives customer age. Export customers with age above or equal to the given. For each customer, export their first name, last name, spent money for tickets(formatted to the second digit) and spent time(in format: "hh\:mm\:ss").Take first 10 records and order the result by spent money in descending order.

            var customers = context.Customers
                .Where(c => c.Age >= age)
                .ToArray();

            var customersDto = customers.Select(c => new ExportCustomerDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                SpentMoney = c.Tickets.Sum(t => t.Price).ToString("F2"),
                SpentTime = TimeSpan.FromSeconds(c.Tickets.Select(t => t.Projection.Movie.Duration).Sum(d => d.TotalSeconds)).ToString(@"hh\:mm\:ss")
            })
                .OrderByDescending(c => decimal.Parse(c.SpentMoney))
                .Take(10)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCustomerDto[]), new XmlRootAttribute("Customers"));

            StringBuilder result = new StringBuilder();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(new StringWriter(result), customersDto, namespaces);

            return result.ToString().TrimEnd();
        }
    }
}