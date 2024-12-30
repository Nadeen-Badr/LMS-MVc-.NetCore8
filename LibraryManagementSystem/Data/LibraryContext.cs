using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models; // Namespace for your models

namespace LibraryManagementSystem.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        // DbSets for your entities
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example of configuring relationships (if needed)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Book)
                .WithMany()
                .HasForeignKey(t => t.BookId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Member)
                .WithMany()
                .HasForeignKey(t => t.MemberId);
        }
    }
}
