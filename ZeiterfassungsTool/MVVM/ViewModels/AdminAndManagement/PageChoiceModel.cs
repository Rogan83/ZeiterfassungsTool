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
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class PageChoiceModel
    {
        public bool IsRunningBusyIndicator { get; set; } = false;
        public bool IsVisible { get; set; } = false;


        private static string path = "";
        private static string pathWindows = "C:\\TimetrackingTool\\";
        private static string pathAndroid = "/storage/emulated/0/Documents/";
        private static string filenameEmployee = "Employees.csv";
        private static string filenameTimetracking = "Timetracking.csv";

        static bool IsAndroid() => DeviceInfo.Current.Platform == DevicePlatform.Android;

        public PageChoiceModel() 
        {
            
        }

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("PageChoice/LoginPage");
           });
        public ICommand BackToMainMenu =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("PageChoice/LoginPage");
           });
        

        public ICommand GoToUserListView =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("PageChoice/PageUserList");
           });

        public ICommand GoToAccountsListView =>
          new Command(() =>
          {
              Shell.Current.GoToAsync(nameof(PageAdminAndManagementManagement));
          });

        public ICommand GoToOvertimeView =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(PageOvertime));
           });

        public ICommand GoToScheduler =>
           new Command(() =>
           {
               //Shell.Current.GoToAsync("AdminPageChoice/AdminPageScheduler");
               Shell.Current.GoToAsync(nameof(PageScheduler));
           });

        public ICommand SaveDatabaseInCSVFile =>
           new Command(async() =>
           {
               bool answer = await App.Current.MainPage.DisplayAlert("Backup", $"Wenn Sie fortfahren, dann werden die Daten im Verzeichnis '{pathWindows}' geschrieben. Wenn sich dort schon ein Backup befindet, wird dieses überschrieben. Möchten Sie fortfahren? Dieser Vorgang kann nicht rückgängig gemacht werden.", "Ja", "Nein");
               if (!answer) return;

               List<Employee> employees = App.EmployeeRepo.GetItems();
               List<Timetracking> timetracking = App.TimetrackingRepo.GetItems();
               ToCSV(employees, timetracking);

               await App.Current.MainPage.DisplayAlert("Backup", "Backup wurde erfolgreich erstellt.", "OK");
           });

        public ICommand LoadDatabaseFromCSVFile =>
           new Command(async(grid) =>
           {
               var answer = await App.Current.MainPage.DisplayAlert("Achtung", $"Möchten Sie wirklich die Datenbank mit den Daten im Verzeichnis '{pathWindows}' überschreiben? Dieser Vorgang kann nicht rückgängig gemacht werden.", "Ja","Nein");               
               if (!answer) return;

               var elements = (Grid)grid;
               //SfBusyIndicator busy = (SfBusyIndicator)grid.
               SfBusyIndicator busy = elements.FindByName("busy") as SfBusyIndicator;
               if (busy == null)
                   return;

               busy.IsRunning = true;   //Funktioniert leider nicht, da die Methode FromCSV nicht asynchron ausgeführt wird...
               FromCSV();
               busy.IsRunning = false;
               await App.Current.MainPage.DisplayAlert("", "Die Übertragung ist abgeschlossen.", "OK");
           });

        


        

        /// <summary>
        /// Überprüfe, ob der Code mit Android oder Windows gestartet wurde und passe dementsprechend den Pfad an.
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

        private string ToCSVEmployee(Employee employee)
        {
            //return employee.Id + ";" + employee.Username + ";" + employee.Role + ";" + employee.WorkingHoursPerWeek + ";" + employee.Password;
            return employee.Id + ";" + employee.Username + ";" + employee.Firstname + ";" + employee.Lastname + ";" + employee.Street + ";" + employee.PostalCode + ";" + employee.City + ";" + employee.Country + ";" + employee.Birthday
                 + ";" + employee.EMail + ";" + employee.Password + ";" + employee.WorkingHoursPerWeek + ";" + employee.IsResetPassword + ";" + employee.Role;
        }

        private string ToCSVTimetracking(Timetracking timetracking)
        {
            return timetracking.Id + ";" + timetracking.StartTime + ";" + timetracking.EndTime + ";" + timetracking.Subject + ";" + timetracking.WorkingTime + ";" + timetracking.IsCurrentlyWorking + ";" + timetracking.EmployeeID;
        }

        private void ToCSV(List<Employee> employees, List<Timetracking> timetrackingList)
        {
            path = DeterminePath();
            try
            {
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (StreamWriter sw = new(path + filenameEmployee))
                {
                    foreach (Employee employee in employees)
                    {
                        sw.WriteLine(ToCSVEmployee(employee));
                    }
                }

                using (StreamWriter sw = new(path + filenameTimetracking))
                {
                    foreach (Timetracking timetracking in timetrackingList)
                    {
                        sw.WriteLine(ToCSVTimetracking(timetracking));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehlermeldung: " + ex.Message);
            }
        }

        private void FromCSV()
        {
            path = DeterminePath();

            App.EmployeeRepo.DeleteTable();
            App.TimetrackingRepo.DeleteTable();

            List<Timetracking> timetrackinglist = new List<Timetracking>();
            try
            {
                using (StreamReader sr = new StreamReader(path + filenameTimetracking))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] parts = sr.ReadLine().Split(';');

                        var timetracking = new Timetracking()
                        {
                            Id = Convert.ToInt32(parts[0]),
                            StartTime = Convert.ToDateTime(parts[1]),
                            EndTime = Convert.ToDateTime(parts[2]),
                            Subject = Convert.ToString(parts[3]),
                            WorkingTime = TimeSpan.Parse(parts[4]),
                            IsCurrentlyWorking = Convert.ToBoolean(parts[5]),
                            EmployeeID = Convert.ToInt32(parts[6])
                        };

                        timetrackinglist.Add(timetracking);
                        App.TimetrackingRepo.SaveItemWithChildren(timetracking);
                    }
                }

                using (StreamReader sr = new StreamReader(path + filenameEmployee))
                {
                    List<Employee> employees = new List<Employee>();

                    while (!sr.EndOfStream)
                    {
                        string[] parts = sr.ReadLine().Split(';');
                        Role role = (Role)Enum.Parse(typeof(Role), parts[13]);
                        int iDEmployee = Convert.ToInt32(parts[0]);

                        List<Timetracking> timetrackingsForThisUser = new List<Timetracking>();
                        foreach (var timetracking in timetrackinglist)
                        {
                            if (timetracking.EmployeeID == iDEmployee)
                            {
                                timetrackingsForThisUser.Add(timetracking);
                            }
                        }

                        var employee = new Employee()
                        {
                            Id = iDEmployee,
                            Username = Convert.ToString(parts[1]),
                            Firstname = Convert.ToString(parts[2]),
                            Lastname= Convert.ToString(parts[3]),
                            Street = Convert.ToString(parts[4]),
                            PostalCode = Convert.ToString(parts[5]),
                            City = Convert.ToString(parts[6]),
                            Country = Convert.ToString(parts[7]),
                            Birthday = Convert.ToString(parts[8]),
                            EMail= Convert.ToString(parts[9]),
                            Password = Convert.ToString(parts[10]),
                            WorkingHoursPerWeek = Convert.ToInt32(parts[11]),
                            IsResetPassword= Convert.ToBoolean(parts[12]),
                            Role = role, //13
                            Timetracking = timetrackingsForThisUser
                        };

                        App.EmployeeRepo.SaveItemWithChildren(employee);

                        //employees.Add(new Employee() { Id = iDEmployee, Username = Convert.ToString(parts[1]), Password = Convert.ToString(parts[2]), WorkingHoursPerWeek = Convert.ToInt32(parts[3]), Role = role, Timetracking = timetrackingsForThisUser });
                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehlermeldung: " + ex.Message);
            }
        }

    }
}
