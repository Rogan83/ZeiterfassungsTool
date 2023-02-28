using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    public class UserPageSchedulerModel
    {
        public ObservableCollection<SchedulerAppointment> SchedulerEvents { get; set; }

        public UserPageSchedulerModel() 
        {
            var user = SaveLoginStatus.WhoIsLoggedIn[0];

            this.SchedulerEvents = new ObservableCollection<SchedulerAppointment>();

            foreach (var timetracking in user.Timetracking)
            {
                SchedulerEvents.Add(new SchedulerAppointment { StartTime = timetracking.Workbegin, EndTime = timetracking.Workend });
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

        public ICommand BackToMainMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("UserPageScheduler/StartPage");
           });
        
        public ICommand BackToTimeTracking =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("UserPageScheduler/UserPage");
           });

        
    }
}
