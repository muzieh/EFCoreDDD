namespace Port.Domain.Model
{
    public class Course : Entity
    {
        public string Name { get; private set; }

        public Course(string name)
        {
            Name = name;
        }
    }
}