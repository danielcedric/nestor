using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Nestor.Tools.Infrastructure.EntityFramework
{
    public class NestorDbContext : DbContext
    {
        public NestorDbContext() : base()
        {
           
        }

        public NestorDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var s = GetAllMaps();
        }

        /// <summary>
        ///     Obtient toutes les classes qui sont déclarées comme étant des maps
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