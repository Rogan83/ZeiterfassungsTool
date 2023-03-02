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
        //base.OnAppearing();
       
        //model.LoadTimeTrackingData();  // Lädt die Daten von Employee.Timetracking in die Eigenschaft "Timetracking", wenn diese Seite dargestellt wird.
    }
    // Ich weiß sonst nicht, wie ich das mit dem Display Alert umsetzen soll, wenn ich dies in der Model Klasse machen soll
    private async void OnButton_DeleteUser(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Benutzer löschen?", "Möchten Sie diesen Benutzer wirklich löschen?", "JA", "NEIN");
        Console.WriteLine("antwort: " + answer);

        if (answer == true)
        {
            App.EmployeeRepo.DeleteItem(model.Employee);
            await Shell.Current.GoToAsync("AdminPageUserManagement/AdminPage");
        }
    }
}