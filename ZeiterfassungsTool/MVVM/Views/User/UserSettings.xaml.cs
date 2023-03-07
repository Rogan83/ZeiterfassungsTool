using ZeiterfassungsTool.MVVM.ViewModels.User;

namespace ZeiterfassungsTool.MVVM.Views.User;

public partial class UserSettings : ContentPage
{
	public UserSettings()
	{
		InitializeComponent();

		BindingContext = new UserSettingsModel();
	}
}