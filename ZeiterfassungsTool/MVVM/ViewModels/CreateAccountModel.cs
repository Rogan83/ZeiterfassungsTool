using Microsoft.Maui.Controls;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
//using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MySQLModels;

using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.StaticClasses;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Core;
using System.Text.Json;
using MvvmHelpers.Interfaces;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CreateAccountModel
    {
        // Verbindung zum MySQL Server
        HttpClient client;
        JsonSerializerOptions _serializerOptions;
        static string baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        static string urlEmployee = $"{baseAddress}/api/Employee";
        ////////////////////////////////////////
        
        public CreateAccountModel()
        {
            //LbUsername = $"Sie sind mit dem Benutzername {Login.WhoIsLoggedIn[0].Username} angemeldet.";

            client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
        }

        #region Properties
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        //public string Birthday { get; set; }
        public DateTime Birthday { get; set; }
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

        //public ICommand BackButton =>
        //   new Command(() =>
        //   {
        //       Shell.Current.GoToAsync("CreateAccount/LoginPage");
        //   });

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

        //public ICommand DeleteTable =>
        //    new Command(() =>
        //    {
        //        App.EmployeeRepo.DeleteTable();
        //    });

        //var salt = DateTime.Now.ToString();
        //var hashedPW = Hash.HashPassword($"{Password}{salt}");          // Das Passwort mit dem Salt in einen Hash Wert umwandeln (Der Salt Wert ändert das gehashte PW nochmals ab, weil z.B. ein Passwort "1234" immer den gleichen Wert als Hash ergibt. So könnte man daraus schließen, dass ein gleicher Hash Wert zum gleichen Passwort gehört. Da nun zusätzlich noch ein Salt Wert hinzugefügt wird, welcher bei jeden User anders ist, ist auch das Passwort bei jeden User anders, selbst wenn User A das selbe PW hat wie User B 

        public ICommand ToRegister =>
            new Command(async(vslRegisterElements) =>
            {
                await CheckIfOneAccountExist();
                var hashedPW = Hash.HashPasswordScrypt(Password);

                if (await ExistsThisUser())           //Untersucht, ob der Benutzername schon vergeben wurde
                {
                    await App.Current.MainPage.DisplayAlert("","Dieser Benutzername ist bereits schon vergeben.","OK");
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
                        await App.Current.MainPage.DisplayAlert("", $"Das Passwort Feld darf nicht leer sein!", "Ok");
                        return;
                    }
                    else if(Username == string.Empty)
                    {
                        await App.Current.MainPage.DisplayAlert("", $"Sie müssen einen Benutzernamen wählen!", "Ok");
                        return;
                    }

                    if (isFirstAccount)
                    {
                        if (hashedPW == null)
                        {
                            await App.Current.MainPage.DisplayAlert("", $"Das Passwort Feld ist leer!", "Ok");
                            return;
                        }

                        //SQLite
                        //App.EmployeeRepo.SaveItem(new Employee()
                        //{
                        //    Username = this.Username,
                        //    Firstname = this.Firstname,
                        //    Lastname = this.Lastname,
                        //    Birthday = this.Birthday,
                        //    City = this.City,
                        //    Country = this.Country,
                        //    EMail = this.EMail,
                        //    PostalCode = this.PostalCode,
                        //    Street = this.Street,
                        //    Password = hashedPW, /*Salt = salt,*/
                        //    Role = Role.Admin
                        //});         //Salt braucht man nur nach der anderen Hash Verschlüsselung

                        InitEmployeeEntrys();

                        Save(hashedPW, 3);      //3 = admin
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
                            //if (Login.WhoIsLoggedIn[0].Role == Role.Admin)            //Wenn ein Admin angemeldet ist, dann kann der Account angelegt werden
                            if (Login.WhoIsLoggedIn[0].RoleId == 3)            //Wenn ein Admin angemeldet ist, dann kann der Account angelegt werden
                            {
                                role = Role.Management;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Fehlende Rechte", alertTextAdmin, "Ok");
                                return;
                            }
                        }
                        else if (rbAdmin)
                        {
                            //if (Login.WhoIsLoggedIn[0].Role == Role.Admin)            //Wenn ein Admin angemeldet ist, dann kann der Account angelegt werden
                            if (Login.WhoIsLoggedIn[0].RoleId == 3)            //Wenn ein Admin angemeldet ist, dann kann der Account angelegt werden
                            {
                                role = Role.Admin;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Fehlende Rechte", alertTextAdmin, "Ok");
                                return;
                            }
                        }

                        rbsIsVisible = false;

                        //SQLite

                        //App.EmployeeRepo.SaveItem(new Employee()
                        //{
                        //    Username = this.Username,
                        //    Firstname = this.Firstname,
                        //    Lastname = this.Lastname,
                        //    Birthday = this.Birthday,
                        //    City = this.City,
                        //    Country = this.Country,
                        //    EMail = this.EMail,
                        //    PostalCode = this.PostalCode,
                        //    Street = this.Street,
                        //    Password = hashedPW,
                        //    //Salt = salt,
                        //    Role = role
                        //});

                        int id;

                        switch (role)
                        {
                            case Role.User:
                                id = 1;
                                break;
                            case Role.Management:
                                id = 2;
                                break;
                            case Role.Admin:
                                id = 3;
                                break;
                            default:
                                id = 1;
                                break;
                        }

                        InitEmployeeEntrys();

                        Save(hashedPW, id);
                    }
                    DebugMessage = App.EmployeeRepo.StatusMessage;
                    await App.Current.MainPage.DisplayAlert("Account angelegt", $"Der Account mit dem Benutzernamen \"{Username}\" wurde erfolgreich angelegt", "Ok");
                    await Shell.Current.GoToAsync("CreateAccount/LoginPage");
                    HideControls(vslRegisterElements);
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("","Der Benutzername und das Passwort Feld darf nicht leer sein!","Ok");
                    return;
                }
                
            });

        #endregion

        #region Methods
        //MySQL
        private async void Save(string hashedPW, int id)
        {
            await MySQLMethods.SaveAccount(new Employee()
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
                WorkingHoursPerWeek = 40,
                RoleId = id              // 1 = user, 2 = management, 3 = admin
            });
        }

        private void InitEmployeeEntrys()
        {
            if (this.Username == null)
                this.Username = string.Empty;
            if (this.Firstname == null)
                this.Firstname = string.Empty;
            if (this.Lastname == null)
                this.Lastname = string.Empty;
            if (this.Birthday == null)
                this.Birthday = DateTime.Now;
            if (this.City == null)
                this.City = string.Empty;
            if (this.Country == null)
                this.Country = string.Empty;
            if (this.EMail == null)
                this.EMail = string.Empty;
            if (this.PostalCode == null)
                this.PostalCode = string.Empty;
            if (this.Street == null)
                this.Street = string.Empty;
            if (this.Password == null)
                this.Password = string.Empty;
        }

        private async Task<bool> ExistsThisUser()
        {
            ////SQLite
            //var user = App.EmployeeRepo.GetItem(x => x.Username == this.Username);

            //if (user == null)
            //    return false;

            //return true;

            //MySQL
            var user = await MySQLMethods.GetSingleAccount(this.Username);

            if (user.Count == 0)
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

        public async Task CheckIfOneAccountExist()
        {
            //SQLite
            //int count = App.EmployeeRepo.GetItems().Count();

            //MySQL
            int count = (await MySQLMethods.GetAllAccounts()).Count;

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
