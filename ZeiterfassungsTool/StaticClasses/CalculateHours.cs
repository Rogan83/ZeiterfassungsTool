
using Syncfusion.Maui.Gauges;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Models;


namespace ZeiterfassungsTool.StaticClasses
{
    public static class CalculateHours
    {
        public static double WorkingHoursThisMonth(string month, string year, List<Timetracking> timetrackingList)
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

        public static double CalculateTargetHoursTotal(double targetHoursPerMonth, List<Timetracking> timetrackingList)
        {
            double workingHoursTotal = 0;
            double targetHoursTotal = 0;

            int countMonths = 0;

            List<string> dates = new();

            foreach (var timetracking in timetrackingList)
            {
                if (timetracking.Subject == "Krank" || timetracking.Subject == "Urlaub")
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

        public static double CalculateIsHoursTotal(List<Timetracking> timetrackingList)
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

        public static double CalculateVacationHoursTotal(List<Timetracking> timetrackingList)
        {
            if (timetrackingList == null)
                return 0;

            // Sortiert die Einträge heraus, wo Urlaub genommen wurde, ermittel von diesen die Zeitdifferenzen und summiere diese zusammen.
            var totalHoursVacationList = timetrackingList.Where(employee => employee.Subject == "Urlaub").Select(x => (x.EndTime - x.StartTime).TotalHours);
            double totalHoursVacation = 0;
            //Nur wenn auch Zeiten ermittelt wurden, dann sollen diese zusammensummiert werden.
            if (totalHoursVacationList.Count() != 0)
            {
                totalHoursVacation = totalHoursVacationList.Aggregate((sum, val) => sum + val);
            }
            //var test = timetrackingList.Where(employee => employee.Subject == "Urlaub");
            //var count = test.Count();
            return totalHoursVacation;
        }
    }
}
