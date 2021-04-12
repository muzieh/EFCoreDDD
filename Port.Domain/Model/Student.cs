﻿namespace Port.Domain.Model
{
    public class Entity
    {
        public long Id { get; private set; }
    }
    
    public class Student : Entity
    {
       public string Email { get; private set; }
       public string FirstName { get; private set; }
       public string LastName { get; private set; }
       public Course FavoriteCourse { get; private set; }

       private Student()
       {
           
       }
       public Student(string firstName, string lastName, string email, Course favoriteCourse)
           :this()
       {
           FirstName = firstName;
           LastName = lastName;
           Email = email;
           FavoriteCourse = favoriteCourse;
       }

       public override string ToString() => $"{Id} {FirstName} {LastName} {Email} {FavoriteCourse}";
    }
    
}