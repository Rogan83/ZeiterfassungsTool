using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;

namespace ZeiterfassungsTool;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
        
    }
    #region Methoden Zum Testen
    //private void OnAddNewEmployee(object sender, EventArgs e)
    //{
    //    Employee employee = new Employee { Firstname = newEmployee.Text };
    //    employee.Timetracking = new List<Timetracking>()
    //    {
    //       new Timetracking { WorkingHours = newTime.Text },
    //       new Timetracking { WorkingHours = "9" },
    //       new Timetracking { WorkingHours = "11" },
    //    };


    //    App.EmployeeRepo.SaveItemWithChildren(employee);
    //    statusMessage.Text = App.EmployeeRepo.StatusMessage;
    //}

    //private void OnGetAllEmployee(object sender, EventArgs e)
    //{
    //    statusMessage.Text = "";

    //    List<Employee> employees = App.EmployeeRepo.GetItemsWithChildren();
    //    //TimetrackingList.ItemsSource = null;
    //    EmployeeList.ItemsSource = employees;
    //}

    //private void OnAddNewTime(object sender, EventArgs e)
    //{
    //    App.TimetrackingRepo.SaveItem(new Timetracking { WorkingHours = newTime.Text, EmployeeID = 111 });
    //    statusMessage.Text = App.TimetrackingRepo.StatusMessage;
    //}

    //private void OnGetAllTimes(object sender, EventArgs e)
    //{
    //    statusMessage.Text = "";

    //    List<Timetracking> timetracking = App.TimetrackingRepo.GetItems();
    //    EmployeeList.ItemsSource = null;
    //    //TimetrackingList.ItemsSource = timetracking;
    //}

    private void OnDropTables(object sender, EventArgs e)
    {
        App.EmployeeRepo.DropTable();
        App.TimetrackingRepo.DropTable();
    }
    #endregion
    #region AsynMethoden Zum Testen
    //private async void OnToRegister(object sender, EventArgs e) 
    //{


    //    int counts = await App.Repo.CountEmployeeAccounts();
    //    if (counts > 0)
    //    {
    //        await Navigation.PushAsync(new CreateUserAccount());
    //    }
    //    else
    //    {
    //        await Navigation.PushAsync(new CreateFirstAdminAccount());
    //    }
    //}

    //private async void Init()
    //{
    //    int counts = await App.Repo.CountEmployeeAccounts();
    //    if (counts > 0)
    //    {
    //        await Navigation.PushAsync(new CreateUserAccount());
    //    }
    //    else
    //    {
    //        await Navigation.PushAsync(new CreateFirstAdminAccount());
    //    }
    //}


    //private async void OnAddNewEmployee(object sender, EventArgs e)
    //{
    //    await App.Repo.AddNewEmployee(newEmployee.Text);
    //    statusMessage.Text = App.Repo.StatusMessage;
    //}

    //private async void OnGetAllEmployee(object sender, EventArgs e)
    //{
    //    statusMessage.Text = "";

    //    List<Employee> employees = await App.Repo.GetAllEmployees();
    //    TimetrackingList.ItemsSource = null;
    //    EmployeeList.ItemsSource = employees;
    //}

    //private async void OnAddTime(object sender, EventArgs e)
    //{
    //    await App.Repo.AddNewTime();
    //    statusMessage.Text = App.Repo.StatusMessage;
    //}

    //private async void OnGetAllTimetrackingData(object sender, EventArgs e)
    //{
    //    statusMessage.Text = "";

    //    List<Timetracking> timetracking = await App.Repo.GetAllTimetracking();
    //    EmployeeList.ItemsSource = null;
    //    TimetrackingList.ItemsSource = timetracking;
    //}

    //private async void OnDebug(object sender, EventArgs e)
    //{
    //    DebugMessage.Text = (await App.Repo.Debug()).ToString();
    //    statusMessage.Text = App.Repo.StatusMessage;
    //}
    #endregion


}

