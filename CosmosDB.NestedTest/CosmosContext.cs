using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CosmosDB.NestedTest
{
    public class CosmosContext : DbContext
    {
        public CosmosContext(DbContextOptions options) : base(options)
        { }


        public DbSet<Library> Libraries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Library>()
                .ToContainer("libraries")
                .HasPartitionKey(x => x.OwnerId)
                .HasKey(x => x.Id);


            builder.Entity<Library>()
                .OwnsMany(x => x.Books, n =>
                {
                    n.OwnsMany(x => x.Pages, ln =>
                    {
                        ln.OwnsMany(x => x.Notes);
                        ln.OwnsMany(x => x.Highlights);
                    });

                });
        } 
 
    }
}
