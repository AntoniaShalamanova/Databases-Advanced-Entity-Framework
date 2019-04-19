using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Cinema.Data.Models;
using Cinema.Data.Models.Enums;
using Cinema.DataProcessor.ImportDto;
using Newtonsoft.Json;

namespace Cinema.DataProcessor
{
    using System;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();

            var moviesDto = JsonConvert.DeserializeObject<ImportMovieDto[]>(jsonString);

            var movies = new List<Movie>();

            foreach (var movieDto in moviesDto)
            {
                var isTitleExist = context.Movies.Select(m => m.Title)
                    .Contains(movieDto.Title);

                if (!IsValid(movieDto) || isTitleExist)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var movie = new Movie()
                {
                    Title = movieDto.Title,
                    Genre = Enum.Parse<Genre>(movieDto.Genre),
                    Duration = TimeSpan.Parse(movieDto.Duration),
                    Rating = movieDto.Rating,
                    Director = movieDto.Director
                };

                result.AppendLine(string.Format(SuccessfulImportMovie, movie.Title, movie.Genre, movie.Rating.ToString("F2")));
                movies.Add(movie);
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            StringBuilder result = new StringBuilder();

            var hallsDto = JsonConvert.DeserializeObject<ImportHallDto[]>(jsonString);

            var halls = new List<Hall>();

            foreach (var hallDto in hallsDto)
            {
                if (!IsValid(hallDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall()
                {
                    Name = hallDto.Name,
                    Is3D = hallDto.Is3D,
                    Is4Dx = hallDto.Is4Dx,
                };

                var seats = new HashSet<Seat>();

                for (int i = 0; i < hallDto.Seats; i++)
                {
                    seats.Add(new Seat()
                    {
                        Hall = hall
                    });
                }

                hall.Seats = seats;

                string projectionType = hall.Is3D && hall.Is4Dx ? "4Dx/3D" :
                    !hall.Is3D && !hall.Is4Dx ? "Normal" :
                    hall.Is3D ? "3D" :
                    "4Dx";

                result.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, projectionType, hall.Seats.Count));
                halls.Add(hall);
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProjectionDto[]), new XmlRootAttribute("Projections"));

            var projectionsDto = (ImportProjectionDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder result = new StringBuilder();

            var projections = new List<Projection>();

            foreach (var projectionDto in projectionsDto)
            {
                var movie = context.Movies.FirstOrDefault(m => m.Id == projectionDto.MovieId);
                var hall = context.Halls.FirstOrDefault(h => h.Id == projectionDto.HallId);

                if (!IsValid(projectionDto) || movie == null || hall == null)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var projection = new Projection()
                {
                    MovieId = projectionDto.MovieId,
                    HallId = projectionDto.HallId,
                    DateTime = DateTime.ParseExact(projectionDto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };

                result.AppendLine(string.Format(SuccessfulImportProjection, movie.Title, projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)));
                projections.Add(projection);
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomerDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder result = new StringBuilder();

            var customers = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                if (!IsValid(customerDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Customer customer = new Customer()
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Age = customerDto.Age,
                    Balance = customerDto.Balance
                };

                var tickets = new List<Ticket>();

                foreach (var ticketDto in customerDto.Tickets)
                {
                    tickets.Add(new Ticket()
                    {
                        ProjectionId = ticketDto.ProjectionId,
                        Customer = customer,
                        Price = ticketDto.Price
                    });
                }

                customer.Tickets = tickets;

                result.AppendLine(string.Format(SuccessfulImportCustomerTicket, customer.FirstName, customer.LastName, customer.Tickets.Count));
                customers.Add(customer);
            }


            context.Customers.AddRange(customers);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}