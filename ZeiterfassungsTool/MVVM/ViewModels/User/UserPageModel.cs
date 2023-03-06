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


namespace ZeiterfassungsTool.MVVM.ViewModels.User
{
    [AddINotifyPropertyChangedInterface]
    public class UserPageModel
    {

        private static System.Timers.Timer aTimer;
        #region Properties

        public bool ShowStartTimer { get; set; }
        public bool ShowStopTimer { get; set; }
        public string DebugText { get; set; }
        public string TimePassed { get; set; }

        public string EntrySubject { get; set; } = string.Empty;
        #endregion


        private bool isDeterminePastTime = false;       //Wurde die vergangene heutige Zeit schon bestimmt?
        private TimeSpan determinePastTime;

        private Employee user;

        public UserPageModel()
        {
            if (SaveLoginStatus.WhoIsLoggedIn.Count > 1)
            {
                DebugText = "Es existieren min. 2 User mit den selben Benutzernamen und Passwort!";
            }

            user = SaveLoginStatus.WhoIsLoggedIn[0];

            index = user.Timetracking.Count - 1;

            if (index >= 0)  //Ist überhaupt ein Datensatz vorhanden?
            {
                ShowStartTimer = !user.Timetracking[index].IsCurrentlyWorking;
                ShowStopTimer = user.Timetracking[index].IsCurrentlyWorking;

                workbegin = user.Timetracking[index].StartTime;
                workend = user.Timetracking[index].EndTime;
            }
            else
            {
                ShowStartTimer = true;
                ShowStopTimer = false;

                workbegin = workend = DateTime.Now;
            }

            InitTimer(100, true);
            //DebugText = user.Timetracking.Count.ToString();
            //user.Timetracking = new List<Timetracking>();
        }

        #region Commands

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("UserPage/StartPage");
           });


        public ICommand BackToMenu =>
            new Command(() =>
            {
                Shell.Current.GoToAsync("UserPage/StartPage");
                //SaveLoginStatus.WhoIsLoggedIn[0] = null;
            });

        public ICommand ForwardToScheduler =>
           new Command(() =>
           {
               //Shell.Current.GoToAsync("UserPageScheduler"); UserPage/UserPageScheduler
               Shell.Current.GoToAsync("UserPage/UserPageScheduler");
           });

        private DateTime workbegin;
        private int index = 0;
        public ICommand StartTimeTracking =>
           new Command(() =>
           {
               user.Timetracking.Add(new Timetracking() { StartTime = DateTime.Now, EndTime = DateTime.Now });

               index = user.Timetracking.Count - 1;

               // Es soll die Zeit gespeichert werden, wann angefangen wird zu Arbeiten
               workbegin = DateTime.Now;

               ShowStartTimer = false;
               ShowStopTimer = true;

               //user.Timetracking[count].Workbegin = DateTime.Now;

               user.Timetracking[index].StartTime = workbegin;
               user.Timetracking[index].IsCurrentlyWorking = true;

               //InitTimer(100, true);

               SaveEmployeeInDataBase();
           });
        private DateTime workend;
        public ICommand StopTimeTracking =>
           new Command(() =>
           {
               // Es soll die Zeit gespeichert werden, wann gestoppt wird zu Arbeiten
               workend = DateTime.Now;

               ShowStartTimer = true;
               ShowStopTimer = false;

               // In Datenbank hinzufügen
               //user.Timetracking.Add(new Timetracking() { Workbegin = workbegin, Workend = workend, WorkingTime = workend - workbegin});
               user.Timetracking[index].EndTime = workend;
               user.Timetracking[index].WorkingTime = workend - workbegin;
               user.Timetracking[index].IsCurrentlyWorking = false;
               user.Timetracking[index].Subject = EntrySubject;

               EntrySubject = string.Empty;

               DebugText = $"Anzahl TimeTracking Datensätze: {user.Timetracking.Count.ToString()}";

               SaveEmployeeInDataBase();
           });
        #endregion
        #region Methods

        private void InitTimer(int duration, bool isActivate)
        {
            aTimer = new System.Timers.Timer(duration);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = isActivate;
        }

        private TimeSpan DeterminePastTime()
        {
           
            // Wenn der Index kleiner als null ist, dann ist noch keine Zeit aufgenommen wurden. Insofern müssen auch keine Zeiten zusammen addiert werden.
            if (index < 0)
            {
                TimePassed = "0h : 0m : 0s";
                return new TimeSpan(0);
            }

            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
            //                  e.SignalTime);
            TimeSpan workingTimeTotal = new();

            //Addiere alle Arbeitszeiten zusammen
            foreach (var timetracking in user.Timetracking)
            {
                if (timetracking.Subject == "Urlaub" || timetracking.Subject == "Krank")
                    continue;

                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

                DateOnly dateInTimetrackingStart = DateOnly.FromDateTime(timetracking.StartTime);
                DateOnly dateInTimetrackingEnd = DateOnly.FromDateTime(timetracking.EndTime);

                if (dateInTimetrackingStart == currentDate && dateInTimetrackingEnd == currentDate)
                {
                    //workingTimeTotal += timetracking.WorkingTime;
                    var difference = timetracking.EndTime - timetracking.StartTime;
                    workingTimeTotal += difference;
                }
                else if (dateInTimetrackingStart == currentDate)
                {
                    TimeOnly timeInTimetrackingStart = TimeOnly.FromDateTime(timetracking.StartTime);
                    TimeOnly endOfDay = new TimeOnly(0, 0);

                    var difference = endOfDay - timeInTimetrackingStart;                // Wenn der Start von der Aufzeichnung am heutigen Datum ist, aber das Ende der Aufzeichnung nicht, dann kann es nur bedeuten, dass über die Nacht hinweg bis zum nächsten Tag gearbeitet wurde. Da aber nur die Zeit vom heutigen Tag angezeigt werden soll, muss die Differenz zwischen 0 Uhr und dem Startzeitpunkt der Aufzeichnung ermittelt werden und zum Schluss dazuaddiert werden.     

                    workingTimeTotal += difference;
                }
            }
            return workingTimeTotal;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            determinePastTime = DeterminePastTime();

            // ... und addiere nach dem Start noch zusätzlich die Zeit, die vergangen ist, wenn die Zeit noch nicht gestoppt wurde. (Was bedeutet, dass noch momentan gearbeitet wird.
            //if (workbegin > workend)

            if (index >= 0)
            {
                if (user.Timetracking[index].IsCurrentlyWorking)
                {
                    var passedTimeSinceStartTimeTracking = DateTime.Now - workbegin;
                    determinePastTime += passedTimeSinceStartTimeTracking;
                }
            }
            else
            {
                TimePassed = "0h : 0m : 0s";
                return;
            }

            //var workingTimeTotal = DateTime.Now - workbegin;
            TimePassed = $"{determinePastTime.Hours.ToString()}h : {determinePastTime.Minutes.ToString()}m : {determinePastTime.Seconds.ToString()}s";

            //TODO:
            //Noch extra hinzufügen, wie lange ich HEUTE gearbeitet habe und nicht in der Summe von allen Arbeitszeiten

        }

        void SaveEmployeeInDataBase()
        {
            App.EmployeeRepo.DeleteItem(user);                  //Updaten funktioniert aus irgendeinen Grund nicht, deswegen musste ich mich mit Löschen und neu hinzufügen behelfen
            App.EmployeeRepo.SaveItemWithChildren(user);        //Speichert irgendwie nicht die children, ka warum

            //var count = App.EmployeeRepo.GetItems().Count;

            List<Employee> a = App.EmployeeRepo.GetItemsWithChildren();
        }

        #endregion
    }
}
