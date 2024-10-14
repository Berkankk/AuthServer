using SharedLibrary.Dto;
using SharedLibrary.Respo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<T, TDto> where T : class where TDto : class //T değeri class ve tdto da class olacak 
    {
        Task<Response<TDto>> GetByIdAsync(int id);

        Task<Response<IEnumerable<TDto>>> GetAllAsync();


        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<T, bool>> predicate);
        //IQueryable: Veritabanında henüz sorgu çalıştırılmadan filtreleme yapma imkanı verir. Performans açısından faydalıdır.
        //Func burada bir delegedir ve geriye bool döner true ise çalışacak
        Task<Response<TDto>> AddAsync(TDto entity);
        Task<Response<NoDataDto>> Remove(int id);

        Task<Response<NoDataDto>> Update(TDto entity, int id);
    }
}
