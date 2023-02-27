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
    public class LoginPageModel
    {
        public string Message { get; set; }
        public ICommand ToLogin =>
           new Command((elements) =>
           {
               string username = string.Empty;
               string password = string.Empty;

               var children = ((VerticalStackLayout)elements).Children;
               foreach (var child in children)
               {
                   if (child is Entry)
                   {
                       try
                       {
                           username = ((Entry)((Entry)child).FindByName("username")).Text;
                           password = ((Entry)((Entry)child).FindByName("password")).Text;
                       }
                       catch (NullReferenceException e)     //Fängt es nicht ab, ka wieso
                       {
                           //((Label)((Label)child).FindByName("DebugMessage")).Text = e.Message;
                       }

                   }
               }
               if (username != null || password != null)
               {
                   var results = App.EmployeeRepo.GetItems(x => x.Username == username && x.Password == password);
                   var count = results.Count();

                   if (count > 0)  //Bedeutet, dass min. 1 Account gefunden wurde, der diesen Username und Passwort hat.
                   {
                       SaveLoginStatus.WhoIsLoggedIn = results;         // Speicher ab, wer sich erfolgreich eingeloggt hat.

                       foreach (var result in results)      //Es könnte theoretisch sein, dass mehrere Accounts mit Users und Admin Rollen vorhanden sind, die den selben Benutzername und Passwort haben ... Man müsste dafür sorgen, dass der Benutzername nicht mehr als 1 mal vergeben wird
                       {
                           switch (result.Role)
                           {
                               case Role.User:
                                   Shell.Current.GoToAsync(nameof(UserPage));
                                   break;
                               case Role.Management:
                                   Shell.Current.GoToAsync(nameof(ManagementPage));
                                   break;
                               case Role.Admin:
                                   Shell.Current.GoToAsync(nameof(AdminPage));
                                   break;

                               default:
                                   Shell.Current.GoToAsync(nameof(UserPage));
                                   break;
                           }
                       }


                   }
                   else
                   {
                       Message = "Benutzername oder Passwort sind falsch";
                   }
               }
               else
               {
                   Message = "Es müssen beide Felder ausgefüllt sein.";
               }


           });

        public ICommand BackToMainMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("LoginPage/StartPage");
           });
    }
}
