namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Channels;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //task 02
            //string? command = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(db, command));

            //task 03
            //Console.WriteLine(GetGoldenBooks(db));

            //task 04
            //Console.WriteLine(GetBooksByPrice(db));

            //task 05
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(db, year));

            //task 06
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(db, input));


            //task 07
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(db, date));

            //task 08
            //string input = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

            //task 09
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(db, input));

            //task 10
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(db, input));

            //task 11
            //int length = int.Parse(Console.ReadLine());
            //Console.WriteLine($"There are {CountBooks(db, length)} books with longer title than {length} symbols");

            //task 12
            //Console.WriteLine(CountCopiesByAuthor(db));

            //task 13
            //Console.WriteLine(GetTotalProfitByCategory(db));

            //task 14
            //Console.WriteLine(GetMostRecentBooks(db));

            //task 15
            //IncreasePrices(db);

            //task 16
            //Console.WriteLine(RemoveBooks(db));


        }

        //2.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            try
            {
                AgeRestriction commandEnum = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);
                var books = context.Books
                    .AsNoTracking()
                    .Where(b => b.AgeRestriction == commandEnum)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();

                return string.Join(Environment.NewLine, books);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        //3.Golden Books


        public static string GetGoldenBooks(BookShopContext context)
        {
            EditionType editionType = EditionType.Gold;

            var goldenBooks = context.Books
                .Where(b => b.EditionType == editionType && b.Copies<5000)
                .OrderBy(b=>b.BookId)
                .Select(b=>b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, goldenBooks);
        }

        //4.Books by Price

        public static string GetBooksByPrice(BookShopContext context)
        {
            var priceAndTitleOfBooks = context.Books
                .Where(b=>b.Price>40)
                .OrderByDescending(b=>b.Price)
                .Select (b=> new 
                {
                    Total = $"{b.Title} - ${b.Price:f2}"
                })
                .ToArray();

            return string.Join(Environment.NewLine, priceAndTitleOfBooks.Select(b=>b.Total));
        }

        //5.Not Released In

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            
            var booksNotRealeased = context.Books
                .Where(b=>b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select( b=>b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, booksNotRealeased);

        }

        //6.Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] catergories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context.BooksCategories
                .Where(c=>catergories.Contains(c.Category.Name.ToLower()))
                .Select(c=>c.Book.Title)
                .ToArray();


            return string.Join(Environment.NewLine, books);
        }

        //7.Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy",
             CultureInfo.InvariantCulture);

            var booksTitle = context.Books
                .Where(b => b.ReleaseDate < dt)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Total = $"{b.Title} - {b.EditionType} - ${b.Price}"
                })
                .ToArray();

            return string.Join (Environment.NewLine, booksTitle.Select(b=>b.Total));
        }

        //8.Author Search

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authorNames = context.Authors
                .Where(a=>a.FirstName.EndsWith(input))
                .Select(a=> new 
                { 
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .OrderBy(a => a)
                .ToArray();

            return string.Join(Environment.NewLine, authorNames.Select(a=>a.FullName));
                
        }

        //9.Book Search

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context.Books
                .Where(b=>b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b=>b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //10.Book Search by Author

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var allBooks = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    Output = $"{b.Title} ({$"{b.Author.FirstName} {b.Author.LastName}"})"
                })
                .ToArray();

            return string.Join(Environment.NewLine, allBooks.Select(x=>x.Output));

        }

        //11.Count Books

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var countOfBooks = context.Books
                .Where(b => b.Title.Length > lengthCheck).Count();

            return countOfBooks;
        }

        //12.Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                   .Select(a => new
                   {
                       a.FirstName,
                       a.LastName,
                       Copies = a.Books.Sum(b => b.Copies)
                   })
                   .OrderByDescending(b => b.Copies).ToArray();

            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FirstName} {a.LastName} - {a.Copies}"));
        }

        //13.Profit by Category

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profit = context.Categories
                .Select(b=> new 
                {
                  b.Name,
                  Profit = b.CategoryBooks.Sum(b=>b.Book.Price*b.Book.Copies)
                })
                .OrderByDescending(b=>b.Profit)
                .ToArray() ;

            return string.Join(Environment.NewLine, profit.Select(b => $"{b.Name} ${b.Profit:f2}"));
        }

        //14.	Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Take(3)
                        .Select(b => $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})")

                }).ToArray()
                .OrderBy(c => c.Name).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var b in books)
            {
                sb.AppendLine($"--{b.Name}");
                foreach (var mrb in b.MostRecentBooks)
                {
                    sb.AppendLine(mrb);
                }
            }
            return sb.ToString().TrimEnd();
        }

        //15.	Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var b in books)
            {
                b.Price += 5;
            }
        }

        //16.	Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
           .Where(b => b.Copies < 4_200)
           .ToArray();

            context.RemoveRange(books);
            context.SaveChanges();
            return books.Length;
        }
    }
}


