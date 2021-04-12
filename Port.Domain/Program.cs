using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Port.Domain.Model;

namespace Port.Domain
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await using var context = new UsersDbContext();
            var studentFaker = new Faker<Student>()
                .CustomInstantiator(f => new Student(f.Name.FirstName() ,f.Name.LastName(), f.Internet.Email(), new Course("course 1")));
            
            /*
            Enumerable.Range(0,100).ToList().ForEach( _ =>
            {
                context.Students.Add(studentFaker.Generate());
                context.SaveChanges();
            });
            */


            var students = await context.Students.Include(p => p.FavoriteCourse).ToListAsync();
            
            foreach (var student in students)
            {
               Console.WriteLine(student.ToString());
            }
        }
    }
}