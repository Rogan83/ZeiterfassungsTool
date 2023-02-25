using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Abstraction;

namespace ZeiterfassungsTool.Models
{
    public class Employee : TableData
    {
        
        public string Firstname { get; set; }
        //public string lastname { get; set; }
        //public string street { get; set; }
        //public string postalCode { get; set; }
        //public string city { get; set; }
        //public string country { get; set; }
        //public string Birthday { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }                        //User, Geschäftsleitung oder Admin
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Timetracking> Timetracking { get; set; }

        
    }
    public enum Role
    {
        User,
        Management,
        Admin
    }
}
