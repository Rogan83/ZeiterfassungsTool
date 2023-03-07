using ZeiterfassungsTool.MVVM.ViewModels.Admin;
namespace ZeiterfassungsTool.MVVM.Views;

public partial class AdminPageUserList : ContentPage
{
	public AdminPageUserList()
	{
		InitializeComponent();

		BindingContext = new AdminPageUserListModel();
	}

   
}