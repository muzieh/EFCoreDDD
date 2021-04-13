using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Port.Domain.Model;
using Serilog;

namespace Port.Domain
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices(out var loggerFactory);

            var logger = loggerFactory.CreateLogger<Program>();
            await using var context = serviceProvider.GetService<UsersDbContext>();
           
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
            
            Console.WriteLine(context.Students.Find(4L));


        }

        private static ServiceProvider ConfigureServices(out ILoggerFactory? loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddLogging( b => b.AddSerilog().SetMinimumLevel(LogLevel.Trace))
                .AddDbContext<UsersDbContext>()
                .BuildServiceProvider();

            loggerFactory = serviceProvider.GetService<ILoggerFactory>();
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
}