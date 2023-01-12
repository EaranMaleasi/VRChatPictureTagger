using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using VRChatPictureTagger.Core.Enums;
using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.DbContexts.Bootstrap;
using VRChatPictureTagger.Interfaces.Services;
using VRChatPictureTagger.Models.Bootstrap;
using VRChatPictureTagger.Services.Bootstrap;
using VRChatPictureTagger.ViewModels;
using VRChatPictureTagger.ViewModels.Bootstrap;
using VRChatPictureTagger.Views;
using VRChatPictureTagger.Views.Bootstrap;

namespace VRChatPictureTagger.Bootstrap
{
	internal class AppConfiguration
	{
		internal static IHost BuildHost()
			=> new HostBuilder()
				.ConfigureAppConfiguration((context, configBuilder) =>
				{
					configBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
					configBuilder.AddJsonFile("appsettings.json", false, true);
				})
				.ConfigureServices((context, services) =>
				{
					services.AddVRCPT_Services()
							.AddVRCPT_Models()
							.AddVRCPT_ViewModels()
							.AddVRCPT_Views()
							.AddVRCPT_Contexts();
					services.Configure<MainWindowOption>(x => x.MainWindow = App._mainWindow);
				})
				.ConfigureLogging((context, logging) =>
				{
					logging.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(context.Configuration).CreateLogger());
				})
				.Build();

		internal static void ConfigureViews(IServiceProvider services)
		{
			IViewFactory viewFactory = services.GetService<IViewFactory>();
			viewFactory.Register<SettingsViewModel, SettingsPage>();
		}

		internal static void ValidateSettings(IServiceProvider services)
		{
			ISetupValidatorService setupValidator = services.GetService<ISetupValidatorService>();

			ValidationResult result = setupValidator.ValidateSetup();
			if (result.HasFlag(ValidationResult.NoSearchPaths))
				setupValidator.SetupDefaultSearchPath();
		}
	}
}
