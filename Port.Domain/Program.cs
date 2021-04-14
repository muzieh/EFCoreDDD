using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Port.Domain.Controllers;
using Port.Domain.Model;
using Serilog;

namespace Port.Domain
{
    class Program
    {
        public static ServiceProvider Services;
        static async Task Main(string[] args)
        {
           Services = GetServiceProvider();
            var conectionString = GetConnectionString();
    
            var logger = Services
                .GetService<ILoggerFactory>()!
                .CreateLogger<Program>();
            
            await using var context = Services.GetService<UsersDbContext>();
           
            await SeedCourses(context);
            
            var courses = new List<Course>();
            courses.AddRange(context.Courses.ToList());
            if (!context.Students.Any())
            {
                SeedStudents(courses, context);
            }


            /*var students = await context
                .Students
                //.Include(p => p.FavoriteCourse)
                .ToListAsync();
            
            foreach (var student in students)
            {
               Console.WriteLine(student.ToString());
               Console.WriteLine(student.FavoriteCourse.Name);
            }*/

            var lis = context.Students.Where(s => s.FavoriteCourse == Course.Math).ToList();
            
            Console.WriteLine( context.Students.Find(4L));

            Execute((c) => new StudentController(c).CheckStudentFavoriteCourse(4L, 2L));



        }

        private static string Execute(Func<UsersDbContext, string> func)
        {
            var loggerFactory = Services.GetService<ILoggerFactory>();
            var connectionString = GetConnectionString();
            using var context = new UsersDbContext(loggerFactory, connectionString);
            return func(context);
        }

        private static string GetConnectionString()
        {
            var conf = Services.GetService<IConfiguration>();
            var positionOptions = new PositionOptions();
            conf.GetSection(PositionOptions.Position).Bind(positionOptions);
            
            return String.Empty;
            //return configuration.GetConnectionString("UsersDb");
        }
        
        private static ServiceProvider GetServiceProvider()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var serviceProvider = new ServiceCollection()
                .AddLogging( b => b.AddSerilog().SetMinimumLevel(LogLevel.Trace))
                .AddScoped( sp =>
                {
                    var loggerFactory = sp.GetService<ILoggerFactory>();
                    var configuration = sp.GetService<IConfiguration>();
                    var connectionString = configuration.GetConnectionString("UsersDB");
                    return new UsersDbContext(loggerFactory, connectionString);
                })
                .AddScoped<IConfiguration>( _ => configuration)
                .BuildServiceProvider();
            //loggerFactory.AddSerilog();
            return serviceProvider;
        }

        private static void SeedStudents(List<Course> courses, UsersDbContext context)
        {
            var studentFaker = new Faker<Student>()
                .CustomInstantiator(f =>
                    new Student(f.Name.FirstName(), f.Name.LastName(), f.Internet.Email(), f.PickRandom(courses)));


            Enumerable.Range(0, 100).ToList().ForEach(_ =>
            {
                var student = studentFaker.Generate();
                context.Students.Add(student);
                context.SaveChanges();
            });
        }

        public static async Task SeedCourses(UsersDbContext context)
        {
            if (!context.Courses.Any())
            {
                await context.Courses.AddAsync(Course.Chemistry);
                await context.Courses.AddAsync(Course.Biology);
                await context.Courses.AddAsync(Course.Math);
                await context.SaveChangesAsync();
            } 
            
        }
    }
    
    public class PositionOptions
    {
        public const string Position = "Position";

        public string Title { get; set; }
        public string Name { get; set; }
    }
}