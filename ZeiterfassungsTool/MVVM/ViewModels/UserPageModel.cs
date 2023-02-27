using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    public class UserPageModel
    {
        
            public ICommand BackToMenu =>
               new Command(() =>
               {
                   Shell.Current.GoToAsync("UserPage/StartPage");
               });
    }
}
