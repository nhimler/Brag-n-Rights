using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymBro_App.DAL.Tests
{
    public static class TestHelpers
    {
        public static Mock<DbSet<T>> CreateMockSet<T>(IList<T> data) where T : class
        {
            var queryable = data.AsQueryable();

            // Create the mock DbSet
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                   .Returns(() => queryable.GetEnumerator());

            // Support AddAsync
            mockSet
            .Setup(d => d.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
            .Callback<T, CancellationToken>((entity, _) => data.Add(entity))
            .Returns<T, CancellationToken>((entity, ct) =>
                // return a ValueTask with a null EntityEntry<T>
                new ValueTask<EntityEntry<T>>((EntityEntry<T>?)null)
            );

            // Support Remove
            mockSet
              .Setup(d => d.Remove(It.IsAny<T>()))
              .Callback<T>(entity => data.Remove(entity));

            return mockSet;


        }
    }
}
