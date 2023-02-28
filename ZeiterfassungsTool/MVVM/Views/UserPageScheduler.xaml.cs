using ZeiterfassungsTool.MVVM.ViewModels;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class UserPageScheduler : ContentPage
{
	public UserPageScheduler()
	{
		InitializeComponent();

		BindingContext = new UserPageSchedulerModel();
	}
}