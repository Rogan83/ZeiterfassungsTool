using System.Collections.ObjectModel;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.ViewModels.Admin;


namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class AdminPageUserManagement : ContentPage
{
    AdminPageUserManagementModel model = new AdminPageUserManagementModel();
    public AdminPageUserManagement()
    {
        InitializeComponent();
        model = new AdminPageUserManagementModel();
        BindingContext = model;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        model.SelectFirstTime();

        var timetracking = model.Employee.Timetracking.OrderBy(x => x.StartTime).ThenBy(x => x.EndTime).ToList();             //Sortiert die Liste aufsteigend, (komischerweise aber nicht den 2. Wert)
        //var timetracking = from t in model.Employee.Timetracking                                                              //SQL �hnliche Syntax
        //                   orderby t.StartTime, t.EndTime
        //                   select t;

        model.Timetracking = new ObservableCollection<Timetracking>(timetracking);
    }

    #region ButtonEvents
    
    //private async void OnButton_DeleteUser(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Benutzer l�schen?", "M�chten Sie diesen Benutzer wirklich l�schen?", "JA", "NEIN");
    //    Console.WriteLine("antwort: " + answer);

    //    if (answer == true)
    //    {
    //        App.EmployeeRepo.DeleteItem(model.Employee);
    //        await Shell.Current.GoToAsync("AdminPageUserManagement/AdminPage");
    //    }
    //}

    //private async void OnButton_Update(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Aktualisieren", "M�chten Sie wirklich diesen Datensatz aktualisieren?", "JA", "NEIN");

    //    if (answer == true )
    //    {
    //        model.Update();
    //    }
    //}

    //private async void OnButton_AddTime(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Hinzuf�gen", "M�chten Sie wirklich diesen Datensatz hinzuf�gen?", "JA", "NEIN");

    //    if (answer == true)
    //    {
    //        model.AddTime();
    //    }
    //}

    //private async void OnButtonDeleteTime(object sender, EventArgs e)
    //{
    //    bool answer = await DisplayAlert("Datensatz l�schen", "M�chten Sie wirklich diesen Datensatz l�schen?", "JA", "NEIN");

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