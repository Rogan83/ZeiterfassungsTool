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
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Core;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CreateAccountModel
    {
        public CreateAccountModel()
        {
            CheckIfOneAccountExist();
            //LbUsername = $"Sie sind mit dem Benutzername {Login.WhoIsLoggedIn[0].Username} angemeldet.";
        }

        #region Properties


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


        public bool UsernameAlreadyExists { get; set; }

        public Color UsernameColor { get; set; } = Color.FromArgb("#E6EEF9");


        private bool isFirstAccount = false;

        public string Info { get; set; }

        public string DebugMessage { get; set; }

        public bool rbUser { get; set; } = true;            //Soll standardmäßig ausgewählt sein
        public bool rbManagement { get; set; }
        public bool rbAdmin { get; set; }

        public string LbUsername { get; set; }

        public bool rbsIsVisible { get; set; } = true;



        #endregion

        string alertTextAdmin = "Sie müssen mit einen Konto angemeldet sein, welches über Adminrechte verfügt.";


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


        //var salt = DateTime.Now.ToString();
        //var hashedPW = Hash.HashPassword($"{Password}{salt}");          // Das Passwort mit dem Salt in einen Hash Wert umwandeln (Der Salt Wert ändert das gehashte PW nochmals ab, weil z.B. ein Passwort "1234" immer den gleichen Wert als Hash ergibt. So könnte man daraus schließen, dass ein gleicher Hash Wert zum gleichen Passwort gehört. Da nun zusätzlich noch ein Salt Wert hinzugefügt wird, welcher bei jeden User anders ist, ist auch das Passwort bei jeden User anders, selbst wenn User A das selbe PW hat wie User B 

        public ICommand ToRegister =>
            new Command((vslRegisterElements) =>
            {
                var hashedPW = Hash.HashPasswordScrypt(Password);

                if (ExistsThisUser())           //Untersucht, ob der Benutzername schon vergeben wurde
                {
                    App.Current.MainPage.DisplayAlert("","Dieser Benutzername ist bereits schon vergeben.","OK");
                    UsernameAlreadyExists = true;
                    UsernameColor = Colors.Red;
                    return;
                }
                else
                {
                    UsernameAlreadyExists = false;
                    UsernameColor = Color.FromArgb("#E6EEF9");
                }
                if (Username != null && Password != null) 
                {
                    if (Password == String.Empty)
                    {
                        App.Current.MainPage.DisplayAlert("", $"Das Passwort Feld darf nicht leer sein!", "Ok");
                        return;
                    }
                    else if(Username == string.Empty)
                    {
                        App.Current.MainPage.DisplayAlert("", $"Sie müssen einen Benutzernamen wählen!", "Ok");
                        return;
                    }

                    if (isFirstAccount)
                    {
                        if (hashedPW == null)
                        {
                            App.Current.MainPage.DisplayAlert("", $"Das Passwort Feld ist leer!", "Ok");
                            return;
                        }
                        App.EmployeeRepo.SaveItem(new Employee() { Username = this.Username, Firstname = this.Firstname, Lastname = this.Lastname, 
                            Birthday = this.Birthday, City = this.City, Country = this.Country, EMail = this.EMail, PostalCode = this.PostalCode, 
                            Street = this.Street, Password = hashedPW, /*Salt = salt,*/ Role = Role.Admin });         //Salt braucht man nur nach der anderen Hash Verschlüsselung
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
                                App.Current.MainPage.DisplayAlert("Fehlende Rechte", alertTextAdmin, "Ok");
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
                                App.Current.MainPage.DisplayAlert("Fehlende Rechte", alertTextAdmin, "Ok");
                                return;
                            }
                        }

                        rbsIsVisible = false;
                        App.EmployeeRepo.SaveItem(new Employee()
                        {
                            Username = this.Username,
                            Firstname = this.Firstname,
                            Lastname = this.Lastname,
                            Birthday = this.Birthday,
                            City = this.City,
                            Country = this.Country,
                            EMail = this.EMail,
                            PostalCode = this.PostalCode,
                            Street = this.Street,
                            Password = hashedPW,
                            //Salt = salt,
                            Role = role
                        });
                    }
                    DebugMessage = App.EmployeeRepo.StatusMessage;
                    App.Current.MainPage.DisplayAlert("Account angelegt", $"Der Account mit dem Benutzernamen \"{Username}\" wurde erfolgreich angelegt", "Ok");
                    Shell.Current.GoToAsync("CreateAccount/LoginPage");
                    HideControls(vslRegisterElements);
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("","Der Benutzername und das Passwort Feld darf nicht leer sein!","Ok");
                    return;
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
                else if (child is SfTextInputLayout)
                {
                    SfTextInputLayout entry = (SfTextInputLayout)child;
                    entry.IsVisible = false;
                }
                else if (child is Label)
                {
                    var lbl = (Label)child;
                    lbl.IsVisible= false;
                }
            }
        }

        //private void ShowBackButton(object vslRegisterElements)
        //{
        //    var vsl = (VerticalStackLayout)vslRegisterElements;
        //    Button btn = (Button)vsl.FindByName("btnBackToMenu");//(Button)vsl.Children[vsl.Children.Count - 1];

        //    if (btn != null )
        //    {
        //        btn.IsVisible = true;   
        //    }
        //}

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
