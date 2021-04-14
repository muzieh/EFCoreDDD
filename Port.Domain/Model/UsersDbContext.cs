using Castle.Core.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Port.Domain.Model
{
    public class UsersDbContext : DbContext
    {
       public DbSet<Student> Students { get; set; }
       public DbSet<Course> Courses { get; set; }
       
       private readonly ILoggerFactory _loggerFactory;
       
       private readonly string _connectionString;

       public UsersDbContext(ILoggerFactory loggerFactory, string connectionString)
       {
           _loggerFactory = loggerFactory;
           _connectionString = connectionString;
           
           this.Database.EnsureCreated();
       }

       public UsersDbContext(ILoggerFactory loggerFactory, IConfigurationRoot configuration)
            :this(loggerFactory, configuration.GetConnectionString("UsersDb"))
       {
           
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
               .UseNpgsql(_connectionString, builder => builder
                   .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
               .UseLoggerFactory(_loggerFactory)
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