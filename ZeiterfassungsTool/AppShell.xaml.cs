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
        Routing.RegisterRoute("Choice", typeof(Choice));
        Routing.RegisterRoute("Test", typeof(Test));

        Routing.RegisterRoute("UserPage", typeof(UserPage));
        Routing.RegisterRoute("UserPageScheduler", typeof(UserPageScheduler));
        Routing.RegisterRoute("UserSettings", typeof(UserSettings));
        Routing.RegisterRoute("UserWorkingHours", typeof(UserWorkingHours));

        Routing.RegisterRoute("UserList", typeof(UserList));
        Routing.RegisterRoute("UserManagement", typeof(UserManagement));
        Routing.RegisterRoute("Scheduler", typeof(Scheduler));
        Routing.RegisterRoute("Overtime", typeof(Overtime));
        Routing.RegisterRoute("AdminAndManagementManagement", typeof(AdminAndManagementManagement));

        Routing.RegisterRoute("ManagementPage", typeof(ManagementPage));

        Routing.RegisterRoute("SettingsConnectionToDatabase", typeof(SettingsConnectionToDatabase));

        //Routing.RegisterRoute("UserList/StartPage", typeof(UserList));
        //Routing.RegisterRoute("UserList/UserManagement", typeof(UserList));
        //Routing.RegisterRoute("UserList/Choice", typeof(UserList));

        //Routing.RegisterRoute("UserManagement/UserList", typeof(UserManagement));

        //Routing.RegisterRoute("UserPage/LoginPage", typeof(UserPage));
        //Routing.RegisterRoute("CreateAccount/LoginPage", typeof(UserPage));

        //Routing.RegisterRoute("UserPageScheduler/UserPage", typeof(UserPageScheduler));
        //Routing.RegisterRoute("UserPageScheduler/StartPage", typeof(UserPageScheduler));
        //Routing.RegisterRoute("UserPage/UserPageScheduler", typeof(UserPage));
        //Routing.RegisterRoute("UserPage/UserWorkingHours", typeof(UserPage));
        //Routing.RegisterRoute("ManagementPage/StartPage", typeof(ManagementPage));

        //Routing.RegisterRoute("UserSettings/UserPage", typeof(UserSettings));
        //Routing.RegisterRoute("UserWorkingHours/UserPage", typeof(UserWorkingHours));

       
        //Routing.RegisterRoute("LoginPage/PageChoice", typeof(LoginPage));
        //Routing.RegisterRoute("PageChoice/LoginPage", typeof(PageChoice));
        //Routing.RegisterRoute("LoginPage/StartPage", typeof(LoginPage));
        //Routing.RegisterRoute("CreateAccount/StartPage", typeof(CreateAccount));

    }
}
