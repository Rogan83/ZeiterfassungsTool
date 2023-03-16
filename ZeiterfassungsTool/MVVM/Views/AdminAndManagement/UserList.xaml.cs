using ZeiterfassungsTool.MVVM.ViewModels.Admin;
namespace ZeiterfassungsTool.MVVM.Views;

public partial class UserList : ContentPage
{
	public UserList()
	{
		InitializeComponent();

		BindingContext = new PageUserListModel();
	}

   
}