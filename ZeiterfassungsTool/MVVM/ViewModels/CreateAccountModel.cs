using Microsoft.Maui.Controls;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CreateAccountModel
    {
        public CreateAccountModel()
        {
            CheckIfOneAccountExist();
            LbUsername = $"Sie sind mit dem Benutzername {Login.WhoIsLoggedIn[0].Username} angemeldet.";
        }

        #region Properties

        private bool isFirstAccount = false;

        public string Info { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DebugMessage { get; set; }

        public bool rbUser { get; set; } = true;            //Soll standardmäßig ausgewählt sein
        public bool rbManagement { get; set; }
        public bool rbAdmin { get; set; }

        public string LbUsername { get; set; }

        public bool rbsIsVisible { get; set; } = true;

        #endregion


        #region Commands

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("CreateAccount/LoginPage");
           });

        //public ICommand GoToLoginPage =>
        //    new Command( () =>
        //    {
        //        Shell.Current.GoToAsync(nameof(LoginPage));
        //    });

        //public ICommand GoToCreateAccountSite =>
        //    new Command(() =>
        //    {
        //        Shell.Current.GoToAsync(nameof(CreateAccount));
                
        //    });

        public ICommand DeleteTable =>
            new Command(() =>
            {
                App.EmployeeRepo.DeleteTable();
            });

        public ICommand ToRegister =>
            new Command((vslRegisterElements) =>
            {
                if (ExistsThisUser())           //Untersucht, ob der Benutzername schon vergeben wurde
                {
                    Info = "Dieser Benutzername ist bereits schon vergeben.";
                    return;
                }
                if (Username != null && Password != null) 
                {
                    if (isFirstAccount)
                    {
                        App.EmployeeRepo.SaveItem(new Employee() { Username = this.Username, Password = this.Password, Role = Role.Admin });
                    }
                    else
                    {
                        rbsIsVisible = true;

                        Role role = Role.User;

                        if (rbUser)
                        {
                            role = Role.User;
                        }
                        else if (rbManagement)
                        {
                            if (Login.WhoIsLoggedIn[0].Role == Role.Admin)            //Wenn ein Admin angemeldet ist, dann kann der Account angelegt werden
                            {
                                role = Role.Management;
                            }
                            else
                            {
                                Info = "Sie müssen mit einen Konto angemeldet sein, welches über Adminrechte verfügt.";
                                return;
                            }
                        }
                        else if (rbAdmin)
                        {
                            if (Login.WhoIsLoggedIn[0].Role == Role.Admin)            //Wenn ein Admin angemeldet ist, dann kann der Account angelegt werden
                            {
                                role = Role.Admin;
                            }
                            else
                            {
                                Info = "Sie müssen mit einen Konto angemeldet sein, welches über Adminrechte verfügt.";
                                return;
                            }
                        }

                        rbsIsVisible = false;
                        App.EmployeeRepo.SaveItem(new Employee() { Username = this.Username, Password = this.Password, Role = role });
                        App.Current.MainPage.DisplayAlert("Account angelegt", $"Account mit dem Namen {Username} wurde erfolgreich angelegt", "Ok");
                        Shell.Current.GoToAsync("CreateAccount/LoginPage");

                        Info = $"{role} Account wurde erfolgreich angelegt";
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

        #endregion

        #region Methods

        private bool ExistsThisUser()
        {
            var user = App.EmployeeRepo.GetItem(x => x.Username == this.Username);

            if (user == null)
                return false;

            return true;
        }
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
                else if (child is RadioButton)
                {
                    var btn = (RadioButton)child;
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
            Button btn = (Button)vsl.FindByName("btnBackToMenu");//(Button)vsl.Children[vsl.Children.Count - 1];

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
                rbsIsVisible = false;
                isFirstAccount = true;
            }
            else
            {
                Info = $"{text}";
                rbsIsVisible = true;
                isFirstAccount = false;
            }
        }
        #endregion

    }
}
