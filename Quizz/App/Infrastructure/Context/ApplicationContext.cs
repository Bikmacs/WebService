using Microsoft.EntityFrameworkCore;
using Quizz.App.Domain.Models;
using Quizz.App.Domain.Models.User;

namespace Quizz.App.Infrastructure.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Person> People { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Дополнительные настройки модели, если необходимо
        }
    }
}