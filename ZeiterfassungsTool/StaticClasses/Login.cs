using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Models;

namespace ZeiterfassungsTool.StaticClasses
{
    public static class Login
    {
        //public static List<Employee> WhoIsLoggedIn { get; set; } = new List<Employee>() { new Employee()};
        public static List<MySQLModels.Employee> WhoIsLoggedIn { get; set; } //= new List<MySQLModels.Employee>() { new MySQLModels.Employee()};
    }
}
