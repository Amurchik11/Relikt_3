using Microsoft.EntityFrameworkCore;
using Relikt_2_DataAccess.Repository.IRepository;
using Relikt_2_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Relikt_2_DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if(filter!=null)
            {
                query = query.Where(filter);
            }
            if(includeProperties!=null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if(!isTracking)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }

        public Page<T> GetPage(PageInfo page, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var totalCount = query.Count();

            if (page != null)
            {
                query = query.Skip(page.OffSet).Take(page.PageSize);
            }

            var items = query.AsNoTracking().ToList();

            return new Page<T>
            {
                Size = page.PageSize,
                Items = items,
                Number = page.PageNumber, 
                TotalCount = totalCount,
            };
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }

    public class PageInfo
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; } = 1;

        public int OffSet => PageSize * (PageNumber - 1);
    }
}
