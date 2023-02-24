using Microsoft.Maui.Controls;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Models;
//using static Android.App.DownloadManager;


namespace ZeiterfassungsTool.Repositories
{
    //public class Repository
    //{
    //    string _dbPath;

    //    public string StatusMessage { get; set; }

    //    // TODO: Add variable for the SQLite connection
    //    public SQLiteAsyncConnection conn;

    //    public Repository(string dbPath)
    //    {
    //        _dbPath = dbPath;

    //    }

    //    private async Task Init()
    //    {
    //        // TODO: Add code to initialize the repository

    //        if (conn != null)
    //            return;

    //        conn = new SQLiteAsyncConnection(_dbPath);
    //        //await conn.ExecuteAsync("drop Table Employee;");
    //        await conn.CreateTableAsync<Employee>();

    //        // Todo Erstelle Table Timetracking mit einen Foreign Key
    //        //await conn.ExecuteAsync($"drop Table Timetracking;");
    //        //await conn.CreateTableAsync<Timetracking>();

    //        //string addForeignKeyquery = $"ALTER TABLE Timetracking ADD FOREIGN KEY (employeeID) References employee(id);";  //funktioniert nicht, weil bei SQLite durch die Alter Methode kein Fremdschlüssel hinzugefügt werden kann

    //        string query = $"create table if not exists timetracking (id integer primary key autoincrement, workbegin DateTime, workend DateTime, WorkingHours varchar(255), employeeid int, foreign key (employeeid) references employee(id));";
    //        int result = await conn.ExecuteAsync(query);
    //        StatusMessage = string.Format("{0} rows are affected added", result);
    //    }


    //    public async Task AddNewEmployee(string firstname)
    //    {
    //        int result = 0;
    //        try
    //        {
    //            await Init();
    //            // basic validation to ensure a name was entered
    //            if (string.IsNullOrEmpty(firstname))
    //                throw new Exception("Valid name required");

    //            // TODO: Insert the new person into the database
    //            result = await conn.InsertAsync(new Employee { Firstname = firstname });

    //            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, firstname);
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = string.Format("Failed to add {0}. Error: {1}", firstname, ex.Message);
    //        }

    //    }



    //    public async Task<List<Employee>> GetAllEmployees()
    //    {
    //        try
    //        {
    //            await Init();

    //            return await conn.Table<Employee>().ToListAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
    //        }

    //        return new List<Employee>();
    //    }


    //    public async Task AddNewTime()
    //    {
    //        int result = 0;
    //        try
    //        {
    //            await Init();
    //            result = await conn.InsertAsync(new Timetracking { Workbegin = DateTime.Now, EmployeeID = 7 });
    //            StatusMessage = string.Format("{0} record(s) added )", result);
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = string.Format("Failed to add. Error: {0}", ex.Message);
    //        }
    //    }

    //    public async Task<List<Timetracking>> GetAllTimetracking()
    //    {
    //        try
    //        {
    //            await Init();

    //            return await conn.Table<Timetracking>().ToListAsync();
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
    //        }

    //        return new List<Timetracking>();
    //    }

    //    public async Task<int> CountEmployeeAccounts()
    //    {
    //        try
    //        {
    //            await Init();

    //            //string query = $"Select Count(*) from Employee";
    //            var result = await conn.Table<Employee>().CountAsync();   // await ist an der Stelle sehr wichtig, ansonsten kommt ein Ergebnis mit den Datentyp "Task<int>" heraus, was ein Hinweis darauf ist, dass das Ergebnis noch nicht ermittelt wurde, da asynchron gearbeitet wird. Es soll also mit "await" solange gewartet werden, bis das Ergebnis ermittelt wurde.

    //            return result;

    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
    //            return 0;
    //        }
    //    }

    //    public async Task<int> Debug()
    //    {
    //        //Ermittel die anzahl der Accounts von den Mitarbeitern
    //        try
    //        {
    //            await Init();

    //            //string query = $"Select Count(*) from Employee";
    //            var result = await conn.Table<Employee>().CountAsync();   // await ist an der Stelle sehr wichtig, ansonsten kommt ein Ergebnis mit den Datentyp "Task<int>" heraus, was ein Hinweis darauf ist, dass das Ergebnis noch nicht ermittelt wurde, da asynchron gearbeitet wird. Es soll also mit "await" solange gewartet werden, bis das Ergebnis ermittelt wurde.

    //            return result;

    //        }
    //        catch (Exception ex)
    //        {
    //            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
    //            return 0;
    //        }

    //    }
    //}
}
