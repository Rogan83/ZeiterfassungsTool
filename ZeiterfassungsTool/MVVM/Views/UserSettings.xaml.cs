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
        //HideButton(btnDeletePassword, Role.User);
        HideButton(btnDeletePassword, 1);
        //HideButton(btnResetPassword, Role.User);
        HideButton(btnResetPassword, 1);
        //HideInputField(inputNewPassword, Role.Admin, Role.Management);
        HideInputField(inputNewPassword, 3, 2);
        //HideInputField(inputVacationDays, Role.User);
        HideInputField(inputVacationDays, 1);
        //HideInputField(inputWorkTime, Role.User);
        HideInputField(inputWorkTime, 1);
        //HidePicker(picker, Role.User);
        HidePicker(picker, 1);

        PreventDamageOwnAccount(btnDeletePassword.BackgroundColor, Colors.Grey);
    }

    //private static void HideButton(object sender, Role role)
    private static void HideButton(object sender, int roleId)
    {
        var btn = sender as Button;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            //if (currentUser.Role == role)
            if (currentUser.RoleId == roleId)
            {
                btn.IsVisible = false;
            }
            else
            {
                btn.IsVisible = true;
            }
        }
    }
    //private static void HideInputField(object sender, Role role)
    private static void HideInputField(object sender, int roleId)
    {
        var element = sender as SfTextInputLayout;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            //if (currentUser.Role == role)
            if (currentUser.RoleId == roleId)
            {
                element.IsVisible = false;
            }
            else
            {
                element.IsVisible = true;
            }
        }
    }
    //private static void HideInputField(object sender, Role role, Role role2)
    private static void HideInputField(object sender, int role, int role2)
    {
        var element = sender as SfTextInputLayout;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            //if (currentUser.Role == role || currentUser.Role == role2)
            if (currentUser.RoleId == role || currentUser.RoleId == role2)
            {
                element.IsVisible = false;
            }
            else
            {
                element.IsVisible = true;
            }
        }
    }
    //private static void HidePicker(object sender, Role role)
    private static void HidePicker(object sender, int role)
    {
        var element = sender as Picker;

        var currentUser = Login.WhoIsLoggedIn[0];
        if (currentUser != null)
        {
            //if (currentUser.Role == role)
            if (currentUser.RoleId == role)
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