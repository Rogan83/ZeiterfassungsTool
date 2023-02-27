using ZeiterfassungsTool.MVVM.ViewModels;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();

        BindingContext = new LoginPageModel();
    }
}