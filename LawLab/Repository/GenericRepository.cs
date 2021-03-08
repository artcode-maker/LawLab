using LawLab.Models;
using System.Collections.Generic;
using System.Linq;

namespace LawLab.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T Get(long id);
        IEnumerable<T> GetAll();
        void Create(T newDataObject);
        void Update(T changedDataObject);
        void Delete(long id);
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private AppDbContext context;
        public GenericRepository(AppDbContext ctx)
        {
            context = ctx;
        }

        public void Create(T newDataObject)
        {
            context.Add(newDataObject);
            context.SaveChanges();
        }

        public void Delete(long id)
        {
            context.Remove<T>(Get(id));
            context.SaveChanges();
        }

        public T Get(long id)
        {
            return context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public void Update(T changedDataObject)
        {
            context.Update<T>(changedDataObject);
            context.SaveChanges();
        }
    }
}
