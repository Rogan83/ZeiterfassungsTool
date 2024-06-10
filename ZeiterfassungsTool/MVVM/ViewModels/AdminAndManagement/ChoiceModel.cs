using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ZeiterfassungsTool.Enumerations;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class ChoiceModel
    {
        public bool IsRunningBusyIndicator { get; set; } = false;
        public bool IsVisible { get; set; } = false;

        private static string path = "";
        private static string pathWindows = "C:\\TimetrackingTool\\";
        private static string pathAndroid = "/storage/emulated/0/Documents/";
        private static string fileNameEmployee = "Employees.csv";
        private static string fileNameTimetracking = "Timetracking.csv";

        static bool IsAndroid() => DeviceInfo.Current.Platform == DevicePlatform.Android;       // Überpüft, ob das Programm für die Android Plattform kompiliert wurde

        public ChoiceModel() 
        {
            path = DeterminePath();
        }

        public ICommand BackButton =>
            new Command(() =>
            {
                Shell.Current.GoToAsync("Choice/LoginPage");
            });

        public ICommand GoToUserListView =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(UserList));
           });

        public ICommand GoToAccountsListView =>
          new Command(() =>
          {
              Shell.Current.GoToAsync(nameof(AdminAndManagementManagement));
          });

        public ICommand GoToOvertimeView =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(Overtime));
           });

        public ICommand GoToScheduler =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(Scheduler));
           });

        public ICommand SaveDatabaseInCSVFile =>
           new Command(async() =>
           {
               path = DeterminePath();
               
               bool answer = await App.Current.MainPage.DisplayAlert("Backup", $"Wenn Sie fortfahren, dann werden die Daten im Verzeichnis '{path}' geschrieben. Wenn sich dort schon ein Backup befindet, wird dieses überschrieben. Möchten Sie fortfahren? Dieser Vorgang kann nicht rückgängig gemacht werden.", "Ja", "Nein");
               
               if (!answer) return;
               //SQLite
               //List<Employee> employees = App.EmployeeRepo.GetItems();
               //List<Timetracking> timetracking = App.TimetrackingRepo.GetItems();

               //MySQL
               List<MySQLModels.Employee> employees = await MySQLMethods.GetAllAccounts();
               List<MySQLModels.Timetracking> timetracking = await MySQLMethods.GetAllTimetrackingData();


               bool isCreatedOrUpdatedCSVFile = ToCSV(employees, timetracking);

               if (isCreatedOrUpdatedCSVFile)
                    await App.Current.MainPage.DisplayAlert("Backup", "Backup wurde erfolgreich erstellt.", "OK");
           });

        public ICommand LoadDatabaseFromCSVFile =>
           new Command(async(grid) =>
           {
               path = DeterminePath();

               var answer = await App.Current.MainPage.DisplayAlert("Achtung", $"Möchten Sie wirklich die Datenbank mit den Daten im Verzeichnis '{path}' überschreiben? Dieser Vorgang kann nicht rückgängig gemacht werden.", "Ja","Nein");               
               if (!answer) return;

               var elements = (Grid)grid;
               //SfBusyIndicator busy = (SfBusyIndicator)grid.
               SfBusyIndicator busy = elements.FindByName("busy") as SfBusyIndicator;
               if (busy == null)
                   return;

               busy.IsRunning = true;   //Funktioniert leider nicht, da die Methode FromCSV nicht asynchron ausgeführt wird...
               bool isOverwriteDatabaseFromCSVFiles = await FromCSV();
               busy.IsRunning = false;

               if (isOverwriteDatabaseFromCSVFiles)
                    await App.Current.MainPage.DisplayAlert("", "Die Übertragung ist abgeschlossen.", "OK");
           });

        /// <summary>
        /// Überprüfe, ob der Code für Android oder Windows erstellt wurde und passe dementsprechend den Pfad an.
        /// </summary>
        /// <returns></returns>
        private string DeterminePath()
        {
            string path;
            if (IsAndroid())
            {
                path = pathAndroid;
            }
            else
            {
                path = pathWindows;
            }
            return path;
        }

        //private string ToCSVEmployee(Employee employee)
        private string ToCSVEmployee(MySQLModels.Employee employee)
        {
            return employee.Id + ";" + employee.Username + ";" + employee.Firstname + ";" + employee.Lastname + ";" + employee.Street + ";" + employee.PostalCode + ";" + employee.City + ";" + employee.Country
                + ";" + employee.Birthday + ";" + employee.EMail + ";" + employee.Password + ";" + employee.WorkingHoursPerWeek + ";" + employee.VacationDaysPerYear + ";" + employee.IsResetPassword /*+ ";" + employee.Salt*/ + ";" + employee.RoleId;
        }

        //private string ToCSVTimetracking(Timetracking timetracking)
        private string ToCSVTimetracking(MySQLModels.Timetracking timetracking)
        {
            return timetracking.Id + ";" + timetracking.StartTime + ";" + timetracking.EndTime + ";" + timetracking.Subject + ";" + timetracking.EmployeeId;
        }
        /// <summary>
        /// Versucht, die jede Tabelle von der Datenbank jeweils in eine CSV Datei zu speichern
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="timetrackingList"></param>
        /// <returns></returns>
        //private bool ToCSV(List<Employee> employees, List<Timetracking> timetrackingList)
        private bool ToCSV(List<MySQLModels.Employee> employees, List<MySQLModels.Timetracking> timetrackingList)
        {
            path = DeterminePath();
            bool isOpenEmployeeFile = true;        // Ist die Datei geöffnet?
            
            try
            {
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (StreamWriter sw = new(path + fileNameEmployee))
                {
                    //foreach (Employee employee in employees)
                    foreach (MySQLModels.Employee employee in employees)
                    {
                        sw.WriteLine(ToCSVEmployee(employee));
                    }
                }
                isOpenEmployeeFile = false;         //Wenn es gelungen ist, in der Datei alle Mitarbeiter zu speichern, dann war die Datei nicht geöffnet.

                using (StreamWriter sw = new(path + fileNameTimetracking))
                {
                    //foreach (Timetracking timetracking in timetrackingList)
                    foreach (MySQLModels.Timetracking timetracking in timetrackingList)
                    {
                        sw.WriteLine(ToCSVTimetracking(timetracking));
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehlermeldung: " + ex.Message);
                if (isOpenEmployeeFile)
                    App.Current.MainPage.DisplayAlert("Warnung", $"Bitte schließen Sie die Datei {fileNameEmployee} im Verzeichnis {path}, sonst kann diese nicht überschrieben werden.","Ok");
                else
                    App.Current.MainPage.DisplayAlert("Warnung", $"Bitte schließen Sie die Datei {fileNameTimetracking} im Verzeichnis {path}, sonst kann diese nicht überschrieben werden.", "Ok");

                return false;
            }
        }
        /// <summary>
        /// Überschreibt die Datenbank mit den Daten von den CSV Dateien.
        /// </summary>
        private async Task<bool> FromCSV()
        {
            path = DeterminePath();

            bool isFoundTimetrackingFile = false;           // Wurde diese Datei gefunden?
            //SQLite
            //List<Timetracking> timetrackinglist = new List<Timetracking>();
            //MySQL
            List<MySQLModels.Timetracking> timetrackinglist = new List<MySQLModels.Timetracking>();
            try
            {
                using (StreamReader sr = new StreamReader(path + fileNameTimetracking))
                {
                    //SQLite
                    //App.TimetrackingRepo.DropTable();
                    //App.TimetrackingRepo.CreateTable();

                    //MySQL
                    await MySQLMethods.DeleteAllTimes();

                    while (!sr.EndOfStream)
                    {
                        string[] parts = sr.ReadLine().Split(';');

                        //SQLite
                        //var timetracking = new Timetracking()
                        //MySQL
                        var timetracking = new MySQLModels.Timetracking()
                        {
                            Id = Convert.ToInt32(parts[0]),
                            StartTime = Convert.ToDateTime(parts[1]),
                            EndTime = Convert.ToDateTime(parts[2]),
                            Subject = Convert.ToString(parts[3]),
                            EmployeeId = Convert.ToInt32(parts[4])
                        };

                        timetrackinglist.Add(timetracking);

                        //SQLite
                        //App.TimetrackingRepo.SaveItem(timetracking);

                        //MySQL
                        await MySQLMethods.AddTime(timetracking);
                    }
                }
                isFoundTimetrackingFile = true;

                using (StreamReader sr = new StreamReader(path + fileNameEmployee))
                {
                    //SQLite
                    //App.EmployeeRepo.DropTable();
                    //App.EmployeeRepo.CreateTable();

                    //MySQL
                    await MySQLMethods.DeleteAllAccounts();
                    //SQLite
                    //List<Employee> employees = new List<Employee>();
                    //MySQL
                    List<MySQLModels.Employee> employees = new List<MySQLModels.Employee>();
                    while (!sr.EndOfStream)
                    {
                        string[] parts = sr.ReadLine().Split(';');
                        //SQLite
                        Role role = (Role)Enum.Parse(typeof(Role), parts[14]);
                        //MySQL
                        int roleId = Convert.ToInt32(parts[14]);

                        int iDEmployee = Convert.ToInt32(parts[0]);
                        //SQLite
                        //List<Timetracking> timetrackingsForThisUser = new List<Timetracking>();
                        //foreach (var timetracking in timetrackinglist)
                        //{
                        //    if (timetracking.EmployeeId == iDEmployee)
                        //    {
                        //        timetrackingsForThisUser.Add(timetracking);
                        //    }
                        //}

                        //var employee = new Employee()
                        var employee = new MySQLModels.Employee()
                        {
                            Id = iDEmployee,
                            Username = Convert.ToString(parts[1]),
                            Firstname = Convert.ToString(parts[2]),
                            Lastname = Convert.ToString(parts[3]),
                            Street = Convert.ToString(parts[4]),
                            PostalCode = Convert.ToString(parts[5]),
                            City = Convert.ToString(parts[6]),
                            Country = Convert.ToString(parts[7]),
                            //SQLite
                            //Birthday = Convert.ToString(parts[8]),
                            //MySQL
                            Birthday = Convert.ToDateTime(parts[8]),
                            EMail = Convert.ToString(parts[9]),
                            Password = Convert.ToString(parts[10]),
                            WorkingHoursPerWeek = Convert.ToInt32(parts[11]),
                            VacationDaysPerYear = Convert.ToInt32(parts[12]),
                            IsResetPassword = Convert.ToBoolean(parts[13]),
                            //Salt = Convert.ToString(parts[13]),
                            //SQLite
                            //Role = role, //14
                            //MySQL
                            RoleId = roleId,

                            //SQLite
                            //Timetracking = timetrackingsForThisUser
                        };

                        //SQLite
                        //App.EmployeeRepo.SaveItemWithChildren(employee);
                        //MySQL
                        await MySQLMethods.SaveAccount(employee);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehlermeldung: " + ex.Message);

                if (!isFoundTimetrackingFile)
                    await App.Current.MainPage.DisplayAlert("Warnung", $"Die Datei {fileNameTimetracking} im Verzeichnis {path} wurde nicht gefunden.", "Ok");
                else
                    await App.Current.MainPage.DisplayAlert("Warnung", $"Die Datei {fileNameEmployee} im Verzeichnis {path} wurde nicht gefunden.", "Ok");
                return false;
            }
        }

    }
}
