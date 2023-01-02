using Microsoft.Extensions.DependencyInjection;

using VRChatPictureTagger.Views.Root;

namespace VRChatPictureTagger.Views.Bootstrap
{
	public static class ViewsDependencyInjectionExtensions
	{
		public static IServiceCollection AddVRCPT_Views(this IServiceCollection services)
		{
			services.AddSingleton<NavigationPage>();
			services.AddSingleton<SettingsPage>();
			return services;
		}
	}
}
