using VRChatPictureTagger.Interfaces.Navigation;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface INavigator
	{
		IView CurrentView { get; }
		IViewModel CurrentViewModel { get; }

		bool CanNavigateTo(string friendlyName);
		void NavigateBack();
		void NavigateTo<TViewModel>() where TViewModel : class, IViewModel;
		void NavigateTo(string friendlyName);
	}
}