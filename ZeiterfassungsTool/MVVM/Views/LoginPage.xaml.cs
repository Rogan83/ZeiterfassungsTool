using ZeiterfassungsTool.MVVM.ViewModels;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginPageModel();
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

		var loggedInEmployee = Login.WhoIsLoggedIn[0];
		// Wenn jemand eingeloggt ist...
		if (loggedInEmployee.Id != 0 )
		{
            VisibiltyBtnAndEntry(false, true);
        }
		else
		{
            VisibiltyBtnAndEntry(true, false);
        }

		void VisibiltyBtnAndEntry(bool isVisibleLogin, bool isVisibleLogout)
		{
            btnForwardToContent.IsVisible = isVisibleLogout;
            btnLogout.IsVisible = isVisibleLogout;

            entryUsername.IsVisible = isVisibleLogin;
            entryPassword.IsVisible = isVisibleLogin;
            btnLogin.IsVisible = isVisibleLogin;
        }
	}
}