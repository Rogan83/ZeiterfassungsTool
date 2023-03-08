using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.User
{
    public class UserSettingsModel
    {
        public int WorkingHoursPerWeek { get; set; } = 40;

        public UserSettingsModel()
        {
            var employee = SaveLoginStatus.WhoIsLoggedIn[0];

            if (employee.WorkingHoursPerWeek != 0)
                WorkingHoursPerWeek = employee.WorkingHoursPerWeek;

        }


        public ICommand BackButton =>
          new Command(() =>
          {
              Shell.Current.GoToAsync("UserSettings/UserPage");
          });

        public ICommand SaveWorkingHours =>
          new Command( async () =>
          {
              var currentEmployee = SaveLoginStatus.WhoIsLoggedIn[0];

              App.EmployeeRepo.DeleteItem(currentEmployee);

              currentEmployee.WorkingHoursPerWeek = WorkingHoursPerWeek;

              App.EmployeeRepo.SaveItemWithChildren(currentEmployee);

              await App.Current.MainPage.DisplayAlert("Speichern", "Speichern erfolgreich", "OK");

              await Shell.Current.GoToAsync("UserSettings/UserPage");

          });
    }
}
