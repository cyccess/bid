using OAuth.Domain.Model;
using System;
using System.Linq;
using OAuth.Domain;
using System.Linq.Expressions;

namespace OAuth.Core.Interfaces
{
    /// <summary>
    /// 数据仓储接口
    /// </summary>
    public interface IRepository : IDisposable
    {
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

        TEntity GetById<TEntity>(int id) where TEntity : AggregateRoot;

        IQueryable<TEntity> Find<TEntity>(Expression<Func<IAggregateRoot, bool>> whereExpr,
            Expression<Func<IAggregateRoot, TEntity>> selectExpr);

    }
}
