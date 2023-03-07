using ZeiterfassungsTool.MVVM.ViewModels.Admin;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class AdminPageScheduler : ContentPage
{
	public AdminPageScheduler()
	{
		InitializeComponent();

		BindingContext = new AdminPageSchedulerModel();
	}
}