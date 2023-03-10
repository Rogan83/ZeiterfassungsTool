using ZeiterfassungsTool.MVVM.ViewModels.Admin;

namespace ZeiterfassungsTool.MVVM.Views.Admin;

public partial class PageOvertime : ContentPage
{
	public PageOvertime()
	{
		InitializeComponent();

		BindingContext = new AdminPageOvertimeModel();
	}


    private void chooseMonth_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        LblInfo.IsVisible = true;
        EntryMonth.IsVisible = true;
        EntryYear.IsVisible = true;
    }

    private void total_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        LblInfo.IsVisible = false;
        EntryMonth.IsVisible = false;
        EntryYear.IsVisible = false;
    }
}