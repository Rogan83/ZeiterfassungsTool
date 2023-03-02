using ZeiterfassungsTool.MVVM.ViewModels.User;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();

		BindingContext = new UserPageModel();
	}
}