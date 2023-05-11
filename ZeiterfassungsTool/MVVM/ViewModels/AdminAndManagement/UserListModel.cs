using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    [AddINotifyPropertyChangedInterface]
    public class UserListModel
    {
        //public Employee SelectedEmployee { get; set; }
        public MySQLModels.Employee SelectedEmployee { get; set; }

        //public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<MySQLModels.Employee> Employees { get; set; } = new ObservableCollection<MySQLModels.Employee>();

        public List<MySQLModels.Employee> employees  = new();

        public UserListModel()
        {
            //SQLite
            //var employees = App.EmployeeRepo.GetItemsWithChildren();
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
               Login.WhoIsLoggedIn = new List<MySQLModels.Employee>() { new MySQLModels.Employee() };
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

        public async Task<ObservableCollection<MySQLModels.Employee>> GetAllUser()
        {
            //SQLite
            //List<Employee> employees = App.EmployeeRepo.GetItemsWithChildren();
            //MySQL
            List<MySQLModels.Employee> employees = await MySQLMethods.GetAllAccounts();

            var user = from e in employees where e.RoleId == 1 select e;            // 1 = user, 2 = Management, 3 = admin

            //return new ObservableCollection<Employee>(employees);
            return new ObservableCollection<MySQLModels.Employee>(user);
        }
    }
}
