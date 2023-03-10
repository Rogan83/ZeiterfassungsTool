using ZeiterfassungsTool.MVVM.ViewModels.Admin;
namespace ZeiterfassungsTool.MVVM.Views;

public partial class PageUserList : ContentPage
{
	public PageUserList()
	{
		InitializeComponent();

		BindingContext = new AdminPageUserListModel();
	}

   
}