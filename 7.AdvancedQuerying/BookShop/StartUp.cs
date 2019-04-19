using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BookShop.Models;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
//using Z.EntityFramework.Plus;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);

                using (var context = new BookShopContext())
                {
                    //string commandString = Console.ReadLine();
                    //int commandInt = int.Parse(Console.ReadLine());

                    //Console.WriteLine(GetBooksByAgeRestriction(context, commandString));
                    //Console.WriteLine(GetGoldenBooks(context));
                    //Console.WriteLine(GetBooksByPrice(context));
                    //Console.WriteLine(GetBooksNotReleasedIn(context, commandInt));
                    //Console.WriteLine(GetBooksByCategory(context, commandString));
                    //Console.WriteLine(GetBooksReleasedBefore(context, commandString));
                    //Console.WriteLine(GetAuthorNamesEndingIn(context, commandString));
                    //Console.WriteLine(GetBookTitlesContaining(context, commandString));
                    //Console.WriteLine(GetBooksByAuthor(context, commandString));
                    //Console.WriteLine(CountBooks(context, commandInt));
                    //Console.WriteLine(CountCopiesByAuthor(context));
                    //Console.WriteLine(GetTotalProfitByCategory(context));
                    //Console.WriteLine(GetMostRecentBooks(context));
                    //IncreasePrices(context);
                    //Console.WriteLine(RemoveBooks(context));
                }
            }
        }

        //1
        public static string GetBooksByAgeRestriction(BookShopContext context, string commandString)
        {
            AgeRestriction AgeRestriction = Enum.Parse<AgeRestriction>(commandString, true);

            string[] bookNames = context.Books
                .Where(b => b.AgeRestriction == AgeRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, bookNames);
        }

        //2
        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] goldenBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, goldenBooks);
        }

        //3
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                }).ToArray()
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //4
        public static string GetBooksNotReleasedIn(BookShopContext context, int commandInt)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.GetValueOrDefault().Year != commandInt)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //5
        public static string GetBooksByCategory(BookShopContext context, string commandString)
        {
            string[] categories = commandString.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //6
        public static string GetBooksReleasedBefore(BookShopContext context, string commandString)
        {
            var targetDate = DateTime.ParseExact(commandString, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate.Value < targetDate)
                .OrderByDescending(b => b.ReleaseDate.Value)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                }).ToArray()
                .ToArray();

            string result = string.Join(Environment.NewLine,
                books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}"));

            return result;
        }

        //7
        public static string GetAuthorNamesEndingIn(BookShopContext context, string commandString)
        {
            var authors = context.Authors
                .Where(a => EF.Functions.Like(a.FirstName, $"%{commandString}"))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(a => a)
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        //8
        public static string GetBookTitlesContaining(BookShopContext context, string commandString)
        {
            var books = context.Books
                .Where(b => EF.Functions.Like(b.Title, $"%{commandString}%"))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //9
        public static string GetBooksByAuthor(BookShopContext context, string commandString)
        {
            var books = context.Books
                .Include(b => b.Author)
                .Where(b => EF.Functions.Like(b.Author.LastName, $"{commandString}%"))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }

        //10
        public static int CountBooks(BookShopContext context, int commandInt)
        {
            return context.Books
                .Count(b => b.Title.Length > commandInt);
        }

        //11
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorsCopies = context.Authors
                .Include(a => a.Books)
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    Copies = a.Books.Sum(b => b.Copies),
                }).ToArray()
                .OrderByDescending(a => a.Copies)
                .ToArray();

            return string.Join(Environment.NewLine,
                authorsCopies.Select(x => $"{x.FirstName} {x.LastName} - {x.Copies}"));
        }

        //12
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoriesTotalProfit = context.Categories
                .Include(c => c.CategoryBooks)
                .ThenInclude(cb => cb.Book)
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price),
                }).ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            return string.Join(Environment.NewLine,
                categoriesTotalProfit.Select(c => $"{c.Name} ${c.TotalProfit:F2}"));
        }

        //13
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
                .Include(c => c.CategoryBooks)
                .ThenInclude(bc => bc.Book)
                .Select(c => new
                {
                    c.Name,
                    Books = c.CategoryBooks.Select(cb => new
                    {
                        cb.Book.Title,
                        ReleaseDate = cb.Book.ReleaseDate.Value
                    })
                        .OrderByDescending(cb => cb.ReleaseDate)
                        .Take(3)
                        .ToArray()
                }).ToArray()
                .OrderBy(c => c.Name)
                .ToArray();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //14
        public static void IncreasePrices(BookShopContext context)
        {
            //context.Books
            // .Where(b => b.ReleaseDate.Value.Year < 2010)
            // .Update(b => new Book() { Price = b.Price + 5 });

            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        //15
        public static int RemoveBooks(BookShopContext context)
        {
            //return context.Books
            //    .Where(b => b.Copies < 4200)
            //    .Delete();

            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Length;
        }
    }
}
