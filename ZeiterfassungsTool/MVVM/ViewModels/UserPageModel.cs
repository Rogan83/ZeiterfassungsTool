using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.StaticClasses;


namespace ZeiterfassungsTool.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class UserPageModel
    {
        private static System.Timers.Timer aTimer;

        public bool isStartTimeTracking { get; set; } = true;
        public bool isStopTimeTracking { get; set; }
        public string DebugText { get; set; }
        public string TimePassed { get; set; }


        private Employee user;

        public UserPageModel()
        {
            if (SaveLoginStatus.WhoIsLoggedIn.Count > 1)
            {
                DebugText = "Es existieren min. 2 User mit den selben Benutzernamen und Passwort!";
            }

            user = SaveLoginStatus.WhoIsLoggedIn[0];

            //DebugText = user.Timetracking.Count.ToString();
            //user.Timetracking = new List<Timetracking>();
        }


        public ICommand BackToMenu =>
            new Command(() =>
            {
                Shell.Current.GoToAsync("UserPage/StartPage");
                SaveLoginStatus.WhoIsLoggedIn[0] = null;
            });

        private DateTime workbegin = DateTime.Now;
        public ICommand StartTimeTracking =>
           new Command(() =>
           {
               // Es soll die Zeit gespeichert werden, wann angefangen wird zu Arbeiten
               workbegin = DateTime.Now;

               isStartTimeTracking = false;
               isStopTimeTracking = true;

               //user.Timetracking[count].Workbegin = DateTime.Now;


               InitTimer(100, true);

               SaveEmployeeInDataBase();

               //Test

            //   user.Timetracking = new List<Timetracking>
            //{
            //    new Timetracking
            //    {
            //        Workbegin = DateTime.Now.AddDays(30)
            //    },
            //    new Timetracking
            //    {
            //        Workbegin = DateTime.Now.AddDays(15)
            //    },
            //};


           });
        private DateTime workend = DateTime.Now;
        public ICommand StopTimeTracking =>
           new Command(() =>
           {
               // Es soll die Zeit gespeichert werden, wann gestoppt wird zu Arbeiten
               workend = DateTime.Now;

               isStartTimeTracking = true;
               isStopTimeTracking = false;

               // In Datenbank hinzufügen
               user.Timetracking.Add(new Timetracking() { Workbegin = workbegin, Workend = workend, WorkingTime = workend - workbegin});

               DebugText = $"Anzahl TimeTracking Datensätze: {user.Timetracking.Count.ToString()}";

               SaveEmployeeInDataBase();
           });

        private void InitTimer(int duration, bool isActivate)
        {
            aTimer = new System.Timers.Timer(duration);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = isActivate;
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
            //                  e.SignalTime);
            TimeSpan workingTimeTotal = new();

            //Addiere alle Arbeitszeiten zusammen
            foreach (var timetracking in user.Timetracking)
            {
                workingTimeTotal += timetracking.WorkingTime;
            }
            // ... und addiere nach dem Start noch zusätzlich die Zeit, die vergangen ist.
            if(workbegin > workend) 
            {
                var passedTimeSinceStartTimeTracking = DateTime.Now - workbegin;
                workingTimeTotal += passedTimeSinceStartTimeTracking;
            }

            //var workingTimeTotal = DateTime.Now - workbegin;
            TimePassed =  $"{workingTimeTotal.Hours.ToString()} h : {workingTimeTotal.Minutes.ToString()} m : {workingTimeTotal.Seconds.ToString()} s";
        }

        void SaveEmployeeInDataBase()
        {
            App.EmployeeRepo.DeleteItem(user);
            App.EmployeeRepo.SaveItemWithChildren(user);        //Speichert irgendwie nicht die children, ka warum

            //var count = App.EmployeeRepo.GetItems().Count;

            List<Employee> a  = App.EmployeeRepo.GetItemsWithChildren();
                                
        }
        





    }
}
