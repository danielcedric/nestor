using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestor.Tools.Domain.Abstractions;
using Nestor.Tools.Domain.Helpers;

namespace Nestor.Tools.Infrastructure.EntityFramework.Abstractions
{
    public class EntityWithCompositeIdMap<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntityWithCompositeId
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

    public class EntityWithIdMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntityWithId
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

    public class EntityWithIdMap<TEntity, TId> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntityWithId<TId> where TId : IEquatable<TId>
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

    public class EntityWithTrackingMap<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntityWithTracking
    {
        /// <summary>
        ///     Génère la configuration type pour l'entité <typeparamref name="TEntity" />
        ///     - La table générée sera du nom du type de l'entité
        ///     - Le champ Id sera clé primaire
        ///     - Les champs CreatedAd, CreatedBy, UpdatedAd, UpdatedBy sont mappés.
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tableName = typeof(TEntity).Name;
            var schema = typeof(TEntity).Namespace.ExtractSchemaFromDomain();

            builder.ToTable(tableName, schema);
            builder.HasKey(x => x.Id);

            builder.Property(c => c.CreatedAt).IsRequired().HasDefaultValueSql("getutcdate()");
            builder.Property(c => c.CreatedBy).IsRequired().HasMaxLength(100);
            builder.Property(c => c.UpdatedAt);
            builder.Property(c => c.UpdatedBy).HasMaxLength(100);
        }
    }
}