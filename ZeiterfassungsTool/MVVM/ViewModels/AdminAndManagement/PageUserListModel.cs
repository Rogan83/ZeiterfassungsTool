using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class PageUserListModel
    {
        

        public Employee SelectedEmployee { get; set; }

        public ObservableCollection<Employee> Employees { get; set; }
            = new ObservableCollection<Employee>();

        public PageUserListModel()
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


        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("PageUserList/PageChoice");
           });



        public ICommand BackToMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("PageUserList/StartPage");
           });

        public ICommand Logout =>
           new Command(() =>
           {
               Login.WhoIsLoggedIn = new List<Employee>() { new Employee() };
               Shell.Current.GoToAsync("PageUserList/StartPage");
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
                  await Shell.Current.GoToAsync($"PageUserManagement", Parameter);            // Geht nicht...

              }



          });
    }
}
