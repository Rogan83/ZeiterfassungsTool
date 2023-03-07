using ZeiterfassungsTool.MVVM.ViewModels.Admin;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class AdminPageChoice : ContentPage
{
	public AdminPageChoice()
	{
		InitializeComponent();

		BindingContext = new AdminPageChoiceModel();
	}
}