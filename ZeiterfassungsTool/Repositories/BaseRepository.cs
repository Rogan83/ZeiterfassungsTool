using Newtonsoft.Json.Linq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Abstraction;
using ZeiterfassungsTool.Models;
using SQLiteNetExtensions.Extensions;

namespace SQLiteDemo.Repositories
{
    public class BaseRepository<T> :
        IBaseRepository<T> where T : TableData, new()
    {
        string _dbPath;
        
        public string StatusMessage { get; set; }

        #region SQLiteAsyncConnection Code
        //public SQLiteAsyncConnection connection;

        //public BaseRepository(string dbPath)
        //{
        //    _dbPath = dbPath;
        //}



        //public async Task Init()
        //{
        //    //if (connection  != null)
        //    //    return;

        //    if (connection == null)
        //        connection = new SQLiteAsyncConnection(_dbPath);

        //    await connection.CreateTableAsync<T>();         // nicht ideal, weil auf diese Weise die Tabelle Timetracking ohne Fremdschlüssel hinzugefügt wird. Eig. wollte ich es so lösen, dass die "CreateTableAsync<T>" Methode nur dann ausgelöst wird, wenn T nicht vom Typ Timetracking ist, aber ich weiß nicht, wie man das umsetzt.

        //}

        //public async Task DeleteItem(T item)
        //{
        //    try
        //    {
        //        await Init();
        //        await connection.DeleteAsync(item);
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //            $"Error: {ex.Message}";
        //    }
        //}

        //public void Dispose()
        //{
        //    connection.CloseAsync();
        //}

        //public async Task<T> GetItem(int id)
        //{
        //    try
        //    {
        //        await Init();
        //        return await connection.Table<T>()
        //            .FirstOrDefaultAsync(x => x.Id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //            $"Error: {ex.Message}";
        //    }
        //    return null;
        //}

        //public async Task<T> GetItem(Expression<Func<T, bool>> predicate)
        //{
        //    try
        //    {
        //        await Init();
        //        return await connection.Table<T>()
        //            .Where(predicate).FirstOrDefaultAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //           $"Error: {ex.Message}";
        //    }
        //    return null;
        //}

        //public async Task<List<T>> GetItems()
        //{
        //    try
        //    {
        //        await Init();  
        //        return await connection.Table<T>().ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //           $"Error: {ex.Message}";
        //    }
        //    return null;
        //}

        //public async Task<List<T>> GetItems(Expression<Func<T, bool>> predicate)
        //{
        //    try
        //    {
        //        await Init(); 
        //        return await connection.Table<T>().Where(predicate).ToListAsync();    
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //           $"Error: {ex.Message}";
        //    }
        //    return null;
        //}

        //public async Task SaveItem(T item)
        //{
        //    int result = 0;
        //    try
        //    {
        //        await Init();
        //        if (item.Id != 0)
        //        {
        //            result =
        //                await connection.UpdateAsync(item);
        //            StatusMessage =
        //            $"{result} row(s) updated";
        //        }
        //        else
        //        {
        //            result = await connection.InsertAsync(item); 
        //            StatusMessage =
        //                $"{result} row(s) added";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //            $"Error: {ex.Message}";
        //    }
        //}

        //public async Task DropTable()
        //{
        //    try
        //    {
        //        await Init();
        //        await connection.DropTableAsync<T>();
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //            $"Error: {ex.Message}";
        //    }
        //}

        //public async Task SaveItemWithChildren(T item, bool recursive = false)
        //{

        //}

        //public async Task<List<T>> GetItemsWithChildren()
        //{
        //    try
        //    {
        //        return await connection
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //           $"Error: {ex.Message}";
        //    }
        //    return null;
        //}
        #endregion



        public SQLiteConnection connection;

        public BaseRepository(string dbPath)
        {
            _dbPath = dbPath;
            Init();
        }

        public void Init()
        {
            //if (connection  != null)
            //    return;

            if (connection == null)
                connection = new SQLiteConnection(_dbPath);

             connection.CreateTable<T>();         // nicht ideal, weil auf diese Weise die Tabelle Timetracking ohne Fremdschlüssel hinzugefügt wird. Eig. wollte ich es so lösen, dass die "CreateTableAsync<T>" Methode nur dann ausgelöst wird, wenn T nicht vom Typ Timetracking ist, aber ich weiß nicht, wie man das umsetzt.

        }

        public void DeleteItem(T item)
        {
            try
            {
                Init();
                connection.Delete(item,true);           //Damit auch die Timetable daten gelöscht werden (hoffentlich)   
            }
            catch (Exception ex)
            {
                StatusMessage =
                    $"Error: {ex.Message}";
            }
        }

        public void Dispose()
        {
            connection.Close();
        }

        public T GetItem(int id)
        {
            try
            {
                Init();
                return connection.Table<T>()
                    .FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                StatusMessage =
                    $"Error: {ex.Message}";
            }
            return null;
        }

        public T GetItem(Expression<Func<T, bool>> predicate)
        {
            try
            {
                Init();
                return  connection.Table<T>()
                    .Where(predicate).FirstOrDefault();
            }
            catch (Exception ex)
            {
                StatusMessage =
                   $"Error: {ex.Message}";
            }
            return null;
        }

        public List<T> GetItems()
        {
            try
            {
                Init();
                return connection.Table<T>().ToList();   
            }
            catch (Exception ex)
            {
                StatusMessage =
                   $"Error: {ex.Message}";
            }
            return null;
        }

        public List<T> GetItems(Expression<Func<T, bool>> predicate)
        {
            try
            {
                Init();
                return connection.Table<T>().Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                StatusMessage =
                   $"Error: {ex.Message}";
            }
            return null;
        }

        public void SaveItem(T item)
        {
            int result = 0;
            try
            {
                Init();
                if (item.Id != 0)
                {
                    result =
                        connection.Update(item);  
                    StatusMessage =
                    $"{result} row(s) updated";
                }
                else
                {
                    result = connection.Insert(item);
                    StatusMessage =
                        $"{result} row(s) added";
                }
            }
            catch (Exception ex)
            {
                StatusMessage =
                    $"Error: {ex.Message}";
            }
        }



        //public void SaveItemWithChildren(T item, bool recursive = false)
        //{

        //    //Init();
        //    if (item.Id != 0)
        //    {

        //        connection.UpdateWithChildren(item);                //Fehler: Fügt nciht die kinder hinzu
        //        connection.upd

        //    }
        //    else
        //    {
        //        connection.InsertWithChildren(item, recursive);

        //    }
        //}
        public void SaveItemWithChildren(T item, bool recursive = false)
        {
            connection.InsertWithChildren(item, recursive);
        }

        public void DropTable()
        {
            try
            {
                Init();
                connection.DropTable<T>();
            }
            catch (Exception ex)
            {
                StatusMessage =
                    $"Error: {ex.Message}";
            }
        }

        public List<T> GetItemsWithChildren()
        {
            try
            {
                //Init();
                return connection.GetAllWithChildren<T>().ToList();
            }
            catch (Exception ex)
            {
                StatusMessage =
                   $"Error: {ex.Message}";
            }
            return null;
        }

        //public List<T> GetItemsWithChildren(Expression<Func<T, bool>> predicate)
        //{
        //    try
        //    {
        //        Init();
        //          var a = connection.GetAllWithChildren<T>().ToList();
                


        //        return connection.Table<T>().Where(predicate).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage =
        //           $"Error: {ex.Message}";
        //    }
        //    return null;
        //}


        public void DeleteTable()
        {
            try
            {
                Init();
                connection.DeleteAll<T>();
            }
            catch (Exception ex)
            {
                StatusMessage =
                    $"Error: {ex.Message}";
            }
        }
    }
}
