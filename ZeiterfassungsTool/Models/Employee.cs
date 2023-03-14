using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Abstraction;
using ZeiterfassungsTool.Enumerations;

namespace ZeiterfassungsTool.Models
{
    public class Employee : TableData
    {
        
        public string Username { get; set; }  = string.Empty;
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Birthday { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; } = string.Empty;
        public int WorkingHoursPerWeek { get; set; } = 40;       //Legt fest, wie viele Stunden dieser Mitarbeiter pro Woche planmäßig arbeiten soll.
        public bool IsResetPassword { get; set; } = false;
        public string Salt { get; set; }
        public Role Role { get; set; }                        //Rollen: Benutzer, Geschäftsleitung oder Admin
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Timetracking> Timetracking { get; set; } = new List<Timetracking>();

        
    }
    
}
