using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nestor.Tools.Domain.Entities;
using Nestor.Tools.Infrastructure.EntityFramework.Exceptions;
using Nestor.Tools.Infrastructure.EntityFramework.Repository;
using static Nestor.Tools.Infrastructure.EntityFramework.UnitOfWork.IUnitOfWork;

namespace Nestor.Tools.Infrastructure.EntityFramework.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
         where TContext : DbContext, new()
    {
        //Here TContext is nothing but your DBContext class
        //In our example it is EmployeeDBContext class
        private readonly TContext context;
        private bool disposed;
        private string errorMessage = string.Empty;
        private IDbContextTransaction transaction;
        private Dictionary<string, object> _repositories;

        //Using the Constructor we are initializing the _context variable is nothing but
        //we are storing the DBContext (EmployeeDBContext) object in _context variable
        public UnitOfWork()
        {
            context = new TContext();
        }
        //The Dispose() method is used to free unmanaged resources like files, 
        //database connections etc. at any time.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //This Context property will return the DBContext object i.e. (EmployeeDBContext) object
        public TContext Context
        {
            get { return context; }
        }
        //This CreateTransaction() method will create a database Trnasaction so that we can do database operations by
        //applying do evrything and do nothing principle
        public void CreateTransaction()
        {
            transaction = context.Database.BeginTransaction();
        }
        //If all the Transactions are completed successfuly then we need to call this Commit() 
        //method to Save the changes permanently in the database
        public void Commit()
        {
            transaction.Commit();
        }
        //If atleast one of the Transaction is Failed then we need to call this Rollback() 
        //method to Rollback the database changes to its previous state
        public void Rollback()
        {
            transaction.Rollback();
            transaction.Dispose();
        }
        //This Save() Method Implement DbContext Class SaveChanges method so whenever we do a transaction we need to
        //call this Save() method so that it will make the changes in the database
        public void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", string.Join(", ",validationError.MemberNames), validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(errorMessage, dbEx);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    context.Dispose();
            disposed = true;
        }

        public DbRepository<T> DbRepository<T>() where T : class, IEntity<Guid>
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(DbRepository<T>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                _repositories.Add(type, repositoryInstance);
            }
            return (DbRepository<T>)_repositories[type];
        }
    }
}
