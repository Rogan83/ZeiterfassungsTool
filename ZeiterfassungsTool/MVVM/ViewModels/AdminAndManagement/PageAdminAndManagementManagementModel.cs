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
    public class PageAdminAndManagementManagementModel
    {

        #region Properties

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public Employee SelectedEmployee { get; set; }


        #endregion

        #region Commands
        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(PageChoice));
           });

        public ICommand DeleteAccount =>
          new Command(async () =>
          {
              if (SelectedEmployee == null)
              {
                  await App.Current.MainPage.DisplayAlert("", "Es wurde kein Acccount selektiert", "OK");
                  return;
              } 

              bool answer = await App.Current.MainPage.DisplayAlert("Benutzer löschen?", $"Möchten Sie den Benutzer {SelectedEmployee.Username} wirklich löschen?", "JA", "NEIN");
             
              if (answer == false)
              {
                  return;
              }

              if (SelectedEmployee != null) 
              {
                  App.EmployeeRepo.DeleteItem(SelectedEmployee);
                  Employees = GetAccountsWithoutLogginInAccount();
                  // Wähle, nachdem der selectierte User gelöscht wurde, den ersten von der Liste und selektiere ihn
                  SelectedEmployee = Employees.FirstOrDefault();
              }
          });

        public ICommand ResetPassword =>
           new Command(async() =>
           {

               if (SelectedEmployee == null)
               {
                   await App.Current.MainPage.DisplayAlert("", "Es wurde kein Acccount selektiert", "OK");
                   return;
               }

               bool answer = await App.Current.MainPage.DisplayAlert("Passwort zurücksetzen?", $"Möchten Sie das Passwort vom Benutzer {SelectedEmployee.Username} wirklich zurücksetzen?", "JA", "NEIN");

               if (answer == false)
               {
                   return;
               }
              
               App.EmployeeRepo.DeleteItem(SelectedEmployee);
               var password = "0";

               var salt = DateTime.Now.ToString();
               var hashedPW = Hash.HashPassword($"{password}{salt}");          // Das Passwort mit dem Salt in einen Hash Wert umwandeln (Der Salt Wert ändert das gehashte PW nochmals ab, weil z.B. ein Passwort "1234" immer den gleichen Wert als Hash ergibt. So könnte man daraus schließen, dass ein gleicher Hash Wert zum gleichen Passwort gehört. Da nun zusätzlich noch ein Salt Wert hinzugefügt wird, welcher bei jeden User anders ist, ist auch das Passwort bei jeden User anders, selbst wenn User A das selbe PW hat wie User B 

               SelectedEmployee.Password = hashedPW;
               SelectedEmployee.Salt = salt;

               SelectedEmployee.IsResetPassword = true;
               App.EmployeeRepo.SaveItemWithChildren(SelectedEmployee);

               await App.Current.MainPage.DisplayAlert("", $"Das Passwort vom Benutzer {SelectedEmployee.Username} lautet nun '0'.", "OK");
           });

        #endregion


        public ObservableCollection<Employee> GetAccountsWithoutLogginInAccount()
        {
            Employee LoggedInAccount = Login.WhoIsLoggedIn[0];
            List<Employee> employees = App.EmployeeRepo.GetItemsWithChildren();
            List<Employee> employeeWithoutLogginInAccount = new();

            //employees.Remove(LoggedInAccount);   //Funktioniert nicht

            foreach (Employee employee in employees)
            {
                if (employee.Username != LoggedInAccount.Username)
                {
                    employeeWithoutLogginInAccount.Add(employee);
                }
            }

            return new ObservableCollection<Employee>(employeeWithoutLogginInAccount);
        }

    }
}
