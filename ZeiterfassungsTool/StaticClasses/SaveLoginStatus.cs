﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Models;

namespace ZeiterfassungsTool.StaticClasses
{
    public static class SaveLoginStatus
    {
        public static List<Employee> WhoIsLoggedIn { get; set; } = new List<Employee>() { new Employee()};
    }
}
