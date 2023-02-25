using ZeiterfassungsTool.MVVM.ViewModels;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class CreateAccount : ContentPage
{
	public CreateAccount()
	{
		InitializeComponent();

		BindingContext = new CreateAccountModel();
	}
}