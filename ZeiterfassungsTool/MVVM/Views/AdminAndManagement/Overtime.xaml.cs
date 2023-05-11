using System.Collections.ObjectModel;
using System.Linq;
using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.MySQLModels;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class Overtime : ContentPage
{
    OvertimeModel overtimeModel;
	public Overtime()
	{
		InitializeComponent();
        overtimeModel = new OvertimeModel();
		BindingContext = overtimeModel;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<Employee> arr = await MySQLMethods.GetAllAccounts();


        var a = new ObservableCollection<MySQLModels.Employee>(arr);

        ObservableCollection<Employee> user = new ();
        foreach (MySQLModels.Employee e in a)
        {
            if (e.RoleId == 1)
                user.Add(e);
        }

        overtimeModel.Employees = user;//a.Where(x => x.RoleId == 1); List<MySQLModels.Employee> xx = a;   

        overtimeModel.CreateEmployeeListForOneMonth();
    }


    private void chooseMonth_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //LblInfo.IsVisible = true;
        EntryMonth.IsVisible = true;
        EntryYear.IsVisible = true;
    }

    private void total_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        //LblInfo.IsVisible = false;
        EntryMonth.IsVisible = false;
        EntryYear.IsVisible = false;
    }
}