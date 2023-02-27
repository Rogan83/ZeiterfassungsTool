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

        public ICommand GoToLoginPage =>
            new Command(() =>
            {
                Shell.Current.GoToAsync(nameof(LoginPage));
            });

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
            new Command((vslRegisterElements) =>
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
                    Info = "Account erfolgreich angelegt";
                    HideControls(vslRegisterElements);
                    ShowBackButton(vslRegisterElements);
                }
                else
                {
                    Info = "Der Benutzername und das Passwort Feld darf nicht leer sein!";
                }
                
            });
        public ICommand BackToMainMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("..");
           });
        private void HideControls(object vslRegisterElements)
        {
            var elements = (VerticalStackLayout)vslRegisterElements;

            for (int i = 1; i < elements.Children.Count - 1; i++)
            {

                var child = ((VerticalStackLayout)vslRegisterElements).Children[i];
                if (child is Entry)
                {
                    var entry = (Entry)child;
                    entry.IsVisible = false;
                }
                else if (child is Button)
                {
                    var btn = (Button)child;
                    btn.IsVisible = false;
                }
                else
                {
                    var lbl = (Label)child;
                    lbl.IsVisible= false;
                }
            }
        }

        private void ShowBackButton(object vslRegisterElements)
        {
            var vsl = (VerticalStackLayout)vslRegisterElements;
            Button btn = (Button)vsl.Children[vsl.Children.Count - 1];

            if (btn != null )
            {
                btn.IsVisible = true;   
            }
        }

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
