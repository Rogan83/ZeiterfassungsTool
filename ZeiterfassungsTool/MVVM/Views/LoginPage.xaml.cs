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
		if (loggedInEmployee.Id != 0)
		{
			ShowLoginControls(true);
		}
		else
		{
			ShowLoginControls(false);
		}

		void ShowLoginControls(bool isVisible)
		{
			//Die Steuerelemente, die man sehen muss, wenn man eingeloggt ist bzw. die man nicht sehen darf, wenn man ausgeloggt ist.
            btnForwardToContent.IsVisible = isVisible;
            btnLogout.IsVisible = isVisible;
			// Die Steuerelemente, die man sehen muss, wenn man ausgeloggt ist bwz. die man nicht sehen darf, wenn man eingeloggt ist
            entryUsername.IsVisible = !isVisible;
            entryPassword.IsVisible = !isVisible;
            btnLogin.IsVisible = !isVisible;
        }
	}
}