using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.QrCode.Api.ClassesDB;

namespace Template.QrCode.Api.Repos
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly InterieurContext _InterieurContext;
        internal DbSet<TEntity> dbSet;

        public Repository(InterieurContext context)
        {
            _InterieurContext = context;
            this.dbSet = context.Set<TEntity>();
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }
            try
            {
                await _InterieurContext.AddAsync(entity);
                await _InterieurContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _InterieurContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                _InterieurContext.Update(entity);
                await _InterieurContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        //new 
        public TEntity GetById(object id)
        {
            try
            {
                return dbSet.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entitie: {ex.Message}");
            }
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_InterieurContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query,
        params object[] parameters)
        {

            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual void insertTraceAuth_Procedure(string query, SqlParameter[] parameters)
        {
            try
            {

                var t = _InterieurContext.TraceMobileApp.FromSqlRaw(query, parameters).ToList();
            }
            catch (Exception e)
            {


            }

        }


        public  Certificats GetCertificats(string query,SqlParameter[] parameters = null)
        {
            try
            {
                if(parameters != null)
                {
                    var t = _InterieurContext.Certificats.FromSqlRaw(query, parameters);

                    if (t.ToList().Count() == 0) return null;
                    else return t.ToList().FirstOrDefault();
                }
                else return _InterieurContext.Certificats.FromSqlRaw(query).ToList().FirstOrDefault();

            }
            catch (Exception e)
            {
                return null;

            }

        }

        public TraceMobileApp GetLastCertificatsFromTrace(string query, SqlParameter[] parameters)
        {
            try
            {
            
                    var t = _InterieurContext.TraceMobileApp.FromSqlRaw(query, parameters);

                    if (t.ToList().Count() == 0) return null;
                    else return t.ToList().FirstOrDefault();
               
            }
            catch (Exception e)
            {
                return null;

            }

        }


        public TraceMobileApp getLastToken(string query, SqlParameter[] parameters)
        {
            try
            {
                var t =  _InterieurContext.TraceMobileApp.FromSqlRaw(query, parameters);
                if (t.ToList().Count() == 0) return null;
                else return t.ToList().FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;

            }

        }




    }
}
