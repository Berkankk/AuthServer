using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepository<T> where T : class  //T burada az önce oluşturduğumuz classlar olacak model içindeki 
    {
        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetAllAsync();
      

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        //IQueryable: Veritabanında henüz sorgu çalıştırılmadan filtreleme yapma imkanı verir. Performans açısından faydalıdır.
        //Func burada bir delegedir ve geriye bool döner true ise çalışacak
        Task AddAsync(T entity);
        void Remove(T entity);

        T Update(T entity);
    }
}
