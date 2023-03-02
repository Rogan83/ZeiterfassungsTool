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

        public DateTime DateAnDTimeStartTime { get; set; }
        
        public DateTime DateStartTime { get; set; } = DateTime.Now;
        public TimeSpan TimeStartTime { get; set; } = TimeSpan.Zero;


        public DateTime DateAnDTimeEndTime { get; set; }
        public DateTime DateEndTime { get; set; } = DateTime.Now;

        public TimeSpan TimeEndTime { get; set; } = TimeSpan.Zero;


        public string Username { get; set; }

        public Timetracking SelectedItem { get; set; }

        public AdminPageUserManagementModel()
        {
            // Timetracking = Employee.Timetracking;

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

        public ICommand SaveDateAndTime =>
           new Command(() =>
           {
               DateAnDTimeStartTime = new DateTime(DateStartTime.Year, DateStartTime.Month, DateStartTime.Day, TimeStartTime.Hours, TimeStartTime.Minutes, TimeStartTime.Seconds);
               DateAnDTimeEndTime = new DateTime(DateEndTime.Year, DateEndTime.Month, DateEndTime.Day, TimeEndTime.Hours, TimeEndTime.Minutes, TimeEndTime.Seconds);
               Console.WriteLine("Datum: " + DateAnDTimeStartTime);
               Console.WriteLine("Datum von einen User: " + Employee.Timetracking[0].StartTime.ToString());

               try
               {
                    Console.WriteLine("selected item start:" + SelectedItem.StartTime); // SelectedItem = DateAnDTime;
                    Console.WriteLine("selected item end:" + SelectedItem.EndTime); // SelectedItem = DateAnDTime;
               }
               catch    (Exception ex)
               {

                   Console.WriteLine("no item selected.");
               }


               var employees = App.EmployeeRepo.GetItemsWithChildren();


               for (int i = 0; i < employees.Count; i++)
               {
                   if (employees[i].Username == Employee.Username)
                   {
                       for (int j = 0; j < employees[i].Timetracking.Count; j++)
                       {
                           if ((employees[i].Timetracking[j].StartTime == SelectedItem.StartTime) && (employees[i].Timetracking[j].EndTime == SelectedItem.EndTime))
                           {
                               //UpdateWithchildren funktioniert nicht, deswegen muss der User gelöscht und in modifizierter Form wieder hinzugefügt werden
                               App.EmployeeRepo.DeleteItem(employees[i]); 

                               var employee = employees[i];
                               employee.Timetracking[j].StartTime = DateAnDTimeStartTime;
                               employee.Timetracking[j].EndTime = DateAnDTimeEndTime;

                               App.EmployeeRepo.SaveItemWithChildren(employee);

                               Employee = employee;

                               return;              
                           }
                       }
                   }

               }

               //SelectedItem.StartTime = DateAnDTimeStartTime;
               //SelectedItem.EndTime = DateAnDTimeEndTime;
              
           });

        //public void LoadTimeTrackingData()
        //{
        //    Timetracking = Employee.Timetracking;
        //    Username = Employee.Username;
        //}

    }
}
