using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.Dto.Import;

namespace VaporStore.DataProcessor
{
    using System;
    using Data;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            var games = new List<Game>();

            StringBuilder result = new StringBuilder();

            foreach (var gameDto in gamesDto)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Count == 0)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game()
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };

                Developer developer = GetDeveloper(context, gameDto.Developer);

                game.Developer = developer;

                Genre genre = GetGenre(context, gameDto.Genre);

                game.Genre = genre;

                foreach (var tagName in gameDto.Tags)
                {
                    var tag = GetTag(context, tagName);

                    game.GameTags.Add(new GameTag()
                    {
                        Tag = tag,
                    });
                }

                result.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
                games.Add(game);
            }

            context.Games.AddRange(games);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDto = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);

            StringBuilder result = new StringBuilder();

            var users = new List<User>();

            foreach (var userDto in usersDto)
            {
                if (!IsValid(userDto) || !userDto.Cards.All(IsValid))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                User user = new User()
                {
                    Username = userDto.Username,
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Age = userDto.Age,
                    Cards = userDto.Cards.Select(c => new Card()
                    {
                        Number = c.Number,
                        Cvc = c.CVC,
                        Type = Enum.Parse<CardType>(c.Type)
                    }).ToArray()
                };

                result.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPurchaseDto[]), new XmlRootAttribute("Purchases"));

            var purchasesDto = (ImportPurchaseDto[])serializer.Deserialize(new StringReader(xmlString));

            StringBuilder result = new StringBuilder();

            var purchases = new List<Purchase>();

            foreach (var purchaseDto in purchasesDto)
            {
                if (!IsValid(purchaseDto))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                Game game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.GameName);

                Card card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.CardNumber);

                var isValidType = Enum.TryParse<PurchaseType>(purchaseDto.Type, out PurchaseType purchaseType);

                if (game == null || card == null || !isValidType)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                Purchase purchase = new Purchase()
                {
                    Type = purchaseType,
                    ProductKey = purchaseDto.ProductKey,
                    Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    Card = card,
                    Game = game
                };

                result.AppendLine($"Imported {game.Name} for {card.User.Username}");
                purchases.Add(purchase);
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static Tag GetTag(VaporStoreDbContext context, string tagName)
        {
            var tag = context.Tags.FirstOrDefault(t => t.Name == tagName);

            if (tag == null)
            {
                tag = new Tag()
                {
                    Name = tagName
                };

                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string genreName)
        {
            var genre = context.Genres.FirstOrDefault(g => g.Name == genreName);

            if (genre == null)
            {
                genre = new Genre()
                {
                    Name = genreName
                };

                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string devName)
        {
            var developer = context.Developers.FirstOrDefault(d => d.Name == devName);

            if (developer == null)
            {
                developer = new Developer()
                {
                    Name = devName
                };

                context.Add(developer);
                context.SaveChanges();
            }

            return developer;
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