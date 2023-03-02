using PropertyChanged;
using System;
using System.Collections.Generic;
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
        public Employee Employee { get; set; }


        //public List<Timetracking> Timetracking { get; set; }

        public DateTime DateAndTimeStartTime { get; set; }
        
        public DateTime DateStartTime { get; set; } = DateTime.Now;
        public TimeSpan TimeStartTime { get; set; } = TimeSpan.Zero;


        public DateTime DateAndTimeEndTime { get; set; }
        public DateTime DateEndTime { get; set; } = DateTime.Now;

        public TimeSpan TimeEndTime { get; set; } = TimeSpan.Zero;


        public string Username { get; set; }

        public Timetracking SelectedTime { get; set; }

        public string EntrySubject { get; set; }


        public AdminPageUserManagementModel()
        {
            
        }




        public ICommand BackToMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("AdminPageUserManagement/StartPage");
           });

        public ICommand Back =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("AdminPageUserManagement/AdminPage");
           });

        public ICommand Logout =>
           new Command(() =>
           {
               SaveLoginStatus.WhoIsLoggedIn = new List<Employee>() { new Employee() };             // Zurücksetzen
               Shell.Current.GoToAsync("AdminPage/StartPage");
           });

        public ICommand Update =>
           new Command(() =>
           {
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
                        Employee.Timetracking[i].Subject = EntrySubject;

                        App.EmployeeRepo.SaveItemWithChildren(Employee);

                        //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
                        var temp = Employee;
                        Employee = null;
                        Employee = temp;

                        return;              
                    }
                }
                   
           });

        public ICommand AddTime =>
           new Command(() =>
           {
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
               Employee.Timetracking.Add(new Timetracking { EmployeeID = Employee.Id, IsCurrentlyWorking = false, StartTime = DateAndTimeStartTime, EndTime = DateAndTimeEndTime
                    , WorkingTime = DateAndTimeEndTime - DateAndTimeStartTime, Subject = EntrySubject, Id = id});   

               App.EmployeeRepo.SaveItemWithChildren(Employee);

               //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
               var temp = Employee;
               Employee = null;
               Employee = temp;

               EntrySubject = string.Empty;
           });

        public ICommand DeleteTime =>
           new Command(() =>
           {
               App.EmployeeRepo.DeleteItem(Employee);

               Employee.Timetracking.Remove(SelectedTime);

               App.EmployeeRepo.SaveItemWithChildren(Employee);

               //Wirkt unnötig, aber ansonsten aktualisiert sich nicht das UI
               var temp = Employee;
               Employee = null;
               Employee = temp;
           });

        /// <summary>
        /// Wählt das erste Item von der Timetracking Liste aus, wenn vorhanden
        /// </summary>
        public void SelectFirstTime()
        {
            //Timetracking = Employee.Timetracking;
            //Username = Employee.Username;

            if (Employee.Timetracking != null)
                SelectedTime = Employee.Timetracking.FirstOrDefault();          //Das schon ein Element ausgewählt wird beim Start
        }

    }
}
