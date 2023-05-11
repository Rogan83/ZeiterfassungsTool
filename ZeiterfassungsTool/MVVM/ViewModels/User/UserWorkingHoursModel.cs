using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.Models;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.User
{
    [AddINotifyPropertyChangedInterface]
    public class UserWorkingHoursModel
    {

        #region Properties

        public MySQLModels.Employee Employee { get; set; }

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
        

        public string OvertimeOrMinusHoursTotalText { get; set; }

        public string Percent { get; set; }
        public string OvertimeOrMinusHoursText { get; set; }



        public double TargetHoursPerMonth { get; set; }

        public double WorkingHours { get; set; }
        public double ShapePointer { get; set; }

        public double OvertimeOrMinusHours { get; set; }
        public double VacationDaysLeft { get; set; }


        public double OvertimeTotal { get; set; }

        public Color ColorOvertimeTotal { get; set; }
        public Color ColorOvertimeOrHoursLeft { get; set; }

        public Color ColorRadial { get; set; }
        public Color ColorPointer { get; set; }

        #endregion


        public UserWorkingHoursModel()
        {
            Employee = Login.WhoIsLoggedIn[0];  
            
            Month = DateTime.Now.Month.ToString();
            Year = DateTime.Now.Year.ToString();

            ColorRadial = Colors.Red;
            ColorPointer = Colors.DarkRed;

            SetWorkingHours();
        }

        //private double GetWorkingDaysPerMonth(double workingHoursPerDay)
        //{
        //    //DateTime dt = new DateTime();

        //    //int days = 0;

        //    //var year = 2023;
        //    //var month = 3;

        //    ////Anzahl der Tage feststellen
        //    //int i = DateTime.DaysInMonth(year, month);

        //    //for (int counter = 1; counter <= i; counter++)
        //    //{
        //    //    dt = new DateTime(year, month, counter);
        //    //    //Wenn der Tag kein Samstag oder Sontag ist zähle!
        //    //    if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
        //    //    {
        //    //        days++;
        //    //    }
        //    //}

        //   
        //}


        private async void SetColorsAndLabels(double workingHoursThisMonth)
        {
            var timetracking = await MySQLMethods.GetTimetracking(Employee.Id);

            //double isHoursTotal = CalculateHours.CalculateIsHoursTotal(Employee.Timetracking);
            double isHoursTotal = CalculateHours.CalculateIsHoursTotal(timetracking);
            //double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(TargetHoursPerMonth, Employee.Timetracking);
            double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(TargetHoursPerMonth, timetracking);
            OvertimeTotal = isHoursTotal - targetHoursTotal;

            TargetHoursPerMonth = Employee.WorkingHoursPerWeek * 4.3;
            //CalculateHours.CalculateVacationHoursTotal();

            if (OvertimeTotal >= 0)
            {
                ColorOvertimeTotal = Colors.DarkGreen;
                OvertimeOrMinusHoursTotalText = "Überstunden insgesamt: ";
            }
            else
            {
                ColorOvertimeTotal = Colors.DarkRed;
                OvertimeOrMinusHoursTotalText = "Minusstunden insgesamt: ";
                OvertimeTotal = OvertimeTotal * -1;
            }


            VacationDaysLeft = (CalculateHours.CalculateVacationHoursTotal(Employee, timetracking) - CalculateHours.CalculateTakenVacationHoursTotal(timetracking)) / 8;

           

            if (workingHoursThisMonth < TargetHoursPerMonth)
            {
                //Minusstunden
                ColorRadial = Colors.Red;
                ColorPointer = Colors.Red;
                ColorOvertimeOrHoursLeft = Colors.DarkRed;

                OvertimeOrMinusHoursText = "Minusstunden: ";
                OvertimeOrMinusHours = (int)(TargetHoursPerMonth - workingHoursThisMonth);



                ShapePointer = workingHoursThisMonth;
            }
            else
            {
                ColorRadial = Colors.Green;
                ColorPointer = Colors.DarkGreen;
                ColorOvertimeOrHoursLeft = Colors.Green;

                OvertimeOrMinusHoursText = "Überstunden: ";
                OvertimeOrMinusHours = (int)(workingHoursThisMonth - TargetHoursPerMonth);

                ShapePointer = OvertimeOrMinusHours;
            }

            var percent = (int)(workingHoursThisMonth / TargetHoursPerMonth * 100);
            Percent = $"{percent} %";

        }

       

        //private double WorkingHoursThisMonth()
        //{
            
        //    double hours = 0;
        //    int currentMonth = 0;
        //    if (Month != null)
        //    {
        //        if(Month.Length == 0)
        //        {
        //            App.Current.MainPage.DisplayAlert("Ungültige Eingabe", "Sie müssen min. 1 Zeichen im Feld Monat eingeben", "OK");
        //            return 0;
        //        }
        //        currentMonth = Int32.Parse(Month);      //DateTime.Now.Month;
        //    }
        //    //
        //    int currentYear = 0;
        //    if (Year != null)
        //    {
        //        if (Year.Length < 4)
        //        {
        //            App.Current.MainPage.DisplayAlert("Ungültige Eingabe", "Sie müssen min. 4 Zeichen im Feld Jahr eingeben", "OK");
        //            return 0;
        //        }
        //        currentYear = Int32.Parse(Year);        //DateTime.Now.Year;
        //    }

        //    var employee = SaveLoginStatus.WhoIsLoggedIn[0];

        //    foreach (var timetracking in employee.Timetracking)
        //    {
        //        if (timetracking.Subject == "Urlaub" || timetracking.Subject == "Krank")
        //            continue;

        //        if (timetracking.StartTime.Month == currentMonth && timetracking.EndTime.Month == currentMonth && timetracking.StartTime.Year == currentYear && timetracking.EndTime.Year == currentYear)
        //        {
        //            TimeSpan timeSpan = timetracking.EndTime - timetracking.StartTime;

        //            hours += timeSpan.TotalHours;

        //        }
        //        else if (timetracking.StartTime.Month == currentMonth && timetracking.StartTime.Year == currentYear)
        //        {
        //            TimeOnly timeInTimetrackingStart = TimeOnly.FromDateTime(timetracking.StartTime);
        //            TimeOnly endOfDay = new TimeOnly(0, 0);

        //            TimeSpan difference = endOfDay - timeInTimetrackingStart;                // Wenn der Start von der Aufzeichnung am heutigen Datum ist, aber das Ende der Aufzeichnung nicht, dann kann es nur bedeuten, dass über die Nacht hinweg bis zum nächsten Tag gearbeitet wurde. Da aber nur die Zeit vom heutigen Tag angezeigt werden soll, muss die Differenz zwischen 0 Uhr und dem Startzeitpunkt der Aufzeichnung ermittelt werden und zum Schluss dazuaddiert werden.     

        //            hours += difference.TotalHours;
        //        }
        //        else if (timetracking.EndTime.Month == currentMonth && timetracking.EndTime.Year == currentYear)
        //        {
        //            TimeOnly timeInTimetrackingEnd = TimeOnly.FromDateTime(timetracking.EndTime);
        //            TimeOnly startOfDay = new TimeOnly(0, 0);

        //            TimeSpan difference = timeInTimetrackingEnd - startOfDay;

        //            hours += difference.TotalHours;
        //        }
        //    }

        //    if (hours < TargetHoursPerMonth)
        //    {
        //        ColorRadial = Colors.Red;
        //        ColorPointer = Colors.Red;

        //        Overtime = 0;
        //        ShapePointer = hours;
        //        HoursLeft = (int)(TargetHoursPerMonth - hours);
        //    }
        //    else
        //    {
        //        ColorRadial = Colors.Green;
        //        ColorPointer = Colors.DarkGreen;

        //        Overtime = (int)(hours - TargetHoursPerMonth);
        //        ShapePointer = Overtime;
        //        HoursLeft = 0;
        //    }

        //    var percent = (int)(hours / TargetHoursPerMonth * 100);
        //    Percent = $"{percent}%";

        //    return (int)hours;
        //}

        //private double CalculateOvertimeTotal()
        //{
        //    var employee = SaveLoginStatus.WhoIsLoggedIn[0];

        //    double workingHoursTotal = 0;
        //    double overtimeTotal = 0;
        //    double targetHoursTotal = 0;

        //    int countMonths = 0;
        //    double workingHoursPerMonth = employee.WorkingHoursPerWeek * 4.3f;

        //    List<string> dates = new();           

        //    foreach (var timetracking in employee.Timetracking)
        //    {
        //        if (timetracking.Subject == "Krank" || timetracking.Subject == "Urlaub")
        //            continue;

        //        workingHoursTotal += (timetracking.EndTime - timetracking.StartTime).TotalHours;

        //        var date = timetracking.StartTime.Month.ToString() + timetracking.StartTime.Year.ToString();
        //        if (!dates.Contains(date))
        //            dates.Add(date);

        //    }

        //    countMonths = dates.Count;
        //    targetHoursTotal = countMonths * workingHoursPerMonth;

        //    overtimeTotal = workingHoursTotal - targetHoursTotal;

        //    if (overtimeTotal >= 0)
        //    {
        //        ColorOvertimeTotal = Colors.DarkGreen;
        //    }
        //    else
        //    {
        //        ColorOvertimeTotal = Colors.DarkRed;
        //    }


        //    return Math.Round(overtimeTotal);
        //}

        public ICommand BackButton =>
         new Command(() =>
         {
             Shell.Current.GoToAsync("UserWorkingHours/UserPage");
         });

        public ICommand ShowWorkingHours =>
         new Command(() =>
         {
             SetWorkingHours();
         });

        private async void SetWorkingHours()
        {
            var timetracking = await MySQLMethods.GetTimetracking(Employee.Id);
            //WorkingHours = CalculateHours.WorkingHoursThisMonth(Month, Year, Employee.Timetracking);
            WorkingHours = CalculateHours.WorkingHoursThisMonth(Month, Year, timetracking);

            //SetColorsAndLabels(WorkingHours);
            SetColorsAndLabels(WorkingHours);
        }
    }
}
