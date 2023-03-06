using ZeiterfassungsTool.MVVM.ViewModels;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class CreateAccount : ContentPage
{
	public CreateAccount()
	{
		InitializeComponent();

		BindingContext = new CreateAccountModel();
	}

    //protected override async void OnNavigating(ShellNavigatingEventArgs args)
    //{
    //    base.OnNavigating(args);

    //    ShellNavigatingDeferral token = args.GetDeferral();

    //    var result = await DisplayActionSheet("Navigate?", "Cancel", "Yes", "No");
    //    if (result != "Yes")
    //    {
    //        args.Cancel();
    //    }
    //    token.Complete();
    //}
}