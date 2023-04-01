using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class SchedulerModel
    {
        public ObservableCollection<SchedulerAppointment> SchedulerEvents { get; set; }

        public SchedulerModel()
        {
            var employees = App.EmployeeRepo.GetItemsWithChildren();

            this.SchedulerEvents = new ObservableCollection<SchedulerAppointment>();

            foreach (var employee in employees)
            {
                foreach (var timetracking in employee.Timetracking)
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

                    SchedulerEvents.Add(new SchedulerAppointment { StartTime = timetracking.StartTime, EndTime = timetracking.EndTime, Subject = concateUsernameAndSubject, Background = background});
                }
            }
        }


        public ICommand BackButton =>
          new Command(() =>
          {
              Shell.Current.GoToAsync("Scheduler/Choice");
          });
    }
}
