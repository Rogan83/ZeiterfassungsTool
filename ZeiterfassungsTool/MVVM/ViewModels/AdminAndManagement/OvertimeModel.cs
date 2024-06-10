using PropertyChanged;
using Syncfusion.Maui.Gauges;
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

        public double PointerSize { get; set; } = 0;
        #endregion

        //public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        public ObservableCollection<MySQLModels.Employee> Employees { get; set; } = new ObservableCollection<MySQLModels.Employee>();
        public ObservableCollection<EmployeeWorkingHours> EmployeesHours { get; set; } = new ObservableCollection<EmployeeWorkingHours>();

        public OvertimeModel()
        {
            // Sucht nur die Accounts heraus, welche die Mitarbeiter Rechte haben.
            Month = DateTime.Now.Month.ToString();
            Year = DateTime.Now.Year.ToString();
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

        public async void CreateEmployeeListForOneMonth()
        {
            double biggestNumber = 0;       // Es soll die größte Zahl von allen Balken von allen Mitarbeitern gefunden werden, damit klar ist, wie groß der Maßstab ist, der unten angezeigt wird, da dieser min. so groß sein muss, wie der größte Balken von allen Mitarbeitern, damit dieser auch richtig dargestellt werden kann. 

            List<EmployeeWorkingHours> employeesHours = new();

            foreach (var employee in Employees)
            {
                var timetracking = await MySQLMethods.GetTimetracking(employee.Id);

                double targetHoursPerMonth = employee.WorkingHoursPerWeek * 4.3;
                if (targetHoursPerMonth > biggestNumber) biggestNumber = (int)targetHoursPerMonth;
                //double actualHoursThisMonth = CalculateHours.WorkingHoursThisMonth(Month, Year, employee.Timetracking);
                double actualHoursThisMonth = CalculateHours.WorkingHoursThisMonth(Month, Year, timetracking);
                if (actualHoursThisMonth > biggestNumber) biggestNumber = (int)actualHoursThisMonth;
                double overtime = actualHoursThisMonth - targetHoursPerMonth;
                if (overtime > biggestNumber) biggestNumber = (int)overtime;

                //double overtimeTotal = CalculateHours.CalculateTargetHoursTotal(targetHoursPerMonth, employee.Timetracking);
                double overtimeTotal = CalculateHours.CalculateTargetHoursTotal(targetHoursPerMonth, timetracking);

                Color color;
                if (overtime >= 0)
                    color = Colors.LightGreen;
                else
                {
                    overtime = overtime * -1;       // wandelt es in diesen Fall eig. in Minusstunden um, deswegen soll als Farbe auch Rot ausgewählt werden, um das zu verdeutlichen
                    color = Colors.Red;
                }

                employeesHours.Add(new EmployeeWorkingHours
                {
                    Username = employee.Username,
                    WorkingHours = actualHoursThisMonth,
                    TargetHours = targetHoursPerMonth,
                    Overtime = overtime,
                    ColorOvertime = color,
                    PointerSize = 0,
                    OffsetOvertime = 25,
                    OffsetTargetHours = 50,
                    OffsetWorkingHours = 75
                });
            }

            try
            {
                BiggestNumber = biggestNumber;               // in dem Augenblick, wo eine kleinere Zahl zugewiesen wird, kommt die Fehlermeldung: "Cannot access a disposed object." und es wird in den Catch Block gesprungen. Komischerweise weist es trotzdem den Wert zu und dieser wird dann auch genutzt. Dies passiert auf der Android Plattform, nicht bei Windows
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            EmployeesHours = new ObservableCollection<EmployeeWorkingHours>(employeesHours);
        }

        private async void CreateEmployeeListTotalHours()
        {
            double biggestNumber = 0;
            List<EmployeeWorkingHours> employeesWorkingHours = new();

            foreach (var employee in Employees)
            {
                var timetracking = await MySQLMethods.GetTimetracking(employee.Id);

                //double actualHoursTotal = CalculateHours.CalculateIsHoursTotal(employee.Timetracking);
                double actualHoursTotal = CalculateHours.CalculateIsHoursTotal(timetracking);
                if (actualHoursTotal > biggestNumber) biggestNumber = (int)actualHoursTotal;
                //double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(employee.WorkingHoursPerWeek * 4.3, employee.Timetracking);
                double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(employee.WorkingHoursPerWeek * 4.3, timetracking);
                if (targetHoursTotal > biggestNumber) biggestNumber = (int)targetHoursTotal;
                double overtimeTotal = actualHoursTotal - targetHoursTotal;
                if (Math.Abs(overtimeTotal) > biggestNumber) biggestNumber = Math.Abs((int)overtimeTotal);              // Weil overtimeTotal auch negativ sein kann, aber dieser später umgewandelt wird ins positive für die Minusstunden, muss, um die Minusstunden richtig anzeigen zu können, der absolute Wert genommen werden
                //double takenVacationHoursTotal = CalculateHours.CalculateTakenVacationHoursTotal(employee.Timetracking);
                double takenVacationHoursTotal = CalculateHours.CalculateTakenVacationHoursTotal(timetracking);
                if (takenVacationHoursTotal > biggestNumber) biggestNumber = (int)takenVacationHoursTotal;

                double vacationTimeInHours = CalculateHours.CalculateVacationHoursTotal(employee, timetracking);
                if (vacationTimeInHours > biggestNumber) biggestNumber = (int)vacationTimeInHours;

                double freeTimeTotal = vacationTimeInHours + overtimeTotal;
                if (freeTimeTotal > biggestNumber) biggestNumber = (int)freeTimeTotal;
                double freeTimeLeft = freeTimeTotal - takenVacationHoursTotal;


                Color colorOvertime;
                if (overtimeTotal >= 0)
                    colorOvertime = Colors.LightGreen;
                else
                {
                    overtimeTotal = overtimeTotal * -1;       // wandelt es in diesen Fall eig. in Minusstunden um, deswegen soll als Farbe auch Rot ausgewählt werden, um das zu verdeutlichen
                    colorOvertime = Colors.Red;
                }
                // Wenn ich hier die BiggestNumber direkt zuweise, dann kommt eine NaN Fehlermeldung. K.a. wieso
                employeesWorkingHours.Add(new EmployeeWorkingHours
                {
                    Username = employee.Username,
                    WorkingHours = actualHoursTotal,
                    TargetHours = targetHoursTotal,
                    Overtime = overtimeTotal,
                    TakenVacationHoursTotal = takenVacationHoursTotal,
                    ColorOvertime = colorOvertime,
                    VacationTimeTotal = vacationTimeInHours,
                    FreeTimeTotal = freeTimeTotal,
                    FreeTimeLeft = freeTimeLeft,
                    PointerSize = 20,
                    OffsetOvertime = 125,
                    OffsetTargetHours = 150,
                    OffsetWorkingHours = 175
                });
            }
            try
            {
                BiggestNumber = biggestNumber;      // in dem Augenblick, wo eine kleinere Zahl zugewiesen wird, kommt die Fehlermeldung: "Cannot access a disposed object." und es wird in den Catch Block gesprungen. Komischerweise weist es trotzdem den Wert zu und dieser wird dann auch genutzt. 
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception: " + e);
            }
            EmployeesHours = new ObservableCollection<EmployeeWorkingHours>(employeesWorkingHours);
        }
    }
    /// <summary>
    /// Die Klasse beinhaltet alle Eigenschaften, die für die Anzeige der vers. Balken notwendig ist.
    /// </summary>
    public class EmployeeWorkingHours
    {
        private double overtimeLeft;

        public string Username { get; set; }

        public double WorkingHours { get; set; }
        public double OffsetWorkingHours { get; set; }

        public double TargetHours { get; set; }
        public double OffsetTargetHours { get; set; }

        public double Overtime { get; set; }
        public double OffsetOvertime { get; set; }

        public double TakenVacationHoursTotal { get; set; }

        public double VacationTimeTotal { get; set; }
        public double FreeTimeTotal { get; set; }
        public double FreeTimeLeft { get; set; }

        public double PointerSize { get; set; }

        public Color ColorOvertime { get; set; }
    }
}
