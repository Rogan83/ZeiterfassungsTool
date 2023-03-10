using ZeiterfassungsTool.MVVM.ViewModels.Admin;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class PageChoice : ContentPage
{
	public PageChoice()
	{
		InitializeComponent();

		BindingContext = new PageChoiceModel();
	}
}