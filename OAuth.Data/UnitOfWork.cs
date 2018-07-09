using OAuth.Core.Interfaces;
using System.Transactions;

namespace OAuth.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _dbContext;

        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        #region TransactionScope

        //private TransactionScope transaction;


        //public void StartTransaction()
        //{
        //    this.transaction = new TransactionScope();
        //}

        //public void CommitTransaction()
        //{
        //    this.transaction.Complete();
        //}


        #endregion


        public void RegisterNew<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void RegisterDirty<TEntity>(TEntity entity) where TEntity : class
        {
            //_dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public void RegisterClean<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Entry(entity).State = System.Data.Entity.EntityState.Unchanged;
        }

        public void RegisterDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public bool Commit()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public void Rollback()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            if (_dbContext != null) _dbContext.Dispose();
            //if (this.transaction != null)
            //{
            //    this.transaction.Dispose();
            //}
        }
    }
}
