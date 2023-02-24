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
        
        public DateTime Workbegin { get; set; }
        public DateTime Workend { get; set; }
        public string WorkingHours { get; set; }
        
        public int EmployeeID { get; set; }
        
    }
}
