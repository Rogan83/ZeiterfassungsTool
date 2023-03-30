using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeiterfassungsTool.Enumerations;

namespace ZeiterfassungsTool.MVVM.Converters
{
    public class EnglishToGermanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> roles = new List<string>();
            try
            {
                List<Role> options = (List<Role>)value;
           
                foreach (var option in options)
                {
                    if (option == Role.Admin)
                        roles.Add("Administrator");
                    else if (option == Role.Management)
                        roles.Add("Geschäftsleitung");
                    else
                        roles.Add("Mitarbeiter");
                }

                return roles;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
