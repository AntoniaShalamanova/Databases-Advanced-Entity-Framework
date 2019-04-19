using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.Dto.Export;

namespace VaporStore.DataProcessor
{
    using System;
    using Data;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            Genre[] genres = genreNames
                .Select(gn => context.Genres.FirstOrDefault(g => g.Name == gn))
                .Where(genre => genre != null)
                .ToArray();

            ExportGenreDto[] genresDto = genres.Select(g => new ExportGenreDto()
            {
                GenreId = g.Id,
                GenreName = g.Name,
                Games = g.Games.Where(game => game.Purchases.Count != 0).Select(game => new ExportGameDto()
                {
                    GameId = game.Id,
                    GameName = game.Name,
                    DeveloperName = game.Developer.Name,
                    TagNames = string.Join(", ", game.GameTags.Select(gt => gt.Tag.Name).ToArray()),
                    PlayersCount = game.Purchases.Count
                })
                    .OrderByDescending(game => game.PlayersCount)
                    .ThenBy(game => game.GameId)
                    .ToArray(),
                TotalPlayers = g.Games.Sum(game => game.Purchases.Count)
            })
            .OrderByDescending(g => g.TotalPlayers)
            .ThenBy(g => g.GenreId)
            .ToArray();

            var jsonString = JsonConvert.SerializeObject(genresDto, Formatting.Indented);

            return jsonString;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            PurchaseType purchaseType = Enum.Parse<PurchaseType>(storeType);

            var users = context.Users
                .Where(u => u.Cards.SelectMany(c => c.Purchases).Any(p => p.Type == purchaseType))
                .ToArray();

            ExportUserDto[] usersDto = users.Select(u => new ExportUserDto()
            {
                UserName = u.Username,
                Purchases = u.Cards.SelectMany(c => c.Purchases)
                    .Where(p => p.Type == purchaseType)
                    .Select(p => new ExportPurchaseDto()
                    {
                        CardNumber = p.Card.Number,
                        CardCvc = p.Card.Cvc,
                        PurchaseDate = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new ExportPurchaseGameDto()
                        {
                            GameName = p.Game.Name,
                            GenreName = p.Game.Genre.Name,
                            GamePrice = p.Game.Price
                        }
                    })
                    .OrderBy(p => p.PurchaseDate)
                    .ToArray(),
                TotalSpent = u.Cards.SelectMany(c => c.Purchases)
                    .Where(p => p.Type == purchaseType)
                    .Sum(p => p.Game.Price)
            })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.UserName)
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportUserDto[]), new XmlRootAttribute("Users"));

            StringBuilder result = new StringBuilder();

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            serializer.Serialize(new StringWriter(result), usersDto, namespaces);

            return result.ToString().TrimEnd();
        }
    }
}