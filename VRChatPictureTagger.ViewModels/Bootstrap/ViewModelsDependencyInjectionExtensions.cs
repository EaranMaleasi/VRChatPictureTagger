using Microsoft.Extensions.DependencyInjection;

namespace VRChatPictureTagger.ViewModels.Bootstrap
{
	public static class ViewModelsDependencyInjectionExtensions
	{
		public static IServiceCollection AddVRCPT_ViewModels(this IServiceCollection services)
		{
			services.AddSingleton<NavigationBaseViewModel>();
			services.AddSingleton<SettingsViewModel>();
			return services;
		}
	}
}
