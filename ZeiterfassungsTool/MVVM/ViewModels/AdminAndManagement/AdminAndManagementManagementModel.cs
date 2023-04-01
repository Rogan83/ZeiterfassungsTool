using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.AdminAndManagement
{
    [AddINotifyPropertyChangedInterface]
    public class AdminAndManagementManagementModel
    {

        #region Properties
        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public Employee SelectedEmployee { get; set; }
        #endregion

        #region Commands
        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(Choice));
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
                  await Shell.Current.GoToAsync($"UserSettings", Parameter);
              }
          });

        #endregion

        #region Methods
        public ObservableCollection<Employee> GetAllAccounts()
        {
            Employee LoggedInAccount = Login.WhoIsLoggedIn[0];
            List<Employee> employees = App.EmployeeRepo.GetItemsWithChildren();
            
            return new ObservableCollection<Employee>(employees);
        }
        #endregion
    }
}
