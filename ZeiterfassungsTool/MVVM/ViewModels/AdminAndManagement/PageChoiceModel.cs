using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;

namespace ZeiterfassungsTool.MVVM.ViewModels.Admin
{
    public class PageChoiceModel
    {
        public PageChoiceModel() 
        {
        
        }

        public ICommand BackButton =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("PageChoice/LoginPage");
           });

        public ICommand GoToUserListView =>
           new Command(() =>
           {
               Shell.Current.GoToAsync("PageChoice/PageUserList");
           });

        public ICommand GoToAccountsListView =>
          new Command(() =>
          {
              Shell.Current.GoToAsync(nameof(PageAdminAndManagementManagement));
          });

        public ICommand GoToOvertimeView =>
           new Command(() =>
           {
               Shell.Current.GoToAsync(nameof(PageOvertime));
           });

        public ICommand GoToScheduler =>
           new Command(() =>
           {
               //Shell.Current.GoToAsync("AdminPageChoice/AdminPageScheduler");
               Shell.Current.GoToAsync(nameof(PageScheduler));
           });
    }
}
