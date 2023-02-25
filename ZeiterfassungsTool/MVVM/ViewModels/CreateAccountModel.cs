using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.MVVM.Views;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    public class CreateAccountModel
    {
        public CreateAccountModel()
        {
            CheckIfOneAccountExist();
        }

        public string Info { get; set; }

        public ICommand GoToCreateAccountSite =>
            new Command(() =>
            {
                Shell.Current.GoToAsync(nameof(CreateAccount));
            });

        public ICommand DeleteTable =>
            new Command(() =>
            {
                App.EmployeeRepo.DeleteTable();
            });

        public void CheckIfOneAccountExist()
        {
            int count = App.EmployeeRepo.GetItems().Count();
            string text = "Bitte geben Sie ihr Benutzername und Passwort zum Registrieren ein.";
            if (count == 0)
            {
                //Noch kein Account in der Datenbank vorhanden. Der erste Account wird somit ein Admin Account werden.
                Info = $"Es existiert noch kein Account. Somit wird jetzt ein Admin Account erstellt. {text}";
            }
            else
            {
                Info = $"{text} Es existieren {count} Accounts";
            }

            
        }
    }
}
