using ZeiterfassungsTool.MVVM.Views;

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
		Routing.RegisterRoute("AdminPage", typeof(AdminPage));
		Routing.RegisterRoute("ManagementPage", typeof(ManagementPage));

        Routing.RegisterRoute("AdminPage/StartPage", typeof(AdminPage));
        Routing.RegisterRoute("UserPage/StartPage", typeof(UserPage));
        Routing.RegisterRoute("ManagementPage/StartPage", typeof(ManagementPage));

        Routing.RegisterRoute("LoginPage/StartPage", typeof(LoginPage));
        Routing.RegisterRoute("CreateAccount/StartPage", typeof(CreateAccount));

    }
}
