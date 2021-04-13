namespace Port.Domain.Model
{
    public class Course : Entity
    {
        public static Course Chemistry { get; }= new Course(1, "Chemistry");
        public static Course Biology { get; }= new Course(2,"Biology");
        public static Course Math { get; }= new Course(3,"Maths");
        public string Name { get; private set; }

        protected Course(long id, string name)
            :base(id)
        {
            Name = name;
        }
    }
}