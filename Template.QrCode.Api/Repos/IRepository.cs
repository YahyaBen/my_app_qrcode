using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Repos
{
    public interface IRepository<TEntity> where TEntity : class , new() 
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);

         TEntity GetById(object id);
         IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);
         void Delete(TEntity entityToDelete);
         void Delete(object id);
    }
}
