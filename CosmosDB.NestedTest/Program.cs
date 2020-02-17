using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace CosmosDB.NestedTest
{
    class Program
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => {
                builder
                    .AddDebug()
                    .AddFilter("", LogLevel.Debug);
            });

        static void Main(string[] args)
        {
            var context = GetContext();
            var id = Guid.NewGuid();
            var notes = new List<Note>();
            var highlights = new List<Highlight> { new Highlight("This is a Highlight.") };
            var pages = new List<Page> { new Page(1, notes, highlights) };
            var books = new List<Book> {new Book("Book 1", pages)};
            context.Libraries.Add(new Library(id, Guid.NewGuid(), books));

            context.SaveChanges();
            context.Dispose();

            context = GetContext();
            var first = context.Libraries.First(x => x.Id == id);

            Console.WriteLine($"Books: {first.Books.Count}");
            Console.WriteLine($"Pages: {first.Books.FirstOrDefault()?.Pages?.Count}");
            Console.WriteLine($"Notes: {first.Books.FirstOrDefault()?.Pages?.FirstOrDefault()?.Notes?.Count}");
            Console.WriteLine($"Highlights: {first.Books.FirstOrDefault()?.Pages?.FirstOrDefault()?.Highlights?.Count}");

            Console.ReadKey();
        }

        private static CosmosContext GetContext()
        { 
            var optionsBuilder = new DbContextOptionsBuilder<CosmosContext>();

            var cosmosEndpoint = "https://localhost:8081";
            var cosmosAuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            var cosmosDbName = "test";
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging().UseCosmos(cosmosEndpoint, cosmosAuthKey, cosmosDbName);
            var context = new CosmosContext(optionsBuilder.Options);

            context.Database.EnsureCreated();

            return context;
        }
    }
}
