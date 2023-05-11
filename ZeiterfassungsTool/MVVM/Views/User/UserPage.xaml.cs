using System;
using ZeiterfassungsTool.MVVM.ViewModels.User;
using ZeiterfassungsTool.StaticClasses;

namespace ZeiterfassungsTool.MVVM.Views;

public partial class UserPage : ContentPage
{
	UserPageModel userPageModel; 
	public UserPage()
	{
		InitializeComponent();
		userPageModel = new UserPageModel();
		BindingContext = userPageModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var user = Login.WhoIsLoggedIn[0];
        userPageModel.Timetracking = await MySQLMethods.GetTimetracking(user.Id);


        userPageModel.ShowStartTimer = true;
        userPageModel.ShowStopTimer = false;

        if (userPageModel.Timetracking == null)
        {
            return;
        }

        if (userPageModel.Timetracking.Count > 0)
        {
            userPageModel.index = userPageModel.Timetracking.Count - 1;
        }

        //if (userPageModel.index >= 0)  //Ist überhaupt ein Datensatz vorhanden?
        if (userPageModel.Timetracking.Count > 0)
        {
            bool isCurrentlyWorking = false;
            //if (user.Timetracking[index] != null)
            if (userPageModel.Timetracking[userPageModel.index] != null)
            {
                // Wenn der Mitarbeiter auf den Stopbutton gedrückt hat, wurde ja die EndTime ermittelt, was bedeutet, dass er momentan nicht arbeitet
                //if (user.Timetracking[index].EndTime > user.Timetracking[index].StartTime)
                if (userPageModel.Timetracking[userPageModel.index].EndTime > userPageModel.Timetracking[userPageModel.index].StartTime)
                {
                    isCurrentlyWorking = false;
                }
                else
                {
                    isCurrentlyWorking = true;
                }
            }

            //ShowStartTimer = !user.Timetracking[index].IsCurrentlyWorking;
            //ShowStopTimer = user.Timetracking[index].IsCurrentlyWorking;

            userPageModel.ShowStartTimer = !isCurrentlyWorking;
            userPageModel.ShowStopTimer = isCurrentlyWorking;

            //workbegin = user.Timetracking[index].StartTime;
            userPageModel.workbegin = userPageModel.Timetracking[userPageModel.index].StartTime;
            //workend = user.Timetracking[index].EndTime;
            userPageModel.workend = userPageModel.Timetracking[userPageModel.index].EndTime;

            //userPageModel.InitTimer(100, true);
        }
        else
        {
            userPageModel.ShowStartTimer = true;
            userPageModel.ShowStopTimer = false;

            userPageModel.workbegin = userPageModel.workend = DateTime.Now;
        }
    }
}