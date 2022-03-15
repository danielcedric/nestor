using System;
using Microsoft.EntityFrameworkCore;

namespace Nestor.Tools.Infrastructure.EntityFramework.UnitOfWork
{
    public interface IUnitOfWork
    {
        public interface IUnitOfWork<out TContext>
        where TContext : DbContext, new()
        {
            TContext Context { get; }
            void CreateTransaction();
            void Commit();
            void Rollback();
            void Save();
        }
    }
}
