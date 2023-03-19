using Syncfusion.Maui.Core;
using System.Diagnostics;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.MVVM.ViewModels.User;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.User;

public partial class UserSettings : ContentPage
{
	public UserSettings()
	{
		InitializeComponent();

		BindingContext = new UserSettingsModel();
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
}