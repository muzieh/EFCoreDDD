using System.Diagnostics;
using System.Reflection;
using FluentAssertions;
using Port.Domain.Model;
using Xunit;

namespace Port.DomainTests.Model
{
    public class EntityShould
    {
        [Fact]
        public void IfNullBeEqualToOtherNullEntity()
        {
            var actualResult = (null is Entity) == (null is Entity);
            actualResult.Should().Be(true);
        }
        
        [Fact]
        public void BeEqualToItself()
        {
            var e = new Entity();
            SetEntityIdWithReflection(e, 12);
            var actualResult = e == e ;
            actualResult.Should().Be(true);
        }
        
        [Fact]
        public void BeEqualToOtherEntityWithTheSameId()
        {
            var e1 = new Entity();
            SetEntityIdWithReflection(e1, 12);
            var e2 = new Entity();
            SetEntityIdWithReflection(e2, 12);
            var actualResult = e1 == e2 ;
            actualResult.Should().Be(true);
        }
        
        [Fact]
        public void NotBeEqualToOtherEntityWithDifferentId()
        {
            var e1 = new Entity();
            SetEntityIdWithReflection(e1, 12);
            var e2 = new Entity();
            SetEntityIdWithReflection(e2, 14);
            var actualResult = e1 == e2 ;
            actualResult.Should().Be(false);
        }
        
        [Fact]
        public void NotBeEqualToOtherEntityWithDifferentId_UsingNotEqualOperator()
        {
            var e1 = new Entity();
            SetEntityIdWithReflection(e1, 12);
            var e2 = new Entity();
            SetEntityIdWithReflection(e2, 14);
            var actualResult = e1 != e2 ;
            actualResult.Should().Be(true);
        }
        
        [Fact]
        public void NotBeEqualToOtherNullEntity()
        {
            var e1 = new Entity();
            SetEntityIdWithReflection(e1, 12);
            var actualResult = e1 == null ;
            actualResult.Should().Be(false);
        }
        
        [Fact]
        public void NotBeEqualToOtherNullEntity_UsingNotEqualOperator()
        {
            var e1 = new Entity();
            SetEntityIdWithReflection(e1, 12);
            var actualResult = e1 != null ;
            actualResult.Should().Be(true);
        }
        
        [Fact]
        public void IfNullNotBeEqualToOtherEntity_UsingNotEqualOperator()
        {
            Entity e1 = null;
            Entity e2 = new Entity();
            SetEntityIdWithReflection(e2, 43L);
            var actualResult = e1 != e2 ;
            actualResult.Should().Be(true);
        }

        private static void SetEntityIdWithReflection(Entity entity, long id)
        {
            var propName = "Id";
            var fieldInfo = typeof(Entity).GetField($"<{propName}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(entity, id);
        }
        
    }
}