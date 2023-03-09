using Syncfusion.Maui.Gauges;
using System;
using System.Collections.Generic;
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

                if (timetracking.StartTime.Month == currentMonth && timetracking.EndTime.Month == currentMonth && timetracking.StartTime.Year == currentYear && timetracking.EndTime.Year == currentYear)
                {
                    TimeSpan timeSpan = timetracking.EndTime - timetracking.StartTime;

                    hours += timeSpan.TotalHours;

                }
                else if (timetracking.StartTime.Month == currentMonth && timetracking.StartTime.Year == currentYear)
                {
                    TimeOnly timeInTimetrackingStart = TimeOnly.FromDateTime(timetracking.StartTime);
                    TimeOnly endOfDay = new TimeOnly(0, 0);

                    TimeSpan difference = endOfDay - timeInTimetrackingStart;                // Wenn der Start von der Aufzeichnung am heutigen Datum ist, aber das Ende der Aufzeichnung nicht, dann kann es nur bedeuten, dass über die Nacht hinweg bis zum nächsten Tag gearbeitet wurde. Da aber nur die Zeit vom heutigen Tag angezeigt werden soll, muss die Differenz zwischen 0 Uhr und dem Startzeitpunkt der Aufzeichnung ermittelt werden und zum Schluss dazuaddiert werden.     

                    hours += difference.TotalHours;
                }
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
    }
}
