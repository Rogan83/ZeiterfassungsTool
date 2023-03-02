using SQLiteDemo.Repositories;
using Syncfusion.Maui.Scheduler;
using System.Globalization;
using System.Resources;
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
        Syncfusion.Licensing.SyncfusionLicenseProvider.
            RegisterLicense("Mgo+DSMBaFt/QHRqVVhkVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jS39QdkNgXHpacHRSRA==;Mgo+DSMBPh8sVXJ0S0J+XE9AflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TdERgWXlddXFRRmJeVg==;ORg4AjUWIQA/Gnt2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdiWn9Yc3FQQmBfV0A=;MTExODgzMkAzMjMwMmUzNDJlMzBaMGhNZzhGdEVndHVGditKcHdYaWdDTXluSEM0c09RT0h4S0hpM0RMdjFnPQ==;MTExODgzM0AzMjMwMmUzNDJlMzBTdVRERWdMclFURWpCd0tzMUxCbjJwL3BkQUdKWmFaUVBtUVhjMkU5VU9FPQ==;NRAiBiAaIQQuGjN/V0Z+WE9EaFtKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdUVhWHxedndQQ2RdUUNy;MTExODgzNUAzMjMwMmUzNDJlMzBoclVYS2RDdVU0NnZUZWZjUlJYSWx4SXVOSU9HQ2VNTGxjWFd6aWJFU3pVPQ==;MTExODgzNkAzMjMwMmUzNDJlMzBTREdnUHRWdTZIL1ZDMFpZNG9oTHIrNm1XekJlQU5WMERkYU5aeTJvSHN3PQ==;Mgo+DSMBMAY9C3t2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdiWn9Yc3FQQmFUUEY=;MTExODgzOEAzMjMwMmUzNDJlMzBpRExYd1NLeUFQN3hKNEQyNkd3VFVYOTFCKzRleGxiRXdZcDcwWXlUNHlRPQ==;MTExODgzOUAzMjMwMmUzNDJlMzBFNzFVUW1NZlk5bk1BTlQydmJ0dTRrZ3Yzb2w2bXVZODNOa1pOREdvR0w4PQ==;MTExODg0MEAzMjMwMmUzNDJlMzBoclVYS2RDdVU0NnZUZWZjUlJYSWx4SXVOSU9HQ2VNTGxjWFd6aWJFU3pVPQ==");
        // TODO: Initialize the Repository property with the Repository singleton object
        InitializeComponent();

        MainPage = new AppShell();
        //MainPage = new AdminPage();
        //MainPage = new UserPageScheduler();

        //Repo = repo;
        EmployeeRepo = employeeRepo;
        TimetrackingRepo= timetrackingRepo;


    }
}
