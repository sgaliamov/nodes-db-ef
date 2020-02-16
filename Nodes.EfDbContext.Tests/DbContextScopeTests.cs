using System.Threading.Tasks;
using FluentAssertions;
using Grace.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Nodes.EfDbContext.Entities;
using Xunit;

namespace Nodes.EfDbContext.Tests
{
    public sealed class DbContextScopeTests
    {
        [Fact]
        public void DbContext_should_be_reused_in_one_scope()
        {
            using var container = GetContainer();

            using var scope = container.BeginLifetimeScope();

            var context1 = scope.Locate<NodesDbContext>();

            var context2 = scope.Locate<NodesDbContext>();

            context1.Should().BeSameAs(context2);
        }

        [Fact]
        public async Task Nodes_count_is_zero()
        {
            using var container = GetContainer();

            var context = container.Locate<NodesDbContext>();

            var count = await context.Nodes.CountAsync().ConfigureAwait(false);

            Assert.Equal(0, count);
        }

        private static DependencyInjectionContainer GetContainer()
        {
            var container = new DependencyInjectionContainer();

            container.Configure(c => {
                var builder = new DbContextOptionsBuilder()
                    .UseSqlServer(@"Data Source=(LocalDb)\TestDb;Initial Catalog=NodesDb;Integrated Security=True");

                c.Export<NodesDbContext>().Lifestyle.SingletonPerScope();
                c.ExportInstance(builder.Options);
                c.Export<DbContextProvider>().Lifestyle.Singleton();
            });

            return container;
        }
    }
}
