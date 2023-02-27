using ZeiterfassungsTool.MVVM.ViewModels;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class ManagementPage : ContentPage
{
	public ManagementPage()
	{
		InitializeComponent();

		BindingContext = new ManagementPageModel();
	}
}