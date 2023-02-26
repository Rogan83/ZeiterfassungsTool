//using Android.OS;
//using Android.Views;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CreateAccountModel
    {
        public CreateAccountModel()
        {
            CheckIfOneAccountExist();
        }
        private bool isFirstAccount = false;

        public string Info { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public string DebugMessage { get; set; }

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

        public ICommand ToRegister =>
            new Command(() =>
            {
                if (Username != null && Password != null) 
                {
                    if (isFirstAccount)
                    {
                        App.EmployeeRepo.SaveItem(new Employee() { Username = this.Username, Password = this.Password, Role = Role.Admin });
                    }
                    else
                    {
                        App.EmployeeRepo.SaveItem(new Employee() { Username = this.Username, Password = this.Password, Role = Role.User });  
                    }
                    DebugMessage = App.EmployeeRepo.StatusMessage;
                    Info = "Speichern erfolgreich";
                }
                else
                {
                    Info = "Der Benutzername und das Passwort Feld darf nicht leer sein!";
                }
            });


        public void CheckIfOneAccountExist()
        {
            int count = App.EmployeeRepo.GetItems().Count();
            string text = "Bitte geben Sie ihr Benutzername und Passwort zum Registrieren ein.";
            if (count == 0)
            {
                //Noch kein Account in der Datenbank vorhanden. Der erste Account wird somit ein Admin Account werden.
                Info = $"Es existiert noch kein Account. Somit wird jetzt ein Admin Account erstellt. {text}";
                isFirstAccount = true;
            }
            else
            {
                Info = $"{text}";
                isFirstAccount = false;
            }

            
        }
    }
}
