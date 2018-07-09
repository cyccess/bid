using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //void StartTransaction();

        //void CommitTransaction();

        void RegisterNew<TEntity>(TEntity entity) where TEntity : class;

        void RegisterDirty<TEntity>(TEntity entity) where TEntity : class;

        void RegisterClean<TEntity>(TEntity entity) where TEntity : class;

        void RegisterDeleted<TEntity>(TEntity entity) where TEntity : class;

        bool Commit();

        void Rollback();
    }
}
