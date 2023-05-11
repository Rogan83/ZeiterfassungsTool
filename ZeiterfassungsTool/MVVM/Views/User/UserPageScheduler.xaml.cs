using Syncfusion.Maui.Scheduler;
using ZeiterfassungsTool.MVVM.ViewModels;
using ZeiterfassungsTool.MVVM.ViewModels.User;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class UserPageScheduler : ContentPage
{
    UserPageSchedulerModel userPageModel;
	public UserPageScheduler()
	{
		InitializeComponent();
		userPageModel = new UserPageSchedulerModel();
		BindingContext = userPageModel;
	}

    protected async override void OnAppearing()
	{
		base.OnAppearing();
        var user = Login.WhoIsLoggedIn[0];
        var timetrackingList = await MySQLMethods.GetTimetracking(user.Id);

        foreach (var timetracking in timetrackingList)
        {
            var subject = timetracking.Subject;
            if (subject == null) { subject = ""; }             // Es darf NIEMALS ein Feld NULL sein, welches dem SchedulerAppointment zugewiesen wird, sonst schmiert die Anwendung ab (

            Color background;
            if (subject == "Krank")
                background = Colors.DarkRed;
            else if (subject == "Urlaub")
                background = Colors.DarkGreen;
            else
                background = Colors.DarkBlue;

            userPageModel.SchedulerEvents.Add(new SchedulerAppointment { StartTime = timetracking.StartTime, EndTime = timetracking.EndTime, Subject = subject, Background = background });                // Wenn hier das Subject hinzugefügt wird, dann funktioniert das Programm nicht mehr beim Android Emulator, aber noch bei Windows
        }
    }
}