using Microsoft.UI.Xaml.Controls;

using VRChatPictureTagger.Interfaces.Navigation;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface INavigator
	{
		bool IsNavigatorInitialized { get; }

		bool CanNavigateTo(string friendlyName);
		void Initialize(Frame navigationView);
		void NavigateTo<TViewModel>() where TViewModel : class, IViewModel;
		void NavigateTo(string friendlyName);
	}
}