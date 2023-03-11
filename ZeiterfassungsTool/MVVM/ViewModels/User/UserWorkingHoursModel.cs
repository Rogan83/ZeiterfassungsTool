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

        public string OvertimeOrMinusHours { get; set; }

        public string Percent { get; set; }

        public double TargetHoursPerMonth { get; set; }

        public double WorkingHours { get; set; }
        public double ShapePointer { get; set; }

        public double HoursLeft { get; set; }

        public double Overtime { get; set; }

        public double OvertimeTotal { get; set; }

        public Color ColorOvertimeTotal { get; set; }

        public Color ColorRadial { get; set; }
        public Color ColorPointer { get; set; }

        #endregion


        public UserWorkingHoursModel()
        {
            Month = DateTime.Now.Month.ToString();
            Year = DateTime.Now.Year.ToString();

            ColorRadial = Colors.Red;
            ColorPointer = Colors.DarkRed;

            var employee = Login.WhoIsLoggedIn[0];

            TargetHoursPerMonth = employee.WorkingHoursPerWeek * 4.3f;

            WorkingHours = CalculateHours.WorkingHoursThisMonth(Month, Year, employee.Timetracking);

            double isHoursTotal = CalculateHours.CalculateIsHoursTotal(employee.Timetracking);
            double targetHoursTotal = CalculateHours.CalculateTargetHoursTotal(TargetHoursPerMonth, employee.Timetracking);
            double overtimeTotal = isHoursTotal - targetHoursTotal;


            //var overtimeTotal = CalculateOvertimeTotal();

            //Zeige an, ob in der Summe Minus- oder Überstunden angefallen sind.
            //if (overtimeTotal >= 0)
            //{
            //    OvertimeOrMinusHours = "Überstunden insgesamt:";
            //}
            //else
            //{
            //    OvertimeOrMinusHours = "Minusstunden insgesamt:";
            //    overtimeTotal = overtimeTotal * -1;
            //}

            //OvertimeTotal = overtimeTotal;

            //WorkingHoursThisMonth();

            SetColorsAndLabels(WorkingHours, overtimeTotal);
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


        private void SetColorsAndLabels(double workingHoursThisMonth, double overtimeTotal)
        {
            SetColorsAndLabels(workingHoursThisMonth);

            if (overtimeTotal >= 0)
            {
                ColorOvertimeTotal = Colors.DarkGreen;
                OvertimeOrMinusHours = "Überstunden insgesamt:";
            }
            else
            {
                ColorOvertimeTotal = Colors.DarkRed;
                OvertimeOrMinusHours = "Minusstunden insgesamt:";
                OvertimeTotal = overtimeTotal * -1;
            }
        }

        private void SetColorsAndLabels(double workingHoursThisMonth)
        {
            if (workingHoursThisMonth < TargetHoursPerMonth)
            {
                ColorRadial = Colors.Red;
                ColorPointer = Colors.Red;

                Overtime = 0;
                ShapePointer = workingHoursThisMonth;
                HoursLeft = (int)(TargetHoursPerMonth - workingHoursThisMonth);
            }
            else
            {
                ColorRadial = Colors.Green;
                ColorPointer = Colors.DarkGreen;

                Overtime = (int)(workingHoursThisMonth - TargetHoursPerMonth);
                ShapePointer = Overtime;
                HoursLeft = 0;
            }
            var percent = (int)(workingHoursThisMonth / TargetHoursPerMonth * 100);
            Percent = $"{percent}%";
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
             //WorkingHours = WorkingHoursThisMonth();
             var employee = Login.WhoIsLoggedIn[0];
             WorkingHours = CalculateHours.WorkingHoursThisMonth(Month, Year, employee.Timetracking);

             SetColorsAndLabels(WorkingHours);
         });
    }
}
