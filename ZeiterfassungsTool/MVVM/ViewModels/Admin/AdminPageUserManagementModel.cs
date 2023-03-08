using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    [AddINotifyPropertyChangedInterface]
    [QueryProperty(nameof(Employee), "employee")]
    public class AdminPageUserManagementModel
    {

        #region Properties
        public Employee Employee { get; set; }
        public ObservableCollection<Timetracking> Timetracking { get; set; } = new ObservableCollection<Timetracking>();

        public DateTime DateAndTimeStartTime { get; set; }
        
        public DateTime DateStartTime { get; set; } = DateTime.Now;
        public TimeSpan TimeStartTime { get; set; } = TimeSpan.Zero;


        public DateTime DateAndTimeEndTime { get; set; }
        public DateTime DateEndTime { get; set; } = DateTime.Now;

        public TimeSpan TimeEndTime { get; set; } = TimeSpan.Zero;


        public string Username { get; set; }

        public Timetracking SelectedTime { get; set; }

        public string EntrySubject { get; set; }

        public bool IsVisibleEntrySubject { get; set; } = true;

        public Color BackgroundColorFrameWork { get; set; } = Colors.LightSkyBlue;        
        public Color BackgroundColorFrameHoliday { get; set; } = Colors.LightGreen;        
        public Color BackgroundColorFrameIll { get; set; } = Colors.LightPink;        

        public bool rbWorktime { get; set; } = true;
        public bool rbHoliday { get; set; }
        public bool rbIll { get; set; }
        #endregion


        public AdminPageUserManagementModel()
        {
        }

        #region Commands

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("AdminPageUserManagement/AdminPageUserList");
           });

        public ICommand BackToMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("AdminPageUserManagement/StartPage");
           });

        public ICommand Back =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("AdminPageUserManagement/AdminPageUserList");
           });

        public ICommand Logout =>
           new Command(() =>
           {
               SaveLoginStatus.WhoIsLoggedIn = new List<Employee>() { new Employee() };             // Zurücksetzen
               Shell.Current.GoToAsync("AdminPageUserList/StartPage");
           });

        public ICommand Update =>
           new Command( async() =>
           {
               bool answer = await App.Current.MainPage.DisplayAlert("Aktualisieren", "Möchten Sie wirklich diesen Datensatz aktualisieren?", "JA", "NEIN");

               if (answer == false)
               {
                   return;
               }

               DateAndTimeStartTime = new DateTime(DateStartTime.Year, DateStartTime.Month, DateStartTime.Day, TimeStartTime.Hours, TimeStartTime.Minutes, TimeStartTime.Seconds);
               DateAndTimeEndTime = new DateTime(DateEndTime.Year, DateEndTime.Month, DateEndTime.Day, TimeEndTime.Hours, TimeEndTime.Minutes, TimeEndTime.Seconds);

               for (int i = 0; i < Employee.Timetracking.Count; i++)
               {
                   if (Employee.Timetracking[i].Id == SelectedTime.Id)
                   {
                       //UpdateWithchildren funktioniert nicht, deswegen muss der User gelöscht und in modifizierter Form wieder hinzugefügt werden
                       App.EmployeeRepo.DeleteItem(Employee);

                       Employee.Timetracking[i].StartTime = DateAndTimeStartTime;
                       Employee.Timetracking[i].EndTime = DateAndTimeEndTime;
                       Employee.Timetracking[i].Subject = ChooseSubject();

                       App.EmployeeRepo.SaveItemWithChildren(Employee);

                       var timetracking = Employee.Timetracking.OrderBy(x => x.StartTime).ThenBy(x => x.EndTime).ToList();
                       Timetracking = new ObservableCollection<Timetracking>(timetracking);

                       return;
                   }
               }

           });

        public ICommand DeleteUser =>
           new Command(async () =>
           {
               bool answer = await App.Current.MainPage.DisplayAlert("Benutzer löschen?", "Möchten Sie diesen Benutzer wirklich löschen?", "JA", "NEIN");
               Console.WriteLine("antwort: " + answer);

               if (answer == false)
               {
                   return;
               }

               App.EmployeeRepo.DeleteItem(Employee);
               await Shell.Current.GoToAsync("AdminPageUserManagement/AdminPageUserList");
           });

        public ICommand DeleteTime =>
            new Command(async () =>
            {
                bool answer = await App.Current.MainPage.DisplayAlert("Datensatz löschen", "Möchten Sie wirklich diesen Datensatz löschen?", "JA", "NEIN");

                if (answer == false)
                {
                    return;
                }

                App.EmployeeRepo.DeleteItem(Employee);

                Employee.Timetracking.Remove(SelectedTime);

                App.EmployeeRepo.SaveItemWithChildren(Employee);

                var timetracking = Employee.Timetracking.OrderBy(x => x.StartTime).ThenBy(x => x.EndTime).ToList();
                Timetracking = new ObservableCollection<Timetracking>(timetracking);
            });

           #endregion

           #region Methods

           private string ChooseSubject()
           {
                string subject;
                if (rbHoliday)
                {
                    subject = "Urlaub";
                    IsVisibleEntrySubject = false;
                }
                else if (rbIll)
                {
                    subject = "Krank";
                    IsVisibleEntrySubject = false;
                }
                else
                {
                    subject = EntrySubject;
                    IsVisibleEntrySubject = true;
                }
                return subject;
           }

        //public void Update()
        //{
        //    DateAndTimeStartTime = new DateTime(DateStartTime.Year, DateStartTime.Month, DateStartTime.Day, TimeStartTime.Hours, TimeStartTime.Minutes, TimeStartTime.Seconds);
        //    DateAndTimeEndTime = new DateTime(DateEndTime.Year, DateEndTime.Month, DateEndTime.Day, TimeEndTime.Hours, TimeEndTime.Minutes, TimeEndTime.Seconds);

        //    for (int i = 0; i < Employee.Timetracking.Count; i++)
        //    {
        //        if (Employee.Timetracking[i].Id == SelectedTime.Id)
        //        {
        //            //UpdateWithchildren funktioniert nicht, deswegen muss der User gelöscht und in modifizierter Form wieder hinzugefügt werden
        //            App.EmployeeRepo.DeleteItem(Employee);

        //            Employee.Timetracking[i].StartTime = DateAndTimeStartTime;
        //            Employee.Timetracking[i].EndTime = DateAndTimeEndTime;
        //            Employee.Timetracking[i].Subject = ChooseSubject();

        //            App.EmployeeRepo.SaveItemWithChildren(Employee);

        //            //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
        //            var temp = Employee;
        //            Employee = null;
        //            Employee = temp;

        //            return;
        //        }
        //    }
        //}

        public ICommand AddTime =>
           new Command(async () =>
           {
               bool answer = await App.Current.MainPage.DisplayAlert("Hinzufügen", "Möchten Sie wirklich diesen Datensatz hinzufügen?", "JA", "NEIN");

               if (answer == false)
               {
                   return;
               }

               DateAndTimeStartTime = new DateTime(DateStartTime.Year, DateStartTime.Month, DateStartTime.Day, TimeStartTime.Hours, TimeStartTime.Minutes, TimeStartTime.Seconds);
               DateAndTimeEndTime = new DateTime(DateEndTime.Year, DateEndTime.Month, DateEndTime.Day, TimeEndTime.Hours, TimeEndTime.Minutes, TimeEndTime.Seconds);

               App.EmployeeRepo.DeleteItem(Employee);

               int lastElement;
               int id;
               if (Employee.Timetracking.Count > 0)
               {
                   lastElement = Employee.Timetracking.Count - 1;
                   id = Employee.Timetracking[lastElement].Id + 1;
               }
               else
               {
                   id = 0;
               }



               Employee.Timetracking.Add(new Timetracking
               {
                   EmployeeID = Employee.Id,
                   IsCurrentlyWorking = false,
                   StartTime = DateAndTimeStartTime,
                   EndTime = DateAndTimeEndTime,
                   WorkingTime = DateAndTimeEndTime - DateAndTimeStartTime,
                   Subject = ChooseSubject(),
                   Id = id
               });

               App.EmployeeRepo.SaveItemWithChildren(Employee);

               //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
               //var temp = Employee;
               //Employee = null;
               //Employee = temp;
               var timetracking = Employee.Timetracking.OrderBy(x => x.StartTime).ThenBy(x => x.EndTime).ToList();
               Timetracking = new ObservableCollection<Timetracking>(timetracking);

               EntrySubject = string.Empty;
           });

              

        //public void AddTime()
        //{
        //    DateAndTimeStartTime = new DateTime(DateStartTime.Year, DateStartTime.Month, DateStartTime.Day, TimeStartTime.Hours, TimeStartTime.Minutes, TimeStartTime.Seconds);
        //    DateAndTimeEndTime = new DateTime(DateEndTime.Year, DateEndTime.Month, DateEndTime.Day, TimeEndTime.Hours, TimeEndTime.Minutes, TimeEndTime.Seconds);

        //    App.EmployeeRepo.DeleteItem(Employee);

        //    int lastElement;
        //    int id;
        //    if (Employee.Timetracking.Count > 0)
        //    {
        //        lastElement = Employee.Timetracking.Count - 1;
        //        id = Employee.Timetracking[lastElement].Id + 1;
        //    }
        //    else
        //    {
        //        id = 0;
        //    }

            

        //    Employee.Timetracking.Add(new Timetracking
        //    {
        //        EmployeeID = Employee.Id,
        //        IsCurrentlyWorking = false,
        //        StartTime = DateAndTimeStartTime,
        //        EndTime = DateAndTimeEndTime,
        //        WorkingTime = DateAndTimeEndTime - DateAndTimeStartTime,
        //        Subject = ChooseSubject(),    
        //        Id = id
        //    });

        //    App.EmployeeRepo.SaveItemWithChildren(Employee);

        //    //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
        //    var temp = Employee;
        //    Employee = null;
        //    Employee = temp;

        //    EntrySubject = string.Empty;
        //}

        //public ICommand DeleteTime =>
        //   new Command(() =>
        //   {
        //       App.EmployeeRepo.DeleteItem(Employee);

        //       Employee.Timetracking.Remove(SelectedTime);

        //       App.EmployeeRepo.SaveItemWithChildren(Employee);

        //       //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
        //       var temp = Employee;
        //       Employee = null;
        //       Employee = temp;
        //   });

        //public void DeleteTime()
        //{
        //    App.EmployeeRepo.DeleteItem(Employee);

        //    Employee.Timetracking.Remove(SelectedTime);

        //    App.EmployeeRepo.SaveItemWithChildren(Employee);

        //    //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
        //    var temp = Employee;
        //    Employee = null;
        //    Employee = temp;
        //}

        /// <summary>
        /// Wählt das erste Item von der Timetracking Liste aus, wenn vorhanden
        /// </summary>
        public void SelectFirstTime()
        {
            //Timetracking = Employee.Timetracking;
            //Username = Employee.Username;
            if (Employee != null)
            {
                if (Employee.Timetracking != null)
                    SelectedTime = Employee.Timetracking.FirstOrDefault();          //Das schon ein Element ausgewählt wird beim Start
            }
            else
            {
                Console.WriteLine("Es wurde kein Mitarbeiter ausgewählt");
            }
            
        }
        #endregion

    }
}
