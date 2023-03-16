using ZeiterfassungsTool.MVVM.ViewModels.Admin;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class Scheduler : ContentPage
{
	public Scheduler()
	{
		InitializeComponent();

		BindingContext = new PageSchedulerModel();
	}
}