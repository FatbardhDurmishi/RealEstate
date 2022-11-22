using Microsoft.EntityFrameworkCore;
using RealEstate.App.Interfaces;
using RealEstate.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.App.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DBRealEstateContext _db;
        internal DbSet<T> dbSet { get; set; }
        public Repository(DBRealEstateContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
            _db.SaveChanges();
        }

        //includeProp - "Category, CoverType"
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);

            }
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query;

            query = dbSet;

            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
            _db.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            _db.SaveChanges();
        }

        public int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void Update(T entity)
        {
            try
            {
                _db.Set<T>().Update(entity);
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateRange(IEnumerable<T> entity)
        {
            try
            {
                _db.Set<T>().UpdateRange(entity);
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
