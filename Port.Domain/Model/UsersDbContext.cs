using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Port.Domain.Model
{
    public class UsersDbContext : DbContext
    {
       public DbSet<Student> Students { get; set; }
       public DbSet<Course> Courses { get; set; }

       public UsersDbContext()
       {
           this.Database.EnsureCreated();
       }
       
       /*public static readonly ILoggerFactory loggerFactory1 = new LoggerFactory(new[] {
                 new ConsoleLoggerProvider(new OptionsMonitor<ConsoleLoggerOptions>())
           });*/
       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
           var loggerFactory = LoggerFactory.Create(builder =>
           {
               builder.AddConsole();
           });
           
           optionsBuilder
               .UseNpgsql("Host=localhost;Username=postgres;Password=example;Database=Users", builder => builder
                   .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
               .UseLoggerFactory(loggerFactory)
              .EnableSensitiveDataLogging()
               .EnableDetailedErrors()
               .UseLazyLoadingProxies();


       }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.Entity<Student>(x =>
           {
               x.HasKey(u => u.Id);
               x.Property(p => p.Id).HasColumnName("StudentId");
               x.Property(p => p.Email);
               x.Property(p => p.FirstName);
               x.Property(p => p.LastName);
               x.HasOne(p => p.FavoriteCourse).WithMany();
           });

           modelBuilder.Entity<Course>(x =>
           {
               x.HasKey(u => u.Id);
               x.Property(p => p.Id).HasColumnName("CourseId");
               x.Property(p => p.Name);
           });

       }
    }
}