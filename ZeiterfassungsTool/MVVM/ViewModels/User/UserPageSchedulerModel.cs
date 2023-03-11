using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.User
{
    public class UserPageSchedulerModel
    {
        public ObservableCollection<SchedulerAppointment> SchedulerEvents { get; set; }

        public UserPageSchedulerModel()
        {
            var user = Login.WhoIsLoggedIn[0];

            SchedulerEvents = new ObservableCollection<SchedulerAppointment>();

            foreach (var timetracking in user.Timetracking)
            {
                var subject = timetracking.Subject;
                if (subject == null ) { subject = ""; }             // Es darf NIEMALS ein Feld NULL sein, welches dem SchedulerAppointment zugewiesen wird, sonst schmiert die Anwendung ab (

                Color background;
                if (subject == "Krank")
                    background = Colors.DarkRed;
                else if(subject == "Urlaub")
                    background = Colors.DarkGreen;
                else
                    background = Colors.DarkBlue;

                SchedulerEvents.Add(new SchedulerAppointment { StartTime = timetracking.StartTime, EndTime = timetracking.EndTime, Subject = subject, Background = background });                // Wenn hier das Subject hinzugefügt wird, dann funktioniert das Programm nicht mehr beim Android Emulator, aber noch bei Windows
            }

            //this.SchedulerEvents = new ObservableCollection<SchedulerAppointment>
            //{

            //    //new SchedulerAppointment
            //    //{


            //    //    //StartTime = DateTime.Now,                               //new DateTime(2023,02,02,15,0,0),
            //    //    //EndTime = DateTime.Now.AddHours(2)                  //new DateTime(2023,02,02,15,0,0)

            //    //} 

            //};
            //SchedulerEvents.Add(new SchedulerAppointment { StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2) });

        }

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("UserPageScheduler/UserPage");
           });
    }
}
