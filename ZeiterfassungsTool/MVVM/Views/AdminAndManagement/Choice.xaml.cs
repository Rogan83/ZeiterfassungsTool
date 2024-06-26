using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class Choice : ContentPage
{
	public Choice()
	{
		InitializeComponent();

		BindingContext = new ChoiceModel();
	}

    protected override void OnAppearing()
	{
		base.OnAppearing();

		var loggedInAccount = Login.WhoIsLoggedIn[0];
		if (loggedInAccount != null)
		{
			//if (loggedInAccount.Role != Enumerations.Role.Admin)
			if (loggedInAccount.RoleId != 3)
			{
				btnShowAllAccounts.IsVisible = false;
			}
			else
			{
				btnShowAllAccounts.IsVisible = true;
			}
		}
	}

}