using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Domain.Helpers;

namespace Nestor.Tools.Infrastructure.EntityFramework.Abstractions
{
    public class EntityWithCompositeIdMap<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntityWithComposedPrimaryKey
    {
        /// <summary>
        ///     Génère la configuration type pour l'entité <typeparamref name="TEntity" />
        ///     - La table générée sera du nom du type de l'entité
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tableName = typeof(TEntity).Name;
            var schema = typeof(TEntity).Namespace.ExtractSchemaFromDomain();

            builder.ToTable(tableName, schema);

            
        }
    }

    public class EntityMap<TEntity, TId> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity<TId>
    {
        /// <summary>
        ///     Génère la configuration type pour l'entité <typeparamref name="TEntity" />
        ///     - La table générée sera du nom du type de l'entité
        ///     - Le champ Id sera clé primaire
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tableName = typeof(TEntity).Name;
            var schema = typeof(TEntity).Namespace.ExtractSchemaFromDomain();

            builder.ToTable(tableName, schema);
            builder.HasKey(x => x.Id);

        }
    }    
}