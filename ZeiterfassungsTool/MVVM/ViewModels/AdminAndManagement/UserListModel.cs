using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class UserListModel
    {
        public Employee SelectedEmployee { get; set; }

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public UserListModel()
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
        #region ICommands
        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("UserList/Choice");
           });

        public ICommand BackToMainMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("UserList/LoginPage");
           });

        public ICommand Logout =>
           new Command(() =>
           {
               Login.WhoIsLoggedIn = new List<Employee>() { new Employee() };
               Shell.Current.GoToAsync("UserList/StartPage");
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
                  await Shell.Current.GoToAsync(nameof(UserManagement), Parameter); 
              }
          });
        #endregion 
    }
}
