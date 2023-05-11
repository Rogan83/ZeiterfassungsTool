
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Gauges;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Models;


namespace ZeiterfassungsTool.StaticClasses
{
    public static class CalculateHours
    {
        //public static double WorkingHoursThisMonth(string month, string year, List<Timetracking> timetrackingList)
        public static double WorkingHoursThisMonth(string month, string year, List<MySQLModels.Timetracking> timetrackingList)
        {
            double hours = 0;
            int currentMonth = 0;
            if (month != null)
            {
                if (month.Length == 0)
                {
                    App.Current.MainPage.DisplayAlert("Ungültige Eingabe", "Sie müssen min. 1 Zeichen im Feld Monat eingeben", "OK");
                    return 0;
                }
                currentMonth = Int32.Parse(month);      
            }
            //
            int currentYear = 0;
            if (year != null)
            {
                if (year.Length < 4)
                {
                    App.Current.MainPage.DisplayAlert("Ungültige Eingabe", "Sie müssen min. 4 Zeichen im Feld Jahr eingeben", "OK");
                    return 0;
                }
                currentYear = Int32.Parse(year);        
            }

            foreach (var timetracking in timetrackingList)
            {
                if (timetracking.Subject == "Urlaub" || timetracking.Subject == "Krank")
                    continue;
                // Wenn der Start- und Endzeitpunkt im selben Monat und Jahr sind ...
                if (timetracking.StartTime.Month == currentMonth && timetracking.EndTime.Month == currentMonth && timetracking.StartTime.Year == currentYear && timetracking.EndTime.Year == currentYear)
                {
                    TimeSpan timeSpan = timetracking.EndTime - timetracking.StartTime;

                    hours += timeSpan.TotalHours;

                }
                // Wenn nur der Startzeitpunkt im selben Monat und Jahr sind ...
                else if (timetracking.StartTime.Month == currentMonth && timetracking.StartTime.Year == currentYear)
                {
                    TimeOnly timeInTimetrackingStart = TimeOnly.FromDateTime(timetracking.StartTime);
                    TimeOnly endOfDay = new TimeOnly(0, 0);
                    // ... dann bedeutet das, dass (normalerweise) der Endzeitpunkt mindestens 1 Monat später liegt, also dass z.B. über die Nacht vom 31.01 von 20 Uhr bis zum 01.02 um 6 Uhr gearbeitet wurde. Da aber nur die Summe von den Arbeitszeiten von einen Monat bestimmt werden soll, darf nicht die Differenz zwischen End- udn Startzeitpunkt ermitteltbestimme nur die Differenz zwischen Ende und Startzeitpunkt ermittelt werden, sondern zwischen dem Ende des Tages und dem Start.
                    TimeSpan difference = endOfDay - timeInTimetrackingStart;                // Wenn der Start von der Aufzeichnung am heutigen Datum ist, aber das Ende der Aufzeichnung nicht, dann kann es nur bedeuten, dass über die Nacht hinweg bis zum nächsten Tag gearbeitet wurde. Da aber nur die Zeit vom heutigen Tag angezeigt werden soll, muss die Differenz zwischen 0 Uhr und dem Startzeitpunkt der Aufzeichnung ermittelt werden und zum Schluss dazuaddiert werden.     

                    hours += difference.TotalHours;
                }
                // ... und wenn nur der Endzeitpunkt im selben Jahr und Monat sind ...
                else if (timetracking.EndTime.Month == currentMonth && timetracking.EndTime.Year == currentYear)
                {
                    TimeOnly timeInTimetrackingEnd = TimeOnly.FromDateTime(timetracking.EndTime);
                    TimeOnly startOfDay = new TimeOnly(0, 0);

                    TimeSpan difference = timeInTimetrackingEnd - startOfDay;

                    hours += difference.TotalHours;
                }
            }

            return Math.Round(hours);
        }

        //public static double CalculateTargetHoursTotal(double targetHoursPerMonth, List<Timetracking> timetrackingList)
        public static double CalculateTargetHoursTotal(double targetHoursPerMonth, List<MySQLModels.Timetracking> timetrackingList)
        {
            double workingHoursTotal = 0;
            double targetHoursTotal = 0;

            int countMonths = 0;

            List<string> dates = new();

            foreach (var timetracking in timetrackingList)
            {
                if (timetracking.Subject == "Urlaub" || timetracking.Subject == "Krank")
                    continue;

                workingHoursTotal += (timetracking.EndTime - timetracking.StartTime).TotalHours;

                var date = timetracking.StartTime.Month.ToString() + timetracking.StartTime.Year.ToString();
                if (!dates.Contains(date))
                    dates.Add(date);
            }

            countMonths = dates.Count;
            targetHoursTotal = countMonths * targetHoursPerMonth;

            //overtimeTotal = workingHoursTotal - targetHoursTotal;

            return Math.Round(targetHoursTotal);
        }

        public static double CalculateIsHoursTotal(List<MySQLModels.Timetracking> timetrackingList)
        {
            double workingHoursTotal = 0;
            foreach (var timetracking in timetrackingList)
            {
                if (timetracking.Subject == "Krank" || timetracking.Subject == "Urlaub")
                    continue;
                
                workingHoursTotal += (timetracking.EndTime - timetracking.StartTime).TotalHours;   
            }

            return Math.Round(workingHoursTotal);
        }

        //public static double CalculateVacationHoursTotal(Employee employee)
        public static double CalculateVacationHoursTotal(MySQLModels.Employee employee, List<MySQLModels.Timetracking> timetracking)
        {
            DateTime firstTime = new DateTime();
            DateTime lastTime = new DateTime();
            //if (employee.Timetracking.Count > 0)
            if (timetracking.Count > 0)  
            {
                //firstTime = employee.Timetracking[0].StartTime;
                firstTime = timetracking[0].StartTime;
                //lastTime = employee.Timetracking[0].EndTime;
                lastTime = timetracking[0].EndTime;
            }
             
            //foreach (var t in employee.Timetracking)
            foreach (var t in timetracking)
            {
                if ( firstTime > t.StartTime)
                {
                    firstTime = t.StartTime;
                }

                if ( lastTime < t.EndTime)
                {
                    lastTime = t.EndTime;
                }
            }
            
            var firstYear = firstTime.Year;
            var lastYear = lastTime.Year;

            var vacationTimeInHours = (lastYear - firstYear + 1) * employee.VacationDaysPerYear * 8;

            return vacationTimeInHours;
        }


        //public static double CalculateTakenVacationHoursTotal(List<Timetracking> timetrackingList)
        public static double CalculateTakenVacationHoursTotal(List<MySQLModels.Timetracking> timetrackingList)
        {
            if (timetrackingList == null)
                return 0;

            // Sortiert die Einträge heraus, wo Urlaub genommen wurde, ermittel von diesen die Zeitdifferenzen und summiere diese zusammen.
            //var totalHoursVacationList = timetrackingList.Where(tt => tt.Subject == "Urlaub").Select(x => (x.EndTime - x.StartTime).TotalHours).ToList();
            //double totalHoursVacation = 0;
            ////Nur wenn auch Zeiten ermittelt wurden, dann sollen diese zusammensummiert werden.
            //if (totalHoursVacationList.Count() != 0)
            //{
            //    bool isSetSum = false;
            //    // Zum Start wird die Summe mit den ersten Wert initialsiert, der gefunden wurde. Dieser darf aber nicht größer als 8 sein, weil am Tag nur 8 Stunden gearbeitet werden und dem Mitarbeiter sollen auch nur 8 Stunden als Urlaub angerechnet werden und nicht volle 24 Stunden, wenn er nicht arbeitet. Deswegen muss der "sum" Wert (Summe) auch einmalig überprüft werden, ob dieser größer als 8 ist. Wenn das der Fall ist, dann auf 8 setzen, weil dann schon den ganzen Tag lang Urlaub gemacht wurde.
            //    totalHoursVacation = totalHoursVacationList.Aggregate((sum, val) => {
            //        if (!isSetSum)
            //        {
            //            if (sum > 8)
            //                sum = 8;
            //            isSetSum = true;
            //        }
            //        //Dieser Wert darf aber nicht größer als 8 sein, weil am Tag nur 8 Stunden gearbeitet werden und dem Mitarbeiter sollen auch nur 8 Stunden als Urlaub angerechnet werden und nicht volle 24 Stunden, wenn er nicht arbeitet. Deswegen muss der "sum" Wert(Summe) auch einmalig überprüft werden, ob dieser größer als 8 ist.Wenn das der Fall ist, dann auf 8 setzen, weil dann schon den ganzen Tag lang Urlaub gemacht wurde.
            //        if (val > 8)
            //        {
            //            val = 8;
            //        }
            //        return (sum + val);
            //    });

            //}
            ////var test = timetrackingList.Where(employee => employee.Subject == "Urlaub");
            ////var count = test.Count();
            //return totalHoursVacation;                  // Wenn z.B. 3 Tage lang Urlaub genommen wurde, dann wird eine Differenz von 3 * 24 Stunden ermittelt. Da aber pro Tag nur 8 Stunden gearbeitet werden, sollen als Urlaub auch nur 8 Stunden, also 1/3 der Zeit als Urlaub verbucht werden, in den man nicht da ist. Deswegen muss die gesamte Zeit durch 3 geteilt werden.


            double sum = 0;
            foreach (var t in timetrackingList)
            {
                if (t.Subject != "Urlaub")
                    continue;

                var startday = t.StartTime.Day;
                var endday = t.EndTime.Day;

                var startTimeHour = t.StartTime.Hour; 
                var endTimeHour = t.EndTime.Hour;

                var startTimeMinute = t.StartTime.Minute;
                var endTimeMinute = t.EndTime.Minute;

                var takenvacationHours = (t.EndTime - t.StartTime).Days * 8 + (endTimeHour - startTimeHour) + Math.Round((endTimeMinute - startTimeMinute)/60f);

                sum += takenvacationHours;

            }
            return sum;
        }
    }
}
