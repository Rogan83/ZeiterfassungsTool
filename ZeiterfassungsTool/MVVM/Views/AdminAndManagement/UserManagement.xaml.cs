using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class UserManagement : ContentPage
{
    UserManagementModel model = new UserManagementModel();
    public UserManagement()
    {
        InitializeComponent();
        model = new UserManagementModel();
        BindingContext = model;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();

        model.SelectFirstTime();

        //List<Timetracking> timetracking = new();
        List<MySQLModels.Timetracking> timetracking = new();
        if (model.Employee != null ) 
        { 
            //SQLite
            //timetracking = model.Employee.Timetracking.OrderBy(x => x.StartTime).ThenBy(x => x.EndTime).ToList();             //Sortiert die Liste aufsteigend, (komischerweise aber nicht den 2. Wert)
            timetracking = model.Timetracking.OrderBy(x => x.StartTime).ThenBy(x => x.EndTime).ToList();
        }
        else
        {
            Debug.WriteLine("Fehler: Employee ist null");
        }
        //var timetracking = from t in model.Employee.Timetracking                                                              //SQL ähnliche Syntax
        //                   orderby t.StartTime, t.EndTime
        //                   select t;

        //model.Timetracking = new ObservableCollection<Timetracking>(timetracking);
        model.Timetracking = new ObservableCollection<MySQLModels.Timetracking>(timetracking);

        var employee = Login.WhoIsLoggedIn[0];

        if (employee != null)
        {
            //if (employee.Role != Enumerations.Role.Admin)
            if (employee.RoleId != 3)
            {
                imgBtnDeleteTime.IsEnabled = false;
                imgBtnDeleteUser.IsEnabled = false;
                imgBtnUpdate.IsEnabled = false;
            }
            else
            {
                imgBtnDeleteTime.IsEnabled = true;
                imgBtnDeleteUser.IsEnabled = true;
                imgBtnUpdate.IsEnabled = true;
            }
                
        }

        dpEndDate.Format = dpStartDate.Format = "dd/MM/yyyy";  
        tpStartTime.Format = tpEndTime.Format = "HH:mm";   
    }

    #region ButtonEvents
    
    //private async void OnButton_DeleteUser(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Benutzer löschen?", "Möchten Sie diesen Benutzer wirklich löschen?", "JA", "NEIN");
    //    Console.WriteLine("antwort: " + answer);

    //    if (answer == true)
    //    {
    //        App.EmployeeRepo.DeleteItem(model.Employee);
    //        await Shell.Current.GoToAsync("AdminPageUserManagement/AdminPage");
    //    }
    //}

    //private async void OnButton_Update(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Aktualisieren", "Möchten Sie wirklich diesen Datensatz aktualisieren?", "JA", "NEIN");

    //    if (answer == true )
    //    {
    //        model.Update();
    //    }
    //}

    //private async void OnButton_AddTime(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Hinzufügen", "Möchten Sie wirklich diesen Datensatz hinzufügen?", "JA", "NEIN");

    //    if (answer == true)
    //    {
    //        model.AddTime();
    //    }
    //}

    //private async void OnButtonDeleteTime(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Datensatz löschen", "Möchten Sie wirklich diesen Datensatz löschen?", "JA", "NEIN");

    //    if (answer == true)
    //    {
    //        model.DeleteTime();
    //    }
    //}

    private void worktime_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        EntrySubject.IsVisible = true;
    }

    private void holiday_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        EntrySubject.IsVisible = false;
    }

    private void ill_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        EntrySubject.IsVisible = false;
    }
    #endregion
}