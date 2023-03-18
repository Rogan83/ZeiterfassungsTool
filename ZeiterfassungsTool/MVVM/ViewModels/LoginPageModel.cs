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
using static SQLite.SQLite3;


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

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("LoginPage/StartPage");
           });

        public LoginPageModel() 
        {
            UpdateButtonForwardText();
            //Margin = new Thickness(20, 5, 20, 5);
            Margin = new Thickness(20, 10, 20, 10);
        }

        public ICommand DeleteTable =>
            new Command(() =>
            {
                //App.EmployeeRepo.DeleteTable();
                //App.TimetrackingRepo.DeleteTable();
                App.EmployeeRepo.DropTable(); App.EmployeeRepo.CreateTable();
                App.TimetrackingRepo.DropTable(); App.TimetrackingRepo.CreateTable();
            });

        public ICommand ToLogin =>
           new Command((element) =>
           {
               var username = EntryUsername; 
               var password = EntryPassword;

               if ((username != null && password != null) ) 
               {
                   if (password == string.Empty || username == string.Empty)
                   {
                       App.Current.MainPage.DisplayAlert("Warnung", "Es müssen beide Felder ausgefüllt sein.", "OK");
                       return;
                   }

                   //var results = App.EmployeeRepo.GetItems(x => x.Username == username && x.Password == password);            //Hatte die Kind Elemente nicht mit geholt
                   List<Employee> allAccounts = App.EmployeeRepo.GetItemsWithChildren();

                   //List<Employee> results = new List<Employee>();
                   //foreach (var item in allAccounts)
                   //{
                   //    if (item.Username == username && item.Password == password)
                   //    {
                   //        results.Add(item);
                   //    }
                   //}
                   bool isEqual = false;
                   var loginUser = allAccounts.FindAll(u => u.Username == username);
                   if (loginUser.Count != 0)
                   {
                       //results = allAccounts.Where(u => u.Password == Hash.HashPassword($"{password}{loginUser[0].Salt}")).ToList();
                       isEqual = Hash.Encoder.Compare(password, loginUser[0].Password);            //Vergleicht das eingegebene - und das gehashte Passwort. Wenn sie übereinstimmen, dann wird true zurückgegeben.  
                   }

                   Debug = App.EmployeeRepo.GetItems().Count.ToString();

                   //var count = results.Count();
                   var count = loginUser.Count();

                   if (count > 0 && isEqual)  //Bedeutet, dass min. 1 Account gefunden wurde, der diesen Username hat und das Passwort korrekt ist.
                   {
                       Login.WhoIsLoggedIn = loginUser;         // Speicher ab, wer sich erfolgreich eingeloggt hat.

                       if (Login.WhoIsLoggedIn[0].IsResetPassword)
                       {
                           App.Current.MainPage.DisplayAlert("Erinnerung", "Ihr Passwort wurde zurückgesetzt. Bitte ändern sie dies sofort in den Einstellungen ab.", "OK");
                       }

                       DeactivateAndActivateSeveralButtonAndEntries(element, true);

                       UpdateButtonForwardText();
                   }
                   else if (count > 1)
                   {
                       App.Current.MainPage.DisplayAlert("Warnung", "Es existieren min. 2 Accounts mit dem selben Benutzernamen!", "OK");
                   }
                   else
                   {
                       //Message = "Benutzername oder Passwort sind falsch";
                       App.Current.MainPage.DisplayAlert("Warnung", "Benutzername oder Passwort sind nicht korrekt.", "OK");
                   }
               }
               else
               {
                   App.Current.MainPage.DisplayAlert("Warnung", "Es müssen beide Felder ausgefüllt sein.", "OK");
                   //Message = "Es müssen beide Felder ausgefüllt sein.";
               }
           });
        /// <summary>
        /// Deaktiviert und Aktiviert bestimmte Buttons und Entries, nachdem sich jemand Ausgeloggt hat.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="activate"></param>
        private void DeactivateAndActivateSeveralButtonAndEntries(object element, bool activate)
        {
            VerticalStackLayout flexLayout = (VerticalStackLayout)element;

            Button btnForwardToContent = (Button)flexLayout.FindByName("btnForwardToContent");
            Button btnLogout = (Button)flexLayout.FindByName("btnLogout");

            SfTextInputLayout entryUsername = (SfTextInputLayout)flexLayout.FindByName("entryUsername");
            SfTextInputLayout entryPassword = (SfTextInputLayout)flexLayout.FindByName("entryPassword");
            Button btnLogin = (Button)flexLayout.FindByName("btnLogin");

            btnForwardToContent.IsVisible = activate;
            btnLogout.IsVisible = activate;

            entryUsername.IsVisible = !activate;
            entryPassword.IsVisible = !activate;
            btnLogin.IsVisible = !activate;
        }

        /// <summary>
        /// Passt den Text vom button in Abhängigkeit davon an, wer sich eingeloggt hat.
        /// </summary>
        private void UpdateButtonForwardText()
        {
            var result = Login.WhoIsLoggedIn[0];
            switch (result.Role)
            {
                case Role.User:
                    TxtForwardToContent = "Weiter zur Benutzerseite";
                    break;
                case Role.Management:
                    TxtForwardToContent = "Weiter zur Geschäftsführungsseite";
                    break;
                case Role.Admin:
                    TxtForwardToContent = "Weiter zur Adminseite";
                    break;
            }
        }

        public ICommand ForwardToContent =>
          new Command(() =>
          {
              var result = Login.WhoIsLoggedIn[0];

              switch (result.Role)
              {
                  case Role.User:
                      Shell.Current.GoToAsync(nameof(UserPage));
                      break;
                  case Role.Management:
                      Shell.Current.GoToAsync(nameof(Choice));
                      break;
                  case Role.Admin:
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
              Login.WhoIsLoggedIn = new List<Employee>() { new Employee() };
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
