using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Abstraction;

namespace ZeiterfassungsTool.Abstraction
{
    public interface IBaseRepository<T> : IDisposable
        where T : TableData, new()
    {
        Task Init();
        Task SaveItem(T item);
        Task<T> GetItem(int id);
        Task<T> GetItem(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetItems();
        Task<List<T>> GetItems(Expression<Func<T, bool>> predicate);
        Task DeleteItem(T item);
        Task DropTable();
    }
}
