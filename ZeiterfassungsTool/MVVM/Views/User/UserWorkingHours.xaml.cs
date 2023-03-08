using ZeiterfassungsTool.MVVM.ViewModels.User;

namespace ZeiterfassungsTool.MVVM.Views.User;

public partial class UserWorkingHours : ContentPage
{
	public UserWorkingHours()
	{
		InitializeComponent();

		BindingContext = new UserWorkingHoursModel();
	}
}