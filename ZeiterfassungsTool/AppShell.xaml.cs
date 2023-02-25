using ZeiterfassungsTool.MVVM.Views;

namespace ZeiterfassungsTool;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute("Startpage", typeof(StartPage));
		Routing.RegisterRoute("CreateAccount", typeof(CreateAccount));

    }
}
