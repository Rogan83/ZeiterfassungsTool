﻿using PropertyChanged;
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

        public bool ShowStartTimer { get; set; } 
        public bool ShowStopTimer { get; set; }
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

            index = user.Timetracking.Count - 1;

            if (index >= 0)  //Ist überhaupt ein Datensatz vorhanden?
            {
                ShowStartTimer = !user.Timetracking[index].IsCurrentlyWorking;
                ShowStopTimer  = user.Timetracking[index].IsCurrentlyWorking;

                workbegin = user.Timetracking[index].Workbegin;
                workend = user.Timetracking[index].Workend;
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


        public ICommand BackToMenu =>
            new Command(() =>
            {
                Shell.Current.GoToAsync("UserPage/StartPage");
                SaveLoginStatus.WhoIsLoggedIn[0] = null;
            });

        private DateTime workbegin;
        private int index = 0;
        public ICommand StartTimeTracking =>
           new Command(() =>
           {
               user.Timetracking.Add(new Timetracking() { Workbegin = DateTime.Now, Workend = DateTime.Now});

               index = user.Timetracking.Count - 1;

               // Es soll die Zeit gespeichert werden, wann angefangen wird zu Arbeiten
               workbegin = DateTime.Now;

               ShowStartTimer = false;
               ShowStopTimer = true;

               //user.Timetracking[count].Workbegin = DateTime.Now;

               user.Timetracking[index].Workbegin = workbegin;
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
               user.Timetracking[index].Workend = workend;
               user.Timetracking[index].WorkingTime = workend - workbegin;
               user.Timetracking[index].IsCurrentlyWorking = false;

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
            App.EmployeeRepo.DeleteItem(user);                  //Updaten funktioniert aus irgendeinen Grund nicht, deswegen musste ich mich mit Löschen und neu hinzufügen behelfen
            App.EmployeeRepo.SaveItemWithChildren(user);        //Speichert irgendwie nicht die children, ka warum

            //var count = App.EmployeeRepo.GetItems().Count;

            List<Employee> a  = App.EmployeeRepo.GetItemsWithChildren();
                                
        }
        





    }
}
