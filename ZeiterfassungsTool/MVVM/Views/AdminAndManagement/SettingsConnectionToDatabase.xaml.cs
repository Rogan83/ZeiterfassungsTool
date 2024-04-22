using ZeiterfassungsTool.MVVM.ViewModels.AdminAndManagement;

namespace ZeiterfassungsTool.MVVM.Views.AdminAndManagement;

public partial class SettingsConnectionToDatabase : ContentPage
{
	SettingsConnectionToDatabaseModel settingsConnectionToDatabaseModel;
	public SettingsConnectionToDatabase()
	{
		InitializeComponent();
		settingsConnectionToDatabaseModel = new SettingsConnectionToDatabaseModel();
		BindingContext = settingsConnectionToDatabaseModel;
	}
}