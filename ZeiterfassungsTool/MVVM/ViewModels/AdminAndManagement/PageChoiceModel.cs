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
           new Command(() =>
           {
               List<Employee> employees = App.EmployeeRepo.GetItems();
               List<Timetracking> timetracking = App.TimetrackingRepo.GetItems();
               ToCSV(employees, timetracking);
           });

        public ICommand LoadDatabaseInCSVFile =>
           new Command(() =>
           {
               FromCSV(); 
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
            return employee.Id + ";" + employee.Username + ";" + employee.Role + ";" + employee.WorkingHoursPerWeek + ";" + employee.Password;
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
                        Role role = (Role)Enum.Parse(typeof(Role), parts[2]);
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
                            Role = role,
                            WorkingHoursPerWeek = Convert.ToInt32(parts[3]),
                            Password = Convert.ToString(parts[4]),
                            Timetracking = timetrackingsForThisUser,
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
