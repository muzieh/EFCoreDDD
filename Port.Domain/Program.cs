using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Port.Domain.Model;

namespace Port.Domain
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            var l = logger.CreateLogger("category");
            
            await using var context = new UsersDbContext();
           
            await SeedCourses(context);
            await context.SaveChangesAsync();
            
            var courses = new List<Course>();
            courses.AddRange(context.Courses.ToList());
            
            var studentFaker = new Faker<Student>()
                .CustomInstantiator(f =>
                    new Student(f.Name.FirstName() ,f.Name.LastName(), f.Internet.Email(), f.PickRandom(courses)));

            l.LogInformation("info from the morning doom");
            
            Enumerable.Range(0,100).ToList().ForEach(  _ =>
            {
                var student = studentFaker.Generate();
                 context.Students.Add(student);
                 context.SaveChanges();
            });


            var students = await context
                .Students
                //.Include(p => p.FavoriteCourse)
                .ToListAsync();
            
            foreach (var student in students)
            {
               Console.WriteLine(student.ToString());
               Console.WriteLine(student.FavoriteCourse.Name);
            }
            
            Console.WriteLine(context.Students.Find(4L));


        }

        public static async Task SeedCourses(UsersDbContext context)
        {
            if (!context.Courses.Any())
            {
                await context.Courses.AddAsync(new Course("Chemistry"));
                await context.Courses.AddAsync(new Course("Biology"));
                await context.Courses.AddAsync(new Course("Maths"));
            } 
            
        }
    }
}