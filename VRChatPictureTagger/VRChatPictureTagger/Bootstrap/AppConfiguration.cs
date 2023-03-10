using System;

using CommunityToolkit.Mvvm.Messaging;

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
		{
			IHost host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					services.Configure<MainSettings>(context.Configuration.GetSection("MainSettings"));
					services.Configure<WindowAndNavigationOptions>(x =>
					{
						x.MainWindow = App.MainWindow;
						x.ContentFrame = App.ContentFrame;
						x.UIDispatcher = App.UiDispatcherQueue;
					});

					services.AddVRCPT_Services()
							.AddVRCPT_Models()
							.AddVRCPT_ViewModels()
							.AddVRCPT_Views()
							.AddVRCPT_Contexts()
							.AddSingleton<IMessenger>(StrongReferenceMessenger.Default);
				})
				.ConfigureLogging((context, logging) =>
				{
					logging.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(context.Configuration.GetSection("Serilog")).CreateLogger());
				})
				.Build();

			return host;
		}

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
