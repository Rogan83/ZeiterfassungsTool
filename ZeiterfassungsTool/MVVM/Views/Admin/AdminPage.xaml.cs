using ZeiterfassungsTool.MVVM.ViewModels.Admin;
namespace ZeiterfassungsTool.MVVM.Views;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();

		BindingContext= new AdminPageModel();
	}

   
}