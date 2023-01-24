namespace VRChatPictureTagger.Interfaces.Navigation
{
	public interface INavigationAware
	{
		/// <summary>
		/// Used to signal a ViewModel that it was navigated to. Implementing this allows a ViewModel to react to being navigated to.
		/// </summary>
		void NavigatedTo();
		/// <summary>
		/// Used to signal a ViewModel that it was navigated away from. Implementing this allows a ViewModel to react to being navigated away from.
		/// </summary>
		void NavigatedFrom();
	}
}
