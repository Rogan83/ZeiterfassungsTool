using Microsoft.Extensions.Logging;
using SQLiteDemo.Repositories;
using ZeiterfassungsTool.Models;

namespace ZeiterfassungsTool;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        // TODO: Add statements for adding PersonRepository as a singleton
        string dbPath = FileAccessHelper.GetLocalFilePath("timetracking.db3");

        //builder.Services.AddSingleton<Repository>(s => ActivatorUtilities.CreateInstance<Repository>(s, dbPath));
        builder.Services.AddSingleton<BaseRepository<Employee>>(s => ActivatorUtilities.CreateInstance<BaseRepository<Employee>>(s, dbPath));
        builder.Services.AddSingleton<BaseRepository<Timetracking>>(s => ActivatorUtilities.CreateInstance<BaseRepository<Timetracking>>(s, dbPath));

        return builder.Build();
	}
}
