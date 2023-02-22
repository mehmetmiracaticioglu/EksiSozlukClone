using EksiSozlukClone.Core.Application.Interface.Repositories;
using EksiSozlukClone.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EksizSozlukClone.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly DbContext dbContex;
        protected DbSet<T> entity => dbContex.Set<T>();
        public GenericRepository(DbContext dbContex)
        { 
            this.dbContex = dbContex ?? throw new ArgumentNullException(nameof(dbContex));
        }

        public virtual int Add(T entity)
        {
            this.entity.Add(entity);
            return dbContex.SaveChanges();   
        }

        public virtual int Add(IEnumerable<T> entities)
        {
            this.entity.AddRange(entities);
            return dbContex.SaveChanges();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            await this.entity.AddAsync(entity);
            return await dbContex.SaveChangesAsync();
        }

        public virtual async Task<int> AddAsync(IEnumerable<T> entities)
        {
            await this.entity.AddRangeAsync(entities);
            return await dbContex.SaveChangesAsync();
        }

        public virtual int AddOrUpdate(T entity)
        {
            if (!this.entity.Local.Any(i=>EqualityComparer<Guid>.Default.Equals(i.Id,entity.Id)))
            {
                dbContex.Update(entity);
            }
            return dbContex.SaveChanges();
        }

        public virtual Task<int> AddOrUpdateAsync(T entity)
        {
            if(!this.entity.Local.Any(i => EqualityComparer<Guid>.Default.Equals(i.Id, entity.Id)))
            {
                dbContex.Update(entity);
            }
            return dbContex.SaveChangesAsync();
        }

        public virtual IQueryable<T> AsQuaryable() => entity.AsQueryable();

        public virtual async Task BulkAdd(IEnumerable<T> entities)
        {
            if (entities != null && entities.Any())
            {
                await Task.CompletedTask;
            }
            await entity.AddRangeAsync(entities);
            await dbContex.SaveChangesAsync();
        }

        public virtual Task BulkDeleteById(IEnumerable<Guid> Ids)
        {
            if (Ids!=null && Ids.Any())
            {
                return Task.CompletedTask;
            }
            dbContex.RemoveRange(entity.Where(i=>Ids.Contains(i.Id)));
            return dbContex.SaveChangesAsync();   
        }

        public virtual async Task BulkDeleteById(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                var entitiesToDelete = entity.Where(predicate).ToList();
                entity.RemoveRange(entitiesToDelete);
                await dbContex.SaveChangesAsync();
            }
        }

        public virtual async Task BulkDeleteById(IEnumerable<T> entities)
        {
            if (entities != null && entities.Any())
            {
                entity.RemoveRange(entities);
                await dbContex.SaveChangesAsync();
            }
        }

        public virtual Task BulkUpdate(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(T entity)
        {
            if (dbContex.Entry(entity).State == EntityState.Detached)
            {
                this.entity.Attach(entity);
            }
            this.entity.Remove(entity);

            return dbContex.SaveChanges();
        }

        public virtual int Delete(Guid id)
        {
            var entity = this.entity.Find(id);
            return Delete(entity);  
        }

        public virtual Task<int> DeleteAsync(T entity)
        {
            if (dbContex.Entry(entity).State==EntityState.Detached)
            {
                this.entity.Attach(entity);
            }
            this.entity.Remove(entity);
            
            return dbContex.SaveChangesAsync(); 
        }

        public virtual Task<int> DeleteAsync(Guid id)
        {
            var entity = this.entity.Find(id);
            return DeleteAsync(entity);
        }

        public virtual bool DeleteRange(Expression<Func<T, bool>> predicate)
        {
           dbContex.RemoveRange(entity.Where(predicate));
            return dbContex.SaveChanges() > 0;  
        }

        public virtual async Task<bool> DeleteRangeAsync(Expression<Func<T, bool>> predicate)
        {
            dbContex.RemoveRange(entity.Where(predicate));
            return await dbContex.SaveChangesAsync() > 0;
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var result = await query.FirstOrDefaultAsync(predicate);

            return result;
        }


        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();
            if (predicate!=null)
                query = query.Where(predicate);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            };
            if (noTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public Task<List<T>> GetAll(bool noTracking = true)
        {
            var query = entity.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            var result = query.ToListAsync();

            return result;
        }



        public virtual async Task<T> GetByIdAsync(Guid Id, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            T found = await entity.FindAsync(Id);

            if (found==null)
            {
                return null;
            }
            if (noTracking)
            {
                dbContex.Entry(found).State= EntityState.Detached;  
            }

            foreach (var include in includes)
            {
                dbContex.Entry(found).Reference(include).Load();
            }
            return found;


        }


        public virtual async Task<List<T>> GetList(Expression<Func<T, bool>> predicate, bool noTracking = true, Func<IQueryable<T>, IQueryable<T>> orderby = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = entity;
            if (predicate != null)
                query = query.Where(predicate);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            };

            if (orderby!=null)
            {
                query = orderby(query);
            }
            if (noTracking)
            {
                query = query.AsNoTracking(); 
            }

            return await query.ToListAsync();                      
        }

        public virtual async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, bool noTracking = true, params Expression<Func<T, object>>[] includes)
        {
            var query = entity.AsQueryable();

            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var result = await query.FirstOrDefaultAsync(predicate);

            return result;
        }


        public virtual int Update(T entity)
        {
            this.entity.Attach(entity);
            dbContex.Entry(entity).State=EntityState.Modified;
            return dbContex.SaveChanges();  
        }

        public virtual Task<int> UpdateAsync(T entity)
        {
            this.entity.Attach(entity);
            dbContex.Entry(entity).State = EntityState.Modified;
            return dbContex.SaveChangesAsync();

        }

        public Task<int> SaveChangesAsync()
        {
            return dbContex.SaveChangesAsync(); 
        }

        public int SaveChanges()
        {
            return dbContex.SaveChanges();  
        }


    }
}
