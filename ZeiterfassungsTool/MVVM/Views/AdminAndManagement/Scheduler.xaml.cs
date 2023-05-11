using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class Scheduler : ContentPage
{
	SchedulerModel schedulerModel;
	public Scheduler()
	{
		InitializeComponent();
		schedulerModel = new SchedulerModel();
		BindingContext = schedulerModel;
	}

    protected async override void OnAppearing()
	{
		base.OnAppearing();

		schedulerModel.Employees = await MySQLMethods.GetAllAccounts();

        schedulerModel.SchedulerEvents = new ObservableCollection<SchedulerAppointment>();

        foreach (var employee in schedulerModel.Employees)
        {
            var timetrackingList = await MySQLMethods.GetTimetracking(employee.Id);

            foreach (var timetracking in timetrackingList)
            {
                var subject = timetracking.Subject;
                if (subject == null) { subject = ""; }             // Es darf NIEMALS ein Feld NULL sein, welches dem SchedulerAppointment zugewiesen wird, sonst schmiert die Anwendung ab 

                Color background;
                if (subject == "Krank")
                    background = Colors.DarkRed;
                else if (subject == "Urlaub")
                    background = Colors.DarkGreen;
                else
                    background = Colors.DarkBlue;

                string concateUsernameAndSubject;
                if (subject != "")
                    concateUsernameAndSubject = $"{employee.Username} - {subject}";
                else
                    concateUsernameAndSubject = employee.Username;

                schedulerModel.SchedulerEvents.Add(new SchedulerAppointment { StartTime = timetracking.StartTime, EndTime = timetracking.EndTime, Subject = concateUsernameAndSubject, Background = background });
            }
        }

    }
}