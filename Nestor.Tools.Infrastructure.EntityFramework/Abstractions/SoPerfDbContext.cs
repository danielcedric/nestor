using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nestor.Tools.Infrastructure.EntityFramework.Abstractions
{
    //public class NestorDbContext<TContext> : DbContext
    //    where TContext : DbContext
    //{
    //    static NestorDbContext()
    //    {

    //    }

    //    protected NestorDbContext() : base()
    //    {
    //        Database.Migrate();
    //    }

    //    protected NestorDbContext(DbContextOptions<TContext> options) : base(options)
    //    {
    //        Database.Migrate();
    //    }

    //}

    public class NestorDbContext : DbContext
    {
        static NestorDbContext()
        {

        }

        protected NestorDbContext() : base()
        {
            Database.Migrate();
        }

        protected NestorDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var s = GetAllMaps();

        }

        /// <summary>
        /// Obtient toutes les classes qui sont déclarées comme étant des maps
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> GetAllMaps()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(IEntityTypeConfiguration<>).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList();
        }
    }
}
