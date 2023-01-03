using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Domain.DomainEvents;
using Nestor.Tools.Infrastructure.Repository;
using Nestor.Tools.Infrastructure.EntityFramework.Exceptions;
using static Nestor.Tools.Infrastructure.EntityFramework.UnitOfWork.IUnitOfWork;
using Nestor.Tools.Exceptions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using Nestor.Tools.Domain.DataAnnotations;
using Nestor.Tools.Helpers;

namespace Nestor.Tools.Infrastructure.EntityFramework.Repository
{
    /// <summary>
    ///     Repository de base pour Entity Framework
    /// </summary>
    /// <typeparam name="TContext">Contexte Entity Framework</typeparam>
    /// <typeparam name="TEntity">Type de l'entité concernée</typeparam>
    /// <typeparam name="TId">Type d'identifiant</typeparam>
    public abstract class DbRepository<TEntity, TId> : IDbRepository<TEntity, TId>, IDisposable
        where TEntity : class, IEntity<TId>
    {
        private DbSet<TEntity> entities;
        private string errorMessage = string.Empty;
        private bool isDisposed;

        #region Properties
        public virtual IQueryable<TEntity> Table
        {
            get { return Entities; }
        }
        /// <summary>
        /// Get the entities
        /// </summary>
        protected virtual DbSet<TEntity> Entities
        {
            get { return entities ??= Context.Set<TEntity>(); }
        }
        #endregion

        public DbRepository(IUnitOfWork<NestorDbContext> unitOfWork)
            : this(unitOfWork.Context)
        {
        }

        public DbRepository(NestorDbContext context)
        {
            Context = context;
            isDisposed = false;
        }

        /// <summary>
        ///     Affecte ou obtient le contexte
        /// </summary>
        public NestorDbContext Context { get; private set; }

        #region Methods

        public TEntity Add(TEntity entity)
        {
            Entities.Add(entity);
            Context.SaveChanges();

            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
            await Context.SaveChangesAsync();

            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                Entities.Add(entity);

                if (Context == null || isDisposed)
                    Context = new NestorDbContext();

                //Context.SaveChanges(); commented out call to SaveChanges as Context save changes will be 
                //called with Unit of work
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", string.Join(", ", validationError.MemberNames), validationError.ErrorMessage) + Environment.NewLine;
                throw new NestorException(errorMessage, dbEx);
            }
        }

        /// <summary>
        /// Insère un ensemble d'entités
        /// </summary>
        /// <param name="entities"></param>
        public void BulkInsert(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }

                Context.ChangeTracker.AutoDetectChangesEnabled = false;

                var validationResults = new List<ValidationResult>();
                foreach (var entity in entities)
                {
                    if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
                    {
                        // throw new ValidationException() or do whatever you want
                    }
                    else
                        Entities.AddRange(entities);
                }

                if (!validationResults.Any())
                    Context.SaveChanges();
                else
                    throw new DbEntityValidationException(validationResults);
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationError in dbEx.EntityValidationErrors)
                {
                        errorMessage += string.Format("Property: {0} Error: {1}", string.Join(", ", validationError.MemberNames),
                                             validationError.ErrorMessage) + Environment.NewLine;
                    
                }
                throw new NestorException(errorMessage, dbEx);
            }
        }

        /// <summary>
        /// Mets à jour l'entité
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (Context == null || isDisposed)
                    Context = new NestorDbContext();

                SetEntryModified(entity);
                //Context.SaveChanges(); cappel commenté à SaveChanges car les changements de sauvegarde de contexte seront appelés avec l'unitofwork
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors)
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", string.Join(", ", validationError.MemberNames), validationError.ErrorMessage);

                throw new Exception(errorMessage, dbEx);
            }
        }

        /// <summary>
        /// Supprime l'entité passée en paramètre
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                if (Context == null || isDisposed)
                    Context = new NestorDbContext();

                Entities.Remove(entity);
                //Context.SaveChanges(); appel commenté à SaveChanges car les changements de sauvegarde de contexte seront appelés avec l'unitofwork
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors)
                        errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", string.Join(", ", validationError.MemberNames), validationError.ErrorMessage);
                throw new Exception(errorMessage, dbEx);
            }
        }

        /// <summary>
        /// Change l'état de l'entité en Modified
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SetEntryModified(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        ///     Attache l'entité passée en paramètre au contexte courant
        /// </summary>
        /// <param name="entity"></param>
        public void Attach(TEntity entity)
        {
            Entities.Attach(entity);
        }

        /// <summary>
        ///     Attache la collection d'entitées passée en paramètre au contexte courant
        /// </summary>
        /// <param name="entity"></param>
        public void Attach(IEnumerable<TEntity> entities)
        {
            Entities.AttachRange(entities);
        }

        public ICollection<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).ToList();
        }

        public async Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate,
            ICollection<string> includes = null, bool noTracking = false)
        {
            IQueryable<TEntity> table = Entities;
            if (includes != null)
                foreach (var include in includes)
                    table = table.Include(include);
            if (noTracking)
                return await table.Where(predicate).AsNoTracking().ToListAsync();
            return await table.Where(predicate).ToListAsync();
        }

        public TEntity GetById(TId id)
        {
            return Entities.Where(entity => entity.Id.Equals(id)).SingleOrDefault();
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await Entities.Where(entity => entity.Id.Equals(id)).SingleOrDefaultAsync();
        }

        /// <summary>
        ///     Obtient une <see cref="TEntity" /> depuis les propriétés qui composent sa clé unique
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NoUniqueByAttributeForTypeException">Excpetion levée si l'entité ne dispose d'aucun <see cref="UniqueByAttribute"/> </exception>
        /// <exception cref="UniqueByAttributeIsEmptyException"></exception>
        /// <returns></returns>
        public virtual Task<TEntity> GetByUniqueKeyAsync(TEntity obj)
        {
            var uniqueByAttribute = typeof(TEntity).GetCustomAttributes(typeof(UniqueByAttribute), true).SingleOrDefault() as UniqueByAttribute;

            if (uniqueByAttribute == null)
                throw new NoUniqueByAttributeForTypeException<TEntity>();

            if (!uniqueByAttribute.PropertyNames.Any())
                throw new UniqueByAttributeIsEmptyException<TEntity>();

            var uniqueKeyProperties = AttributeHelper.GetValueAndPropertyNameDictionary(obj, uniqueByAttribute.PropertyNames);

            // Composition du critère de requête
            var query = Query();
            foreach (var uniqueKeyProperty in uniqueKeyProperties)
            {
                var prop = typeof(TEntity).GetProperty(uniqueKeyProperty.Key);
                if (prop.PropertyType.IsSubclassOf(typeof(Entity)))
                    throw new CantExecuteAutoUniqueKeyQueryOnClassPropertyException(uniqueKeyProperty.Key);
                else
                {
                    ParameterExpression pe = Expression.Parameter(typeof(TEntity));

                    // Le type est un type valeur
                    if (uniqueKeyProperty.Value == null)
                    {
                        // Cas de la valeur nulle
                        Expression left = Expression.Property(pe, uniqueKeyProperty.Key);
                        Expression right = Expression.Constant(null, typeof(object));

                        Expression expression = Expression.Equal(left, right);
                    }
                    else
                    {
                        Expression left = Expression.Property(pe, uniqueKeyProperty.Key);
                        Expression right = Expression.Constant(uniqueKeyProperty.Value);

                        Expression expression = Expression.Equal(left, right);
                    }
                }
            }

            return query.SingleOrDefaultAsync();
        }

        /// <summary>
        ///     Obtient une <see cref="TEntity" /> depuis les propriétés qui composent sa clé unique
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual TEntity GetByUniqueKey(TEntity obj)
        {
            return GetByUniqueKeyAsync(obj).Result;
        }

        public ICollection<TEntity> List()
        {
            return Entities.ToList();
        }

        public async Task<ICollection<TEntity>> ListAsync()
        {
            return await Entities.ToListAsync();
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }

        /// <summary>
        ///     Méthode d'execution d'un traitement Entity Framework dans un nouveau contexte de transaction
        /// </summary>
        /// <param name="action">Méthode à executer</param>
        /// <param name="parameter">Paramètres</param>
        public void Execute(Action<TEntity> action, TEntity parameter)
        {
            Execute(action, parameter);
        }

        /// <summary>
        ///     Méthode d'execution d'un traitement Entity Framework dans un nouveau contexte de transaction
        /// </summary>
        /// <param name="action">Méthode à executer</param>
        /// <param name="parameter">Paramètres</param>
        /// <param name="onSuccessAction">Méthode à éxecuter en cas de succès</param>
        /// <param name="clearSession">Indique la session nHibernate doit être effacée après le traitement</param>
        public void Execute(Action<TEntity> action, TEntity parameter, Action<TEntity> onSuccessAction)
        {
            Execute(action, parameter, onSuccessAction, null);
        }

        /// <summary>
        ///     Méthode d'execution d'un traitement Entity Framework dans un nouveau contexte de transaction
        /// </summary>
        /// <param name="action">Méthode à executer</param>
        /// <param name="parameter">Paramètres</param>
        /// <param name="onErrorAction">Méthode à executer en cas d'erreur</param>
        public void Execute(Action<TEntity> action, TEntity parameter, Action<EntityFrameworkException> onErrorAction)
        {
            Execute(action, parameter, onErrorAction);
        }

        /// <summary>
        ///     Méthode d'execution d'un traitement Entity Framework dans un nouveau contexte de transaction
        /// </summary>
        /// <param name="action">Méthode à executer</param>
        /// <param name="parameter">Paramètres</param>
        /// <param name="onSuccessAction">Méthode à executer en cas de succès</param>
        /// <param name="onErrorAction">Méthode à executer en cas d'erreur</param>
        /// <param name="clearSession">Indique la session nHibernate doit être nettoyée après le traitement</param>
        public void Execute(Action<TEntity> action, TEntity parameter, Action<TEntity> onSuccessAction,
            Action<EntityFrameworkException> onErrorAction)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    action(parameter);

                    // Déclenchement des événements de domaine si l'entité est un aggrégat
                    if(parameter is AggregateRoot)
                        DomainEventStore.DispatchEventsForAggregate((parameter as AggregateRoot).Id);

                    transaction.Commit();

                    onSuccessAction?.Invoke(parameter);
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    onErrorAction?.Invoke(new EntityFrameworkException(e, parameter));
                }
            }
        }

        /// <summary>
        ///     Effectue un comptage des éléments de type <typeparamref name="TEntity" />, la clause where sera passée en paramètre
        ///     grâce à un prédicat
        /// </summary>
        /// <param name="predicate">Prédicat de sélection</param>
        /// <returns>Le nombre d'éléments</returns>
        public int CountBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).Count();
        }

        public Task<int> CountByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Where(predicate).CountAsync();
        }

        public IQueryable<TEntity> Query()
        {
            return Entities.AsQueryable();
        }

        /// <summary>
        ///     Force le rechargement de l'entité passée en paramètre
        /// </summary>
        /// <param name="entity">entité à recharger</param>
        public async Task ReloadAsync(TEntity entity)
        {
            await Context.Entry(entity).ReloadAsync();
        }

        /// <summary>
        ///     Force le rechargement des entités passées en paramètre
        /// </summary>
        /// <param name="entities">entités à recharger</param>
        public async Task ReloadAsync(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities) await ReloadAsync(item);
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            isDisposed = true;
        }

        #endregion
    }

    public abstract class DbRepository<TEntity> : DbRepository<TEntity, Guid>, IDbRepository<TEntity, Guid>
        where TEntity : class, IEntity<Guid>
    {
        public DbRepository(NestorDbContext context) : base(context)
        {
        }
    }
}