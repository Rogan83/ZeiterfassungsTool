using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Abstraction;

namespace ZeiterfassungsTool.Models
{
    public class Timetracking : TableData
    {
        //(id int primary key auto_increment, workDate date, workBegin time, workEnd time, pause time, employeeID int, foreign key (employeeID) References timetracking(id));

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Subject { get; set; } = String.Empty;              //Hier kann beschrieben werden, was heute gemacht wurde. WICHTIG: vorher Initialisieren, so dass es nicht NULL ist, sonst stürzt das Programm ab, wenn man es für Android kompiliert. Bei WIndows funktioniert es komischerweise aber.
        public TimeSpan WorkingTime { get; set; }

        public bool IsCurrentlyWorking { get; set; }

        [ForeignKey(typeof(Employee))]
        public int EmployeeID { get; set; }

    }
}
