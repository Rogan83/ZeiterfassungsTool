using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.MVVM.Views.Admin;
using ZeiterfassungsTool.MVVM.Views.AdminAndManagement;
using ZeiterfassungsTool.MVVM.Views.User;

namespace ZeiterfassungsTool;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("StartPage", typeof(StartPage));
        Routing.RegisterRoute("CreateAccount", typeof(CreateAccount));
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));

        Routing.RegisterRoute("UserPage", typeof(UserPage));
        Routing.RegisterRoute("UserPageScheduler", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserSettings", typeof(UserSettings));
        Routing.RegisterRoute("UserWorkingHours", typeof(UserWorkingHours));



        Routing.RegisterRoute("PageUserList", typeof(PageUserList));
        Routing.RegisterRoute("PageUserManagement", typeof(PageUserManagement));
        Routing.RegisterRoute("PageChoice", typeof(PageChoice));
        Routing.RegisterRoute("PageScheduler", typeof(PageScheduler));
        Routing.RegisterRoute("PageOvertime", typeof(PageOvertime));
        Routing.RegisterRoute("PageAdminAndManagementManagement", typeof(PageAdminAndManagementManagement));



        Routing.RegisterRoute("ManagementPage", typeof(ManagementPage));


        Routing.RegisterRoute("PageUserList/StartPage", typeof(PageUserList));
        Routing.RegisterRoute("PageUserList/PageUserManagement", typeof(PageUserList));
        Routing.RegisterRoute("PageUserManagement/PageUserList", typeof(PageUserManagement));

        Routing.RegisterRoute("UserPage/StartPage", typeof(UserPage));
        Routing.RegisterRoute("UserPageScheduler/UserPage", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserPageScheduler/StartPage", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserPage/UserPageScheduler", typeof(UserPage));
        Routing.RegisterRoute("UserPage/UserWorkingHours", typeof(UserPage));
        Routing.RegisterRoute("ManagementPage/StartPage", typeof(ManagementPage));

        Routing.RegisterRoute("UserSettings/UserPage", typeof(UserSettings));
        Routing.RegisterRoute("UserWorkingHours/UserPage", typeof(UserWorkingHours));

        Routing.RegisterRoute("LoginPage/StartPage", typeof(LoginPage));
        Routing.RegisterRoute("LoginPage/PageChoice", typeof(LoginPage));
        Routing.RegisterRoute("CreateAccount/StartPage", typeof(CreateAccount));

    }
}
