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

        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Birthday { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }


        public Thickness Margin { get; set; }


        public int WorkingHoursPerWeek { get; set; } = 40;

        public UserSettingsModel()
        {
            var employee = Login.WhoIsLoggedIn[0];

            if (employee.WorkingHoursPerWeek != 0)
                WorkingHoursPerWeek = employee.WorkingHoursPerWeek;

            if (employee.Username != null)
                Username = employee.Username;

            if (employee.Firstname != null)
                Firstname = employee.Firstname;

            if(employee.Lastname != null)
                Lastname = employee.Lastname;

            if (employee.Street != null)
                Street = employee.Street;

            if (employee.PostalCode != null)
                PostalCode = employee.PostalCode;

            if (employee.City != null)  
                City = employee.City;

            if (employee.Country != null)
                Country = employee.Country;

            if (employee.Birthday != null)
                Birthday = employee.Birthday;

            if (employee.EMail != null)
                EMail = employee.EMail;

            if (employee.Password != null) 
                Password = employee.Password;


            Margin = new Thickness(20, 20, 20, 20);
        }


        public ICommand BackButton =>
          new Command(() =>
          {
              Shell.Current.GoToAsync("UserSettings/UserPage");
          });

        public ICommand SaveWorkingHours =>
          new Command( async () =>
          {
              if (Password == null || Password == String.Empty)
              {
                  await App.Current.MainPage.DisplayAlert("Fehler", "Das Passwort Feld darf nicht leer sein!", "OK");
                  return;
              }

              var currentEmployee = Login.WhoIsLoggedIn[0];

              App.EmployeeRepo.DeleteItem(currentEmployee);

              currentEmployee.WorkingHoursPerWeek = WorkingHoursPerWeek;
              currentEmployee.Username = Username;
              currentEmployee.Firstname= Firstname;
              currentEmployee.Lastname= Lastname;
              currentEmployee.Street = Street;
              currentEmployee.PostalCode = PostalCode;
              currentEmployee.City = City;
              currentEmployee.Country = Country;
              currentEmployee.Birthday = Birthday;
              currentEmployee.EMail = EMail;
              currentEmployee.Password = Password;

              App.EmployeeRepo.SaveItemWithChildren(currentEmployee);

              await App.Current.MainPage.DisplayAlert("", "Die Einstellungen wurden gespeichert.", "OK");

              await Shell.Current.GoToAsync("UserSettings/UserPage");

          });
    }
}
