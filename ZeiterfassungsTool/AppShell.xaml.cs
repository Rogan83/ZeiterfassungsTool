using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.MVVM.Views.Admin;

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

        Routing.RegisterRoute("AdminPage", typeof(AdminPage));
        Routing.RegisterRoute("AdminPageUserManagement", typeof(AdminPageUserManagement));


        Routing.RegisterRoute("ManagementPage", typeof(ManagementPage));


        Routing.RegisterRoute("AdminPage/StartPage", typeof(AdminPage));
        Routing.RegisterRoute("AdminPage/AdminPageUserManagement", typeof(AdminPage));
        Routing.RegisterRoute("AdminPageUserManagement/AdminPage", typeof(AdminPageUserManagement));

        Routing.RegisterRoute("UserPage/StartPage", typeof(UserPage));
        Routing.RegisterRoute("UserPageScheduler/UserPage", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserPageScheduler/StartPage", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserPage/UserPageScheduler", typeof(UserPage));
        Routing.RegisterRoute("ManagementPage/StartPage", typeof(ManagementPage));

        Routing.RegisterRoute("LoginPage/StartPage", typeof(LoginPage));
        Routing.RegisterRoute("CreateAccount/StartPage", typeof(CreateAccount));

    }
}
