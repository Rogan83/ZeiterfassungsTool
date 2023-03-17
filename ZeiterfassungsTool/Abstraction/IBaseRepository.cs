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
        #region AsyncInterface
        //Task Init();
        //Task SaveItem(T item);
        ////Task SaveItemWithChildren(T item, bool recursive = false);
        //Task<T> GetItem(int id);
        //Task<T> GetItem(Expression<Func<T, bool>> predicate);
        //Task<List<T>> GetItems();
        //Task<List<T>> GetItems(Expression<Func<T, bool>> predicate);
        ////List<T> GetItemsWithChildren();
        //Task DeleteItem(T item);
        //Task DropTable();

        #endregion

        void Init();
        void SaveItem(T item);
        void SaveItemWithChildren(T item, bool recursive = false);
        T GetItem(int id);
        T GetItem(Expression<Func<T, bool>> predicate);
        List<T> GetItems();
        List<T> GetItems(Expression<Func<T, bool>> predicate);
        List<T> GetItemsWithChildren();
        void DeleteItem(T item);
        void DropTable();

        void CreateTable();
        void DeleteTable();
    }
}
