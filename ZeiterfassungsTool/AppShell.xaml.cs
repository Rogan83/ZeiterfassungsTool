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
		Routing.RegisterRoute("UserTimeTracking", typeof(UserTimeTracking));
		Routing.RegisterRoute("AdminPage", typeof(AdminPage));

    }
}
