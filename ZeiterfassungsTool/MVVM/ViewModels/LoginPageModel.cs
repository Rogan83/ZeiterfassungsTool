using Microsoft.Maui.Controls;
using PropertyChanged;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;
using ZeiterfassungsTool.StaticClasses;



namespace ZeiterfassungsTool.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginPageModel
    {
        public Thickness Margin { get; set; }
        public string Message { get; set; }
        public string Debug { get; set; }

        public string EntryUsername { get; set; }
        public string EntryPassword { get; set; }

        public string TxtForwardToContent { get; set; }

        //public ICommand BackButton =>
        //   new Command(() =>
        //   {
        //       //Shell.Current.GoToAsync("LoginPage/StartPage");
        //   });

        public LoginPageModel() 
        {
            UpdateButtonForwardText();
            Margin = new Thickness(20, 10, 20, 10);
        }
        /// <summary>
        /// Löscht die Tabelle und erstellt eine erneute.
        /// </summary>
        public ICommand DeleteTable =>
            new Command(() =>
            {
                // Ich habe deswegen die komplette Tabelle gelöscht und wieder erstellt, damit die IDs wieder von vorne starten. Wenn ich den Inhalt der Tabelle nur löschen würde, dann würde er an der Stelle mit der ID weiterzählen, von wo er aufgehört hatte.
                App.EmployeeRepo.DropTable(); 
                App.EmployeeRepo.CreateTable();

                App.TimetrackingRepo.DropTable(); 
                App.TimetrackingRepo.CreateTable();
            });

        public ICommand ToLogin =>
           new Command(async(element) =>
           {
               var username = EntryUsername; 
               var password = EntryPassword;

               if (username != null && password != null) 
               {
                   if (password == string.Empty || username == string.Empty)
                   {
                       await App.Current.MainPage.DisplayAlert("Warnung", "Es müssen beide Felder ausgefüllt sein.", "OK");
                       return;
                   }

                   //var results = App.EmployeeRepo.GetItems(x => x.Username == username && x.Password == password);            //Hatte die Kind Elemente nicht mit geholt
                   //SQLite
                   List<Employee> allAccountsSQLite = App.EmployeeRepo.GetItemsWithChildren();

                   //MySQL
                   List<MySQLModels.Employee> allAccounts = await MySQLMethods.GetAllAccounts();

                   bool isEqual = false;
                   var loginUser = allAccounts.FindAll(u => u.Username == username);
                   if (loginUser.Count != 0)
                   {
                       //results = allAccounts.Where(u => u.Password == Hash.HashPassword($"{password}{loginUser[0].Salt}")).ToList();
                       isEqual = Hash.Encoder.Compare(password, loginUser[0].Password);            //Vergleicht das eingegebene - und das gehashte Passwort. Wenn sie übereinstimmen, dann wird true zurückgegeben.  
                   }

                   //Debug = App.EmployeeRepo.GetItems().Count.ToString();

                   var count = loginUser.Count();

                   if (count == 1 && isEqual)  //Bedeutet, dass min. 1 Account gefunden wurde, der diesen Username hat und das Passwort korrekt ist.
                   {
                       Login.WhoIsLoggedIn = loginUser;         // Speicher ab, wer sich erfolgreich eingeloggt hat.

                       if (Login.WhoIsLoggedIn[0].IsResetPassword)
                       {
                           await App.Current.MainPage.DisplayAlert("Erinnerung", "Ihr Passwort wurde zurückgesetzt. Bitte ändern sie dies sofort in den Einstellungen ab.", "OK");
                       }

                       DeactivateAndActivateSeveralButtonAndEntries(element, true);

                       UpdateButtonForwardText();
                   }
                   else if (count > 1)
                   {
                       await App.Current.MainPage.DisplayAlert("Warnung", "Es existieren min. 2 Accounts mit dem selben Benutzernamen!", "OK");
                   }
                   else
                   {
                       await App.Current.MainPage.DisplayAlert("Warnung", "Benutzername oder Passwort sind nicht korrekt.", "OK");
                   }
               }
               else
               {
                   await App.Current.MainPage.DisplayAlert("Warnung", "Es müssen beide Felder ausgefüllt sein.", "OK");
               }
           });
        /// <summary>
        /// Deaktiviert und Aktiviert bestimmte Buttons und Entries, nachdem sich jemand Ausgeloggt hat.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isVisible"></param>
        private void DeactivateAndActivateSeveralButtonAndEntries(object element, bool isVisible)
        {
            VerticalStackLayout flexLayout = (VerticalStackLayout)element;

            Button btnForwardToContent = (Button)flexLayout.FindByName("btnForwardToContent");
            Button btnLogout = (Button)flexLayout.FindByName("btnLogout");

            SfTextInputLayout entryUsername = (SfTextInputLayout)flexLayout.FindByName("entryUsername");
            SfTextInputLayout entryPassword = (SfTextInputLayout)flexLayout.FindByName("entryPassword");
            Button btnLogin = (Button)flexLayout.FindByName("btnLogin");

            btnForwardToContent.IsVisible = isVisible;
            btnLogout.IsVisible = isVisible;

            entryUsername.IsVisible = !isVisible;
            entryPassword.IsVisible = !isVisible;
            btnLogin.IsVisible = !isVisible;
        }

        /// <summary>
        /// Passt den Text vom button in Abhängigkeit davon an, wer sich eingeloggt hat.
        /// </summary>
        private void UpdateButtonForwardText()
        {
            MySQLModels.Employee result;
            if (Login.WhoIsLoggedIn != null)
                result = Login.WhoIsLoggedIn[0];
            else
                return;
            //switch (result.Role)
            //{
            //    case Role.User:
            //        TxtForwardToContent = "Weiter zur Benutzerseite";
            //        break;
            //    case Role.Management:
            //        TxtForwardToContent = "Weiter zur Geschäftsführung";
            //        break;
            //    case Role.Admin:
            //        TxtForwardToContent = "Weiter zur Adminseite";
            //        break;
            //}

            switch (result.RoleId)
            {
                case 1:
                    TxtForwardToContent = "Weiter zur Benutzerseite";
                    break;
                case 2:
                    TxtForwardToContent = "Weiter zur Geschäftsführung";
                    break;
                case 3:
                    TxtForwardToContent = "Weiter zur Adminseite";
                    break;
            }
        }

        public async Task CheckIfOneAccountExist()
        {
            //SQLite
            //int count = App.EmployeeRepo.GetItems().Count();

            //MySQL
            int count = (await MySQLMethods.GetAllAccounts()).Count;

            if (count == 0)
            {
                await App.Current.MainPage.DisplayAlert("", "Es existiert noch kein Account. Es wird jetzt ein Admin Account eingerichtet", "OK");
                await Shell.Current.GoToAsync(nameof(CreateAccount));
                //Noch kein Account in der Datenbank vorhanden. Der erste Account wird somit ein Admin Account werden.
            }
        }
            


        /// <summary>
        /// Damit wird zur nächsten Seite gesprungen. Zur welche Seite man springt, hängt von den Rechten ab, die der Nutzer hat.
        /// </summary>
        public ICommand ForwardToContent =>
          new Command(() =>
          {
              var result = Login.WhoIsLoggedIn[0];

              //switch (result.Role)
              switch (result.RoleId)
              {
                  //case Role.User:
                  case 1:
                      Shell.Current.GoToAsync(nameof(UserPage));
                      break;
                  //case Role.Management:
                  case 2:
                      Shell.Current.GoToAsync(nameof(Choice));
                      break;
                  //case Role.Admin:
                  case 3:
                       Shell.Current.GoToAsync(nameof(Choice));
                       break;
                  default:
                      Shell.Current.GoToAsync(nameof(UserPage));
                      break;
              }
          });
        public ICommand Logout =>
          new Command((element) =>
          {
              Login.WhoIsLoggedIn = new List<MySQLModels.Employee>() { new MySQLModels.Employee() };
              Shell.Current.GoToAsync(nameof(LoginPage));

              DeactivateAndActivateSeveralButtonAndEntries(element, false);

              EntryUsername = String.Empty;
              EntryPassword = String.Empty;
          });

        public ICommand GoToCreateAccountSite =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(CreateAccount));
           });
    }
}
