using SQLiteDemo.Repositories;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.Repositories;

namespace ZeiterfassungsTool;

public partial class App : Application
{
    //public static Repository Repo { get; private set; }
    public static BaseRepository<Employee> EmployeeRepo { get; set; }
    public static BaseRepository<Timetracking> TimetrackingRepo { get; set; }
    //public App(Repository repo)
    public App(BaseRepository<Employee> employeeRepo, BaseRepository<Timetracking> timetrackingRepo)
    {
        // TODO: Initialize the Repository property with the Repository singleton object
        InitializeComponent();

        MainPage = new AppShell();

        //Repo = repo;
        EmployeeRepo = employeeRepo;
        TimetrackingRepo= timetrackingRepo;
    }
}
