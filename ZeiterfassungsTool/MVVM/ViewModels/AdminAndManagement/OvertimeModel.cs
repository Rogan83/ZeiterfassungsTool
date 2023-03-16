using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    [AddINotifyPropertyChangedInterface]
    public class OvertimeModel
    {
        #region Properties
       
        private string month;
        public string Month
        {
            get => month; set
            {
                bool parseSuccessful = Int32.TryParse(value, out int result);
                if (parseSuccessful)
                {
                    if (result > 12)
                        result = 12;
                    else if (result < 0)
                        result = 0;
                    value = result.ToString();

                }

                month = value;
            }
        }
        public string Year { get; set; }
        
        public double BiggestNumber { get; set; }

        public bool rbChooseMonth { get; set; } = true;
        public bool rbTotal { get; set; }

        #endregion

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<EmployeeWorkingHours> EmployeesWorkingHours { get; set; } = new ObservableCollection<EmployeeWorkingHours>();

        public OvertimeModel()
        {
            Employees = new ObservableCollection<Employee>(App.EmployeeRepo.GetItemsWithChildren().Where(x => x.Role == Enumerations.Role.User));        
            //var Allaccounts = new ObservableCollection<Employee>(App.EmployeeRepo.GetItemsWithChildren());

            //List<Employee> users = new List<Employee>();

            //foreach (var account in Allaccounts)
            //{
            //    if (account.Role == Enumerations.Role.User) 
            //    {
            //        users.Add(account);
            //    }
            //}
            //Employees = new ObservableCollection<Employee>(users);


            Month = DateTime.Now.Month.ToString();
            Year = DateTime.Now.Year.ToString();

            CreateEmployeeListForOneMonth();
        }

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(Choice));
           });

        public ICommand CreateEmployeeListCommand =>
           new Command(() =>
           {
               if (rbChooseMonth)
               {
                    CreateEmployeeListForOneMonth();
               }
               else
               {
                   CreateEmployeeListTotalHours();
               }
           });

        //public ICommand CreateEmployeeListTotalHours =>
        //    new Command(() =>
        //    {
        //        List<EmployeeWorkingHours> employeesWorkingHours = new();

        //        foreach (var employee in Employees) 
        //        {
        //            double actualHoursTotal = CalculateHours.CalculateHoursTotal(employee.Timetracking);
        //            if (actualHoursTotal > BiggestNumber) BiggestNumber = actualHoursTotal;
        //            double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(employee.WorkingHoursPerWeek * 4.3, employee.Timetracking);
        //            if (targetHoursTotal > BiggestNumber) BiggestNumber = targetHoursTotal;
        //            double overtimeTotal = actualHoursTotal - targetHoursTotal;
        //            if (overtimeTotal > BiggestNumber) BiggestNumber = overtimeTotal;

        //            employeesWorkingHours.Add(new EmployeeWorkingHours { Username = employee.Username, WorkingHours = actualHoursTotal, TargetHours = targetHoursTotal, Overtime = overtimeTotal });
        //        }
        //        EmployeesWorkingHours = new ObservableCollection<EmployeeWorkingHours>(employeesWorkingHours);
        //    });



        private void CreateEmployeeListForOneMonth()
        {
            double biggestNumber = 0;

            List<EmployeeWorkingHours> employeesWorkingHours = new();

            foreach (var employee in Employees)
            {
                double targetHoursPerMonth = employee.WorkingHoursPerWeek * 4.3;
                if (targetHoursPerMonth > biggestNumber) biggestNumber = (int)targetHoursPerMonth;
                double actualHoursThisMonth = CalculateHours.WorkingHoursThisMonth(Month, Year, employee.Timetracking);
                if (actualHoursThisMonth > biggestNumber) biggestNumber = (int)actualHoursThisMonth;
                double overtime = actualHoursThisMonth - targetHoursPerMonth;
                if (overtime > biggestNumber) biggestNumber = (int)overtime;

                double overtimeTotal = CalculateHours.CalculateTargetHoursTotal(targetHoursPerMonth, employee.Timetracking);

                Color color;
                if (overtime >= 0)
                    color = Colors.Green;
                else
                {
                    overtime = overtime * -1;       // wandelt es in diesen Fall eig. in Minusstunden um, deswegen soll als Farbe auch Rot ausgewählt werden, um das zu verdeutlichen
                    color = Colors.Red;
                }

                employeesWorkingHours.Add(new EmployeeWorkingHours { Username = employee.Username, WorkingHours = actualHoursThisMonth, TargetHours = targetHoursPerMonth, Overtime = overtime, Color = color });
            }

            try
            {
                BiggestNumber= biggestNumber;               // in dem Augenblick, wo eine kleinere Zahl zugewiesen wird, kommt die Fehlermeldung: "Cannot access a disposed object." und es wird in den Catch Block gesprungen. Komischerweise weist es trotzdem den Wert zu und dieser wird dann auch genutzt. 
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            EmployeesWorkingHours = new ObservableCollection<EmployeeWorkingHours>(employeesWorkingHours);
        }

        private void CreateEmployeeListTotalHours()
        {
            
            double biggestNumber = 0;
            List<EmployeeWorkingHours> employeesWorkingHours = new();

            foreach (var employee in Employees)
            {
                double actualHoursTotal = CalculateHours.CalculateIsHoursTotal(employee.Timetracking);
                if (actualHoursTotal > biggestNumber) biggestNumber = (int)actualHoursTotal;
                double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(employee.WorkingHoursPerWeek * 4.3, employee.Timetracking);
                if (targetHoursTotal > biggestNumber) biggestNumber = (int)targetHoursTotal;
                double overtimeTotal = actualHoursTotal - targetHoursTotal;
                if (overtimeTotal > biggestNumber) biggestNumber = (int)overtimeTotal;

                Color color;
                if (overtimeTotal >= 0)
                    color = Colors.Green;
                else
                {
                    overtimeTotal = overtimeTotal * -1;       // wandelt es in diesen Fall eig. in Minusstunden um, deswegen soll als Farbe auch Rot ausgewählt werden, um das zu verdeutlichen
                    color = Colors.Red;
                }
                // Wenn ich hier die BiggestNumber direkt zuweise, dann kommt eine NaN Fehlermeldung. K.a. wieso
                employeesWorkingHours.Add(new EmployeeWorkingHours { Username = employee.Username, WorkingHours = actualHoursTotal, TargetHours = targetHoursTotal, Overtime = overtimeTotal, Color = color });
            }
            try
            {
                BiggestNumber = biggestNumber;      // in dem Augenblick, wo eine kleinere Zahl zugewiesen wird, kommt die Fehlermeldung: "Cannot access a disposed object." und es wird in den Catch Block gesprungen. Komischerweise weist es trotzdem den Wert zu und dieser wird dann auch genutzt. 
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            EmployeesWorkingHours = new ObservableCollection<EmployeeWorkingHours>(employeesWorkingHours);
        }
    }

    public class EmployeeWorkingHours
    {
        public string Username { get; set; }
        public double WorkingHours { get; set; }
        public double TargetHours { get; set; }
        public double Overtime { get; set; }

        public Color Color { get; set; }
    }
}
