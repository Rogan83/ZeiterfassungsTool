using Syncfusion.Maui.Core;
using System.Diagnostics;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.MVVM.ViewModels.User;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.User;


public partial class UserSettings : ContentPage
{
    UserSettingsModel model;
    Color color = new();
    public UserSettings()
	{
		InitializeComponent();

		model = new UserSettingsModel();

        BindingContext = model;
	}

    

    protected override void OnAppearing()
    {
        base.OnAppearing();
        HideButton(btnDeletePassword, Role.User);
        HideButton(btnResetPassword, Role.User);
        HideInputField(inputNewPassword, Role.Admin, Role.Management);
        HideInputField(inputVacationDays, Role.User);
        HideInputField(inputWorkTime, Role.User);
        HidePicker(picker, Role.User);

        PreventDamageOwnAccount(btnDeletePassword.BackgroundColor, Colors.Grey);
    }

    private static void HideButton(object sender, Role role)
    {
        var btn = sender as Button;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            if (currentUser.Role == role)
            {
                btn.IsVisible = false;
            }
            else
            {
                btn.IsVisible = true;
            }
        }
    }
    private static void HideInputField(object sender, Role role)
    {
        var element = sender as SfTextInputLayout;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            if (currentUser.Role == role)
            {
                element.IsVisible = false;
            }
            else
            {
                element.IsVisible = true;
            }
        }
    }
    private static void HideInputField(object sender, Role role, Role role2)
    {
        var element = sender as SfTextInputLayout;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            if (currentUser.Role == role || currentUser.Role == role2)
            {
                element.IsVisible = false;
            }
            else
            {
                element.IsVisible = true;
            }
        }
    }
    private static void HidePicker(object sender, Role role)
    {
        var element = sender as Picker;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            if (currentUser.Role == role)
            {
                element.IsVisible = false;
            }
            else
            {
                element.IsVisible = true;
            }
        }
    }
    /// <summary>
    /// Verhindert, dass weder der eigene Adminaccount gelöscht - noch die Adminrechte (versehentlich) entzogen werden können
    /// </summary>
    /// <param name="bgColorAktivate"></param>
    /// <param name="bgColorDeactivate"></param>
    private void PreventDamageOwnAccount(Color bgColorAktivate, Color bgColorDeactivate)
    {
        var loggedInAccount = Login.WhoIsLoggedIn[0];
        

        if (model.Employee.Username == loggedInAccount.Username)
        {
            btnDeletePassword.IsEnabled= false;
            btnDeletePassword.BackgroundColor = bgColorDeactivate;

            picker.IsVisible= false;
        }
        else
        {
            btnDeletePassword.IsEnabled = true;
            picker.IsVisible = true;
        }

    }
}