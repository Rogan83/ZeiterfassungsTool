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

            //if (employee.Password != null) 
            //    Password = employee.Password;


            Margin = new Thickness(20, 5, 20, 5);
        }


        public ICommand BackButton =>
          new Command(() =>
          {
              Shell.Current.GoToAsync("UserSettings/UserPage");
          });

        public ICommand SaveChangingData =>
          new Command( async () =>
          {
              //var salt = DateTime.Now.ToString();
              //var hashedPW = Hash.HashPassword($"{Password}{salt}");          // Das Passwort mit dem Salt in einen Hash Wert umwandeln (Der Salt Wert ändert das gehashte PW nochmals ab, weil z.B. ein Passwort "1234" immer den gleichen Wert als Hash ergibt. So könnte man daraus schließen, dass ein gleicher Hash Wert zum gleichen Passwort gehört. Da nun zusätzlich noch ein Salt Wert hinzugefügt wird, welcher bei jeden User anders ist, ist auch das Passwort bei jeden User anders, selbst wenn User A das selbe PW hat wie User B 
              var hashedPW = Hash.HashPasswordScrypt(Password);

              var user = App.EmployeeRepo.GetItems().Find(name => name.Username == Username);
              var currentEmployee = Login.WhoIsLoggedIn[0];

              //Wurde ein Nutzer mit dem Benutzernamen gefunden, dann darf dieser Benutzername nicht vergeben werden
              if (user != null && currentEmployee.Username != Username)
              {
                  await App.Current.MainPage.DisplayAlert("Fehler", "Dieser Benutzername wurde schon vergeben", "OK");
                  return;
              }


              if (Password == null || Password == String.Empty)
              {
                  await App.Current.MainPage.DisplayAlert("Fehler", "Das Passwort Feld darf nicht leer sein!", "OK");
                  return;
              }

              if (Password == "0" && currentEmployee.IsResetPassword)
              {
                  await App.Current.MainPage.DisplayAlert("Fehler", "Sie müssen ein neues Passwort vergeben!", "OK");
                  return;
              }

              

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
              currentEmployee.Password = hashedPW;
              //currentEmployee.Salt = salt;
              

              currentEmployee.IsResetPassword = false;

              App.EmployeeRepo.SaveItemWithChildren(currentEmployee);

              await App.Current.MainPage.DisplayAlert("", "Die Einstellungen wurden gespeichert.", "OK");

              await Shell.Current.GoToAsync("UserSettings/UserPage");

          });
    }
}
