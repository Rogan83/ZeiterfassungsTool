using System.Collections.ObjectModel;
using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class UserList : ContentPage
{
	UserListModel userListmodel;
	public UserList()
	{
		InitializeComponent();
		userListmodel = new UserListModel();
		BindingContext = userListmodel;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		userListmodel.Employees = await userListmodel.GetAllUser();

        foreach (var e in userListmodel.Employees)
        {
            //if (employee.Role == Role.User)               // Es sollen nur die Benutzer hinzugef�gt werden, da nur diese die Arbeitszeiten mit der Start und Stopfunktion einf�gen k�nnen
            if (e.RoleId == 1)                       // Es sollen nur die Benutzer hinzugef�gt werden, da nur diese die Arbeitszeiten mit der Start und Stopfunktion einf�gen k�nnen
            {
                userListmodel.employees.Add(e);
            }
        }

		//userListmodel.SelectedEmployee = userListmodel.Employees.FirstOrDefault();
    }
}