using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Laftika.DAL
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T              GetById(int id);
        void           Insert(T entity);
        void           Update(T entity);
        void           Delete(int id);
        Task<bool>     Save();
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T>  Entities;

        public GenericRepository(DbContext context)
        {
            Context  = context;
            Entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return Entities.AsEnumerable();
        }

        public T GetById(int id)
        {
            return Entities.Find(id);
        }

        public void Insert(T entity)
        {
            Entities.Add(entity);
        }

        public void Update(T entity)
        {
            Entities.Update(entity);
        }

        public void Delete(int id)
        {
            Entities.Remove(GetById(id));
        }

        public async Task<bool> Save()
        {
            await Context.SaveChangesAsync();

            return true;
        }
    }
}