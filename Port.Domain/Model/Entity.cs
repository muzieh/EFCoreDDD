using System;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualBasic.CompilerServices;

namespace Port.Domain.Model
{

    public class Entity
    {
        public long Id { get; }

        protected Entity(long id)
        {
            Id = id;
        }

        //TODO: should be protected. It's only to fix broken tests
        public Entity()
        {
            
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Entity other)) return false;

            if (ReferenceEquals(this, other)) return true;

            if (GetRealType(this) != GetRealType(other)) return false;

            //transient objects not saved to the database yet
            if (Id == 0 || other.Id == 0) return false;

            return Id == other.Id;
        }

        private System.Type GetRealType(object obj)
        {
            if(obj == null)
                throw new ArgumentNullException("obj");
            
            return obj.GetType().FullName.Contains("Castle.Proxies") ? obj.GetType().BaseType : obj.GetType();
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return Equals(a, b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
            
    }
}