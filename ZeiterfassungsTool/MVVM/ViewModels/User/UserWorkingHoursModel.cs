using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.User
{
    public class UserWorkingHoursModel
    {
        public double HoursToWorkThisMonth { get; set; }

        public double WorkingHours { get; set; }

        public Color Color { get; set; }

        public UserWorkingHoursModel()
        {
            var employee = SaveLoginStatus.WhoIsLoggedIn[0];

            double workingHoursPerDay = employee.WorkingHoursPerWeek / 5;

            HoursToWorkThisMonth = GetWorkingDaysPerMonth(workingHoursPerDay);

            WorkingHours = WorkingHoursThisMonth();
        }

        private double GetWorkingDaysPerMonth(double workingHoursPerDay)
        {
            //DateTime dt = new DateTime();

            //int days = 0;

            //var year = 2023;
            //var month = 3;

            ////Anzahl der Tage feststellen
            //int i = DateTime.DaysInMonth(year, month);

            //for (int counter = 1; counter <= i; counter++)
            //{
            //    dt = new DateTime(year, month, counter);
            //    //Wenn der Tag kein Samstag oder Sontag ist zähle!
            //    if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
            //    {
            //        days++;
            //    }
            //}

            double workingHoursPerMonth = workingHoursPerDay * 5 * 4.3;

            return workingHoursPerMonth;
        }

        private double WorkingHoursThisMonth()
        {
            double hours = 0;
            var currentMonth = DateTime.Now.Month;  

            var employee = SaveLoginStatus.WhoIsLoggedIn[0];

            foreach (var timetracking in employee.Timetracking)
            {
                if (timetracking.Subject == "Urlaub" || timetracking.Subject == "Krank")
                    continue;

                if (timetracking.StartTime.Month == currentMonth && timetracking.EndTime.Month == currentMonth)
                {
                    TimeSpan timeSpan = timetracking.EndTime - timetracking.StartTime;

                    hours += timeSpan.TotalHours;
                    
                }
                else if (timetracking.StartTime.Month == currentMonth)
                {
                    TimeOnly timeInTimetrackingStart = TimeOnly.FromDateTime(timetracking.StartTime);
                    TimeOnly endOfDay = new TimeOnly(0, 0);

                    TimeSpan difference = endOfDay - timeInTimetrackingStart;                // Wenn der Start von der Aufzeichnung am heutigen Datum ist, aber das Ende der Aufzeichnung nicht, dann kann es nur bedeuten, dass über die Nacht hinweg bis zum nächsten Tag gearbeitet wurde. Da aber nur die Zeit vom heutigen Tag angezeigt werden soll, muss die Differenz zwischen 0 Uhr und dem Startzeitpunkt der Aufzeichnung ermittelt werden und zum Schluss dazuaddiert werden.     

                    hours += difference.TotalHours;
                }
                else if (timetracking.EndTime.Month == currentMonth)
                {
                    TimeOnly timeInTimetrackingEnd = TimeOnly.FromDateTime(timetracking.EndTime);
                    TimeOnly startOfDay = new TimeOnly(0, 0);

                    TimeSpan difference = timeInTimetrackingEnd - startOfDay;

                    hours += difference.TotalHours;
                }
            }

            if (hours < HoursToWorkThisMonth)
            {
                Color = Colors.Red;
            }
            else
            {
                hours -= HoursToWorkThisMonth;
                Color = Colors.Green;
            }

            return hours;
        }

        public ICommand BackButton =>
         new Command(() =>
         {
             Shell.Current.GoToAsync("UserWorkingHours/UserPage");
         });
    }
}
