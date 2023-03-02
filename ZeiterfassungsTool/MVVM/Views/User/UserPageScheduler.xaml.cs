using ZeiterfassungsTool.MVVM.ViewModels;
using ZeiterfassungsTool.MVVM.ViewModels.User;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class UserPageScheduler : ContentPage
{
	public UserPageScheduler()
	{
		InitializeComponent();

		BindingContext = new UserPageSchedulerModel();
	}
}