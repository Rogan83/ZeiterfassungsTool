using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Models;

namespace ZeiterfassungsTool.Selectors
{
    public class EmployeeDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var timetracking = item as MySQLModels.Timetracking;
            if (timetracking == null)
            {
                Application.Current.Resources.TryGetValue("EmployeeStyleWork", out var employeeStyleWork);
                return employeeStyleWork as DataTemplate;
            }

            if(timetracking.Subject == "Krank")
            {
                Application.Current.Resources.TryGetValue("EmployeeStyleIll", out var employeeStyleIll);
                return employeeStyleIll as DataTemplate;
            }
            else if (timetracking.Subject == "Urlaub")
            {
                Application.Current.Resources.TryGetValue("EmployeeStyleHoliday", out var employeeStyleHoliday);
                return employeeStyleHoliday as DataTemplate;
            }
            else
            {
                Application.Current.Resources.TryGetValue("EmployeeStyleWork", out var employeeStyleWork);
                return employeeStyleWork as DataTemplate;
            }
            //return new DataTemplate();
        }
    }
}
