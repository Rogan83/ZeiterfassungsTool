using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.StaticClasses;


namespace ZeiterfassungsTool.MVVM.ViewModels.User
{
    [AddINotifyPropertyChangedInterface]
    public class UserPageModel
    {
        private static System.Timers.Timer Timer1;
        public DateTime workbegin;
        public DateTime workend;

        public List<MySQLModels.Timetracking> Timetracking { get; set; }

        #region Properties

        public bool ShowStartTimer { get; set; }
        public bool ShowStopTimer { get; set; }
        public string DebugText { get; set; }
        public string TimePassed { get; set; }

        public string EntrySubject { get; set; } = string.Empty;
        #endregion
        public int index = 0;

        private TimeSpan determinePastTime;

        //private Employee user;
        private MySQLModels.Employee user;

        public UserPageModel()
        {
            if (Login.WhoIsLoggedIn.Count > 1)
            {
                DebugText = "Es existieren min. 2 User mit den selben Benutzernamen und Passwort!";
            }

            user = Login.WhoIsLoggedIn[0];

            //index = user.Timetracking.Count - 1;

            InitTimer(100, true);
            //DebugText = user.Timetracking.Count.ToString();
            //user.Timetracking = new List<Timetracking>();
        }

        #region Commands

        public ICommand BackButton =>
           new Command(() =>
           {
               //Shell.Current.GoToAsync(nameof(LoginPage));
               Timer1.Enabled = false;
               Shell.Current.GoToAsync("UserPage/LoginPage");
           });

        public ICommand Settings =>
           new Command(async() =>
           {
               //Shell.Current.GoToAsync("UserPage/UserSettings");
               var employee = Login.WhoIsLoggedIn[0];

               var Parameter = new Dictionary<string, object>
               {
                    {"employee", employee }
               };

               if (employee != null)
               {
                   await Shell.Current.GoToAsync($"UserSettings", Parameter);      
               }
               else
               {
                   Debug.WriteLine("Es ist niemand eingeloggt bzw. es wurde der statischen Eigenschaft 'WhoIsLoggedIn nichts zugewiesen'");
               }
           });

        public ICommand ForwardToWorkingHoursPage =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("UserPage/UserWorkingHours");
           });

        //public ICommand BackToMenu =>
        //    new Command(() =>
        //    {
        //        Shell.Current.GoToAsync("UserPage/LoginPage");
        //        //SaveLoginStatus.WhoIsLoggedIn[0] = null;
        //    });

        public ICommand ForwardToScheduler =>
           new Command(() =>
           {
               //Shell.Current.GoToAsync("UserPageScheduler"); UserPage/UserPageScheduler
               Shell.Current.GoToAsync("UserPage/UserPageScheduler");
           });
        
        public ICommand StartTimeTracking =>
           new Command(() =>
           {
               if (Timetracking.Count > 0)
               {
                   index = Timetracking.Count;
               }
               else
               {
                   index = 0;
               }

               //user.Timetracking.Add(new Timetracking() { StartTime = DateTime.Now, EndTime = DateTime.Now });
               Timetracking.Add(new MySQLModels.Timetracking() { StartTime = DateTime.Now, EndTime = DateTime.Now });

               //index = user.Timetracking.Count - 1;

               // Es soll die Zeit gespeichert werden, wann angefangen wird zu Arbeiten
               workbegin = DateTime.Now;

               ShowStartTimer = false;
               ShowStopTimer = true;

               //user.Timetracking[count].Workbegin = DateTime.Now;

               //user.Timetracking[index].StartTime = workbegin;
               Timetracking[index].StartTime = workbegin;
               Timetracking[index].Typeid = 1;
               Timetracking[index].EmployeeId = user.Id;

               //InitTimer(100, true);

               SaveTimetrackingInDatabase();
           });
        
        public ICommand StopTimeTracking =>
           new Command(async() =>
           {
               Timetracking = await MySQLMethods.GetTimetracking(user.Id);

               // Es soll die Zeit gespeichert werden, wann gestoppt wird zu Arbeiten
               workend = DateTime.Now;

               ShowStartTimer = true;
               ShowStopTimer = false;

               // In Datenbank hinzufügen
               //user.Timetracking[index].EndTime = workend;
               Timetracking[index].EndTime = workend;
               //user.Timetracking[index].Subject = EntrySubject;
               Timetracking[index].Subject = EntrySubject;

               EntrySubject = string.Empty;

               //DebugText = $"Anzahl TimeTracking Datensätze: {user.Timetracking.Count.ToString()}";
               DebugText = $"Anzahl TimeTracking Datensätze: {Timetracking.Count.ToString()}";

               UpdateTimetrackingInDatabase();
           });
        #endregion
        #region Methods

        public void InitTimer(int duration, bool isActivate)
        {
            Timer1 = new System.Timers.Timer(duration);
            Timer1.Elapsed += OnTimedEvent;
            Timer1.AutoReset = true;
            Timer1.Enabled = isActivate;
        }
        

        private async Task<TimeSpan> DeterminePastTime()
        {
            var timetrackingList = await MySQLMethods.GetTimetracking(user.Id);
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
            //foreach (var timetracking in user.Timetracking)
            foreach (var timetracking in timetrackingList)
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
                else if (dateInTimetrackingEnd == currentDate)
                {
                    TimeOnly timeInTimetrackingEnd = TimeOnly.FromDateTime(timetracking.EndTime);
                    TimeOnly startOfDay = new TimeOnly(0, 0);

                    var difference = timeInTimetrackingEnd - startOfDay;

                    workingTimeTotal += difference;
                }
            }
            return workingTimeTotal;
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            determinePastTime = await DeterminePastTime();

            // ... und addiere nach dem Start noch zusätzlich die Zeit, die vergangen ist, wenn die Zeit noch nicht gestoppt wurde. (Was bedeutet, dass noch momentan gearbeitet wird.
            if (workbegin >= workend)
            {
                var passedTimeSinceStartTimeTracking = DateTime.Now - workbegin;
                determinePastTime += passedTimeSinceStartTimeTracking;
            }

            //var workingTimeTotal = DateTime.Now - workbegin;
            TimePassed = $"{determinePastTime.Hours.ToString()}h : {determinePastTime.Minutes.ToString()}m : {determinePastTime.Seconds.ToString()}s";
        }

       

        async void SaveTimetrackingInDatabase()
        {
            //SQLite
            //App.EmployeeRepo.DeleteItem(user);                  //Updaten funktioniert aus irgendeinen Grund nicht, deswegen musste ich mich mit Löschen und neu hinzufügen behelfen
            //App.EmployeeRepo.SaveItemWithChildren(user);        //Speichert irgendwie nicht die children, ka warum

            //MySQL
            await MySQLMethods.AddTime(Timetracking[index]);
        }

        async void UpdateTimetrackingInDatabase()
        {
            var id = Timetracking[index].Id;
            await MySQLMethods.UpdateTimetracking(Timetracking[index]);
        }

        #endregion
    }
}
