using ZeiterfassungsTool.MVVM.ViewModels.Admin;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class PageScheduler : ContentPage
{
	public PageScheduler()
	{
		InitializeComponent();

		BindingContext = new AdminPageSchedulerModel();
	}
}