using System.Collections.ObjectModel;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.ViewModels.AdminAndManagement;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.AdminAndManagement;

public partial class AdminAndManagementManagement : ContentPage
{
    AdminAndManagementManagementModel model = new();
    public AdminAndManagementManagement()
    {
        InitializeComponent();

        model = new AdminAndManagementManagementModel();
        BindingContext = model;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //Holt alle Mitarbeiter in die Liste (inkl. Admins und Geschäftsleitung). Es soll aber NICHT der Account erscheinen, mit dem man sich eingeloggt hat, damit dieser auch nicht gelöscht werden kann

        //model.Employees = model.GetAccountsWithoutLogginInAccount();
        model.Employees = model.GetAllAccounts();
        
    }
}