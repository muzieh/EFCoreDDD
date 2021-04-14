using Microsoft.AspNetCore.Connections.Features;
using Port.Domain.Model;

namespace Port.Domain.Controllers
{
    public class StudentController
    {
        private readonly UsersDbContext _context;

        public StudentController(UsersDbContext context)
        {
            _context = context;
        }

        public string CheckStudentFavoriteCourse(long studentId, long courseId)
        {
            var student = _context.Students.Find(studentId);
            if (student == null)
                return "Student not found";

            var course = _context.Courses.Find(courseId);

            if (course == null)
                return "Cours not found";
            
            return student.FavoriteCourse == course ?
                "Yes" : "No";
        }
    }
}