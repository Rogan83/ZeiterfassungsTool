using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;
using ZeiterfassungsTool.MySQLModels;

namespace ZeiterfassungsTool.MVVM.ViewModels.AdminAndManagement
{
    
    public class SettingsConnectionToDatabaseModel
    {
        public string path = @"D:";
        public string filename = "Settings.txt";
        public string pathPlusFilename;

        static HttpClient client;
        static JsonSerializerOptions _serializerOptions;

        public string ConnectionData { get; set; }          //Wird von der XAML Datei gebunden und der Wert wird im Entry Feld festgelegt

        public SettingsConnectionToDatabaseModel() 
        {
            pathPlusFilename = String.Format($"{path}/{filename}");

            client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
        }

        public ICommand ReadConnectionData =>
           new Command(async() =>
           {
               if (!File.Exists(pathPlusFilename))
               {
                   var file = File.Create(pathPlusFilename);
                   file.Close();
               }

               string baseAdress = ConnectionData;
               string urlEmployee = $"{baseAdress}/api/Employee";
               string urlTimetracking = $"{baseAdress}/api/Timetracking";

               //Verbindung überprüfen
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(urlEmployee);

                    File.WriteAllText(pathPlusFilename, ConnectionData);

                    Debug.WriteLine("Daten wurden in die Datei geschrieben");

                    await App.Current.MainPage.DisplayAlert("", "Verbindung erfolgreich aufgebaut", "OK");
                    await App.Current.MainPage.DisplayAlert("", "Daten wurden in die Datei geschrieben.", "OK");

                    //await Shell.Current.GoToAsync(nameof(LoginPage));
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("", $"Es konnte keine Verbindung zum Server hergestellt werden.", "OK");
                    Debug.WriteLine(ex);
                }
           });
    }
}
