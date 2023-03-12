using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class PageChoice : ContentPage
{
	public PageChoice()
	{
		InitializeComponent();

		BindingContext = new PageChoiceModel();
	}

    protected override void OnAppearing()
	{
		base.OnAppearing();

		var loggedInAccount = Login.WhoIsLoggedIn[0];
		if (loggedInAccount != null)
		{
			if (loggedInAccount.Role != Enumerations.Role.Admin)
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