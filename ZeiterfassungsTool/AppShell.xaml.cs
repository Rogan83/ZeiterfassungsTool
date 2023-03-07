using ZeiterfassungsTool.MVVM.ViewModels.Admin;
using ZeiterfassungsTool.MVVM.Views;
using ZeiterfassungsTool.MVVM.Views.Admin;
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


        Routing.RegisterRoute("AdminPageUserList", typeof(AdminPageUserList));
        Routing.RegisterRoute("AdminPageUserManagement", typeof(AdminPageUserManagement));
        Routing.RegisterRoute("AdminPageChoice", typeof(AdminPageChoice));
        Routing.RegisterRoute("AdminPageScheduler", typeof(AdminPageScheduler));



        Routing.RegisterRoute("ManagementPage", typeof(ManagementPage));


        Routing.RegisterRoute("AdminPageUserList/StartPage", typeof(AdminPageUserList));
        Routing.RegisterRoute("AdminPageUserList/AdminPageUserManagement", typeof(AdminPageUserList));
        Routing.RegisterRoute("AdminPageUserManagement/AdminPageUserList", typeof(AdminPageUserManagement));
        //Routing.RegisterRoute("AdminPageChoice/AdminPageScheduler", typeof(AdminPageChoice));

        Routing.RegisterRoute("UserPage/StartPage", typeof(UserPage));
        Routing.RegisterRoute("UserPageScheduler/UserPage", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserPageScheduler/StartPage", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserPage/UserPageScheduler", typeof(UserPage));
        Routing.RegisterRoute("ManagementPage/StartPage", typeof(ManagementPage));

        Routing.RegisterRoute("UserSettings/UserPage", typeof(UserSettings));

        Routing.RegisterRoute("LoginPage/StartPage", typeof(LoginPage));
        Routing.RegisterRoute("LoginPage/AdminPageChoice", typeof(LoginPage));
        Routing.RegisterRoute("CreateAccount/StartPage", typeof(CreateAccount));

    }
}
