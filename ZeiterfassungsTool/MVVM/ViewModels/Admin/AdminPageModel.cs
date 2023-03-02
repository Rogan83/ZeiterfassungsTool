using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class AdminPageModel
    {
        

        public Employee SelectedEmployee { get; set; }

        public ObservableCollection<Employee> Employees { get; set; }
            = new ObservableCollection<Employee>();

        public AdminPageModel()
        {
            var employees = App.EmployeeRepo.GetItemsWithChildren();
            foreach (var employee in employees)
            {
                if (employee.Role == Role.User)             // Es sollen nur die Benutzer hinzugefügt werden, da nur diese die Arbeitszeiten mit der Start und Stopfunktion einfügen können
                {
                    Employees.Add(employee);
                }
            }
        }

        public ICommand BackToMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("AdminPage/StartPage");
           });

        public ICommand Logout =>
           new Command(() =>
           {
               SaveLoginStatus.WhoIsLoggedIn = new List<Employee>() { new Employee() };
               Shell.Current.GoToAsync("AdminPage/StartPage");
           });

        public ICommand EmployeeClickedCommand =>
          new Command(async () =>
          {
              var employee = SelectedEmployee;


              var Parameter = new Dictionary<string, object>
              {
                    {"employee", employee }
              };



              if (employee != null)
              {
                  //Shell.Current.GoToAsync(nameof(AdminPageUserManagement), employeeParameter);            // Geht nicht...
                  //Shell.Current.GoToAsync($"AdminPage/AdminPageUserManagement", employeeParameter);            // Geht nicht...
                  await Shell.Current.GoToAsync($"AdminPageUserManagement", Parameter);            // Geht nicht...

              }



          });
    }
}
