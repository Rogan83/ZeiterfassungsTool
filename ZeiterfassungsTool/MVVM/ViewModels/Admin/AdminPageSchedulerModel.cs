using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class AdminPageSchedulerModel
    {

        public ICommand BackButton =>
          new Command(() =>
          {
              Shell.Current.GoToAsync("AdminPageScheduler/AdminPageChoice");
          });
    }
}
