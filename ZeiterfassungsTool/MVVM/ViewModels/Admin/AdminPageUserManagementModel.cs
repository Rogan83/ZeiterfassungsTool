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


        public List<Timetracking> Timetracking { get; set; }



        public string Username { get; set; }

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



        public void LoadTimeTrackingData()
        {
            Timetracking = Employee.Timetracking;
            Username = Employee.Username;
        }

    }
}
