using Microsoft.Extensions.DependencyInjection;

using VRChatPictureTagger.Interfaces.Services;
using VRChatPictureTagger.Services.Messaging;

namespace VRChatPictureTagger.Services.Bootstrap
{
	public static class ServicesDependencyInjectionExtensions
	{
		public static IServiceCollection AddVRCPT_Services(this IServiceCollection services)
		{
			services.AddSingleton<IViewFactory, ViewFactory>();
			services.AddSingleton<ISaveSettingsService, SaveSettingsService>();
			services.AddSingleton<INavigator, Navigator>();
			services.AddSingleton<IWindowHandleService, WindowHandleService>();
			services.AddSingleton<ISetupValidatorService, SetupValidatorService>();
			services.AddSingleton<IMessagingCenter, MessagingCenter>();


			return services;
		}
	}
}
