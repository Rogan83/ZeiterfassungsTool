using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;

namespace ZeiterfassungsTool;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
        

    }

    private async void OnAddNewEmployee(object sender, EventArgs e)
    {
        await App.EmployeeRepo.SaveItem(new Employee { Firstname = newData.Text });
            statusMessage.Text = App.EmployeeRepo.StatusMessage;
    }

    private async void OnGetAllEmployee(object sender, EventArgs e)
    {
        statusMessage.Text = "";

        List<Employee> employees = await App.EmployeeRepo.GetItems();
        TimetrackingList.ItemsSource = null;
        EmployeeList.ItemsSource = employees;
    }

    private async void OnAddNewTime(object sender, EventArgs e)
    {
        await App.TimetrackingRepo.SaveItem(new Timetracking { WorkingHours = newData.Text, EmployeeID = 111});
        statusMessage.Text = App.TimetrackingRepo.StatusMessage;
    }

    private async void OnGetAllTimes(object sender, EventArgs e)
    {
        statusMessage.Text = "";

        List<Timetracking> timetracking = await App.TimetrackingRepo.GetItems();
        EmployeeList.ItemsSource = null;
        TimetrackingList.ItemsSource = timetracking;
    }

    private async void OnDropTables(object sender, EventArgs e)
    {
        await App.EmployeeRepo.DropTable();
        await App.TimetrackingRepo.DropTable();
    }

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
}

