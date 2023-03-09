using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class AdminPageOvertimeModel
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

        public double BiggestNumber { get; set; } = 0;

        public bool rbChooseMonth { get; set; } = true;
        public bool rbTotal { get; set; }

        #endregion

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<EmployeeWorkingHours> EmployeesWorkingHours { get; set; } = new ObservableCollection<EmployeeWorkingHours>();

        public AdminPageOvertimeModel()
        {
            Employees = new ObservableCollection<Employee>(App.EmployeeRepo.GetItemsWithChildren());

            Month = DateTime.Now.Month.ToString();
            Year = DateTime.Now.Year.ToString();

            CreateEmployeeListForOneMonth();
        }

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(AdminPageChoice));
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
            List<EmployeeWorkingHours> employeesWorkingHours = new();

            foreach (var employee in Employees)
            {
                double targetHoursPerMonth = employee.WorkingHoursPerWeek * 4.3;
                if (targetHoursPerMonth > BiggestNumber) BiggestNumber = targetHoursPerMonth;
                double actualHoursThisMonth = CalculateHours.WorkingHoursThisMonth(Month, Year, employee.Timetracking);
                if (actualHoursThisMonth > BiggestNumber) BiggestNumber = actualHoursThisMonth;
                double overtime = actualHoursThisMonth - targetHoursPerMonth;
                if (overtime > BiggestNumber) BiggestNumber = overtime;

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

            EmployeesWorkingHours = new ObservableCollection<EmployeeWorkingHours>(employeesWorkingHours);
        }

        private void CreateEmployeeListTotalHours()
        {
            List<EmployeeWorkingHours> employeesWorkingHours = new();

            foreach (var employee in Employees)
            {
                double actualHoursTotal = CalculateHours.CalculateIsHoursTotal(employee.Timetracking);
                if (actualHoursTotal > BiggestNumber) BiggestNumber = actualHoursTotal;
                double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(employee.WorkingHoursPerWeek * 4.3, employee.Timetracking);
                if (targetHoursTotal > BiggestNumber) BiggestNumber = targetHoursTotal;
                double overtimeTotal = actualHoursTotal - targetHoursTotal;
                if (overtimeTotal > BiggestNumber) BiggestNumber = overtimeTotal;

                Color color;
                if (overtimeTotal >= 0)
                    color = Colors.Green;
                else
                {
                    overtimeTotal = overtimeTotal * -1;       // wandelt es in diesen Fall eig. in Minusstunden um, deswegen soll als Farbe auch Rot ausgewählt werden, um das zu verdeutlichen
                    color = Colors.Red;
                }

                employeesWorkingHours.Add(new EmployeeWorkingHours { Username = employee.Username, WorkingHours = actualHoursTotal, TargetHours = targetHoursTotal, Overtime = overtimeTotal, Color = color });
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
