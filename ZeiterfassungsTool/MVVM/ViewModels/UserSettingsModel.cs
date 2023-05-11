using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.User
{
    [AddINotifyPropertyChangedInterface]
    [QueryProperty(nameof(Employee), "employee")]
    public class UserSettingsModel
    {
        private System.Timers.Timer aTimer;

        //public Employee Employee { get; set; }
        public MySQLModels.Employee Employee { get; set; }

        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Birthday { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }

        public int VacationDays { get; set; }
        
        public List<Role> RoleList { get; set; } = new List<Role>()
        {
            Role.User, 
            Role.Management,
            Role.Admin, 
        };

        public List<int> RoleList2 { get; set; } = new List<int>()
        {
            1,
            2,
            3,
        };

        public Enumerations.Role SelectedRole { get; set; }

        public int SelectedRoleIndex { get; set; }

        public Thickness Margin { get; set; }


        public double WorkingHoursPerWeek { get; set; } = 40;

        public UserSettingsModel()
        {
            //Bevor die Eingabefelder mit den Startwerten initialsiert werden können, muss ein kurzer Augenblick gewartet werden, weil vorher noch nicht die Employee Eigenschaft mit den Wert aktualisert wurde, welche von der "USerPageModel" Klasse übergeben wurde 
            InitTimerForInitialisationInputFields(50, true);
        }

        private void InitTimerForInitialisationInputFields(int duration, bool isActivate)
        {
            aTimer = new System.Timers.Timer(duration);
            aTimer.Elapsed += OnInitInputFields;
            aTimer.AutoReset = true;
            aTimer.Enabled = isActivate;
        }

        private void OnInitInputFields(object source, ElapsedEventArgs e)
        {
            if (Employee == null)
                return;


            if (Employee.WorkingHoursPerWeek != 0)
                WorkingHoursPerWeek = Employee.WorkingHoursPerWeek;

            if (Employee.Username != null)
                Username = Employee.Username;

            if (Employee.Firstname != null)
                Firstname = Employee.Firstname;

            if (Employee.Lastname != null)
                Lastname = Employee.Lastname;

            if (Employee.Street != null)
                Street = Employee.Street;

            if (Employee.PostalCode != null)
                PostalCode = Employee.PostalCode;

            if (Employee.City != null)
                City = Employee.City;

            if (Employee.Country != null)
                Country = Employee.Country;

            Birthday = Employee.Birthday;

            if (Employee.EMail != null)
                EMail = Employee.EMail;

            VacationDays = Employee.VacationDaysPerYear;

            //SelectedRole = Employee.Role;
            SelectedRoleIndex = RoleList2.FindIndex(x => x == Employee.RoleId);

            //if (employee.Password != null) 
            //    Password = employee.Password;
            Margin = new Thickness(20, 5, 20, 5);

            aTimer.Enabled = false;
        }

        private void Back()
        {
            //if (Login.WhoIsLoggedIn[0].Role == Enumerations.Role.User)
            if (Login.WhoIsLoggedIn[0].RoleId == 1)
                Shell.Current.GoToAsync("UserSettings/UserPage");
            else
                Shell.Current.GoToAsync(nameof(AdminAndManagementManagement));
        }

        public ICommand BackButton =>
          new Command(() =>
          {
              Back();
          });
        public ICommand DeleteAccount =>
            new Command(async() =>
            {
                try
                {
                    bool answer = await App.Current.MainPage.DisplayAlert("Benutzer löschen?", $"Möchten Sie den Benutzer {Employee.Username} wirklich löschen?", "JA", "NEIN");

                    if (answer == false)
                    {
                        return;
                    }
                    //SQLite
                    //App.EmployeeRepo.DeleteItem(Employee);

                    //MySQL
                    await MySQLMethods.DeleteAccount(Employee.Id);

                    await App.Current.MainPage.DisplayAlert("", $"Der Account mit dem Benutzernamen '{Employee.Username}' wurde gelöscht", "OK");
                    await Shell.Current.GoToAsync(nameof(Choice));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: " + ex);
                }
            });

        public ICommand ResetPassword =>
            new Command(async() =>
            {
                try
                {
                    bool answer = await App.Current.MainPage.DisplayAlert("Passwort zurücksetzen?", $"Möchten Sie das Passwort vom Benutzer {Employee.Username} wirklich zurücksetzen?", "JA", "NEIN");

                    if (answer == false)
                    {
                        return;
                    }

                    //SQLite
                    //App.EmployeeRepo.DeleteItem(Employee);

                    var password = "0";

                    //var salt = DateTime.Now.ToString();
                    //var hashedPW = Hash.HashPassword($"{password}{salt}");          // Das Passwort mit dem Salt in einen Hash Wert umwandeln (Der Salt Wert ändert das gehashte PW nochmals ab, weil z.B. ein Passwort "1234" immer den gleichen Wert als Hash ergibt. So könnte man daraus schließen, dass ein gleicher Hash Wert zum gleichen Passwort gehört. Da nun zusätzlich noch ein Salt Wert hinzugefügt wird, welcher bei jeden User anders ist, ist auch das Passwort bei jeden User anders, selbst wenn User A das selbe PW hat wie User B 
                    var hashedPW = Hash.HashPasswordScrypt(password);

                    Employee.Password = hashedPW;
                    //SelectedEmployee.Salt = salt;

                    Employee.IsResetPassword = true;

                    //SQLite
                    //App.EmployeeRepo.SaveItemWithChildren(Employee);

                    //MySQL
                    await MySQLMethods.UpdateAccount(Employee);

                    await App.Current.MainPage.DisplayAlert("", $"Das Passwort vom Benutzer {Employee.Username} lautet nun '0'.", "OK");
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error: " + ex);
                }
            });

        public ICommand SaveChangingData =>
          new Command(async () =>
          {
              //var salt = DateTime.Now.ToString();
              //var hashedPW = Hash.HashPassword($"{Password}{salt}");          // Das Passwort mit dem Salt in einen Hash Wert umwandeln (Der Salt Wert ändert das gehashte PW nochmals ab, weil z.B. ein Passwort "1234" immer den gleichen Wert als Hash ergibt. So könnte man daraus schließen, dass ein gleicher Hash Wert zum gleichen Passwort gehört. Da nun zusätzlich noch ein Salt Wert hinzugefügt wird, welcher bei jeden User anders ist, ist auch das Passwort bei jeden User anders, selbst wenn User A das selbe PW hat wie User B 
              string hashedPW = null;
              if (Password != null)
                hashedPW = Hash.HashPasswordScrypt(Password);

              //SQLite
              //var user = App.EmployeeRepo.GetItems().Find(name => name.Username == Username);

              //MySQL
              var user = (await MySQLMethods.GetAllAccounts()).Find(user => user.Username == Username);

              //Wurde ein Nutzer mit dem Benutzernamen gefunden, dann darf dieser Benutzername nicht vergeben werden
              if (user != null && Employee.Username != Username)
              {
                  await App.Current.MainPage.DisplayAlert("Fehler", "Dieser Benutzername wurde schon vergeben", "OK");
                  return;
              }

              //if (Password == null || Password == String.Empty)
              //{
              //    await App.Current.MainPage.DisplayAlert("Fehler", "Das Passwort Feld darf nicht leer sein!", "OK");
              //    return;
              //}

              if (Password == "0" && Employee.IsResetPassword)
              {
                  await App.Current.MainPage.DisplayAlert("Fehler", "Sie müssen ein neues Passwort vergeben!", "OK");
                  return;
              }

              //SQLite
              //App.EmployeeRepo.DeleteItem(Employee);

              //MySQL
              Employee.WorkingHoursPerWeek = WorkingHoursPerWeek;
              Employee.Username = Username;
              Employee.Firstname= Firstname;
              Employee.Lastname= Lastname;
              Employee.Street = Street;
              Employee.PostalCode = PostalCode;
              Employee.City = City;
              Employee.Country = Country;
              Employee.Birthday = Birthday;
              Employee.EMail = EMail;
              Employee.VacationDaysPerYear = VacationDays;
              Employee.RoleId = RoleList2[SelectedRoleIndex];

              if (hashedPW != null)
                  Employee.Password = hashedPW;
              //currentEmployee.Salt = salt;


              Employee.IsResetPassword = false;

              //SQLite
              //App.EmployeeRepo.SaveItemWithChildren(Employee);

              //MySQL
              await MySQLMethods.UpdateAccount(Employee);

              await App.Current.MainPage.DisplayAlert("", "Die Einstellungen wurden gespeichert.", "OK");

              Back();
          });
    }
}
