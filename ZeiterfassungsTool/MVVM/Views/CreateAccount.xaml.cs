using ZeiterfassungsTool.MVVM.ViewModels;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class CreateAccount : ContentPage
{
    CreateAccountModel createAccount;
    public CreateAccount()
    {
        InitializeComponent();
        createAccount = new CreateAccountModel();
        BindingContext = createAccount;
    }

    protected async override void OnAppearing()
    { 
        base.OnAppearing(); 
        await createAccount.CheckIfOneAccountExist();
    }
    //protected override bool OnBackButtonPressed()
    //{
    //    Shell.Current.GoToAsync(nameof(LoginPage));
    //    return true;
    //}


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
    