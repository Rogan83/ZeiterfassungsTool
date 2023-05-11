using PropertyChanged;
using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    [AddINotifyPropertyChangedInterface]
    public class SchedulerModel
    {
        public ObservableCollection<SchedulerAppointment> SchedulerEvents { get; set; }

        public List<MySQLModels.Employee> Employees { get; set; }

        public SchedulerModel()
        {
            //SQLite
            //var employees = App.EmployeeRepo.GetItemsWithChildren();
        }


        public ICommand BackButton =>
          new Command(() =>
          {
              Shell.Current.GoToAsync("Scheduler/Choice");
          });
    }
}
