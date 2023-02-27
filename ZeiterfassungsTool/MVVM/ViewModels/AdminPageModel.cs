using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.MVVM.Views;

namespace ZeiterfassungsTool.MVVM.ViewModels
{
    public class AdminPageModel
    {
        public ICommand BackToMenu =>
           new Command(() => 
           {
               Shell.Current.GoToAsync("AdminPage/StartPage");
           });
    }
}
