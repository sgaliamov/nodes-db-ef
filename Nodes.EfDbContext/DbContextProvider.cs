using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Nodes.EfDbContext
{
    public sealed class DbContextProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DbContextProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetContext<T>() where T : DbContext
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
