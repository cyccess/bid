using System;
using OAuth.Core.Interfaces;
using OAuth.Domain.Model;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using OAuth.Domain;

namespace OAuth.Data
{
    public class BaseRepository : IRepository
    {
        private readonly IDbContext _context;

        public BaseRepository(IDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return GetEntities<TEntity>();
        }

        private IDbSet<TEntity> GetEntities<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public TEntity GetById<TEntity>(int id) where TEntity : AggregateRoot
        {
            return (from item in GetAll<TEntity>()
                    where item.Id == id
                    select item).SingleOrDefault();
        }


        public IQueryable<TEntity> Find<TEntity>(Expression<Func<IAggregateRoot, bool>> whereExpr, Expression<Func<IAggregateRoot, TEntity>> selectExpr)
        {
            return GetEntities<IAggregateRoot>().Where(whereExpr).Select(selectExpr);
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }
    }
}
