using System;

using VRChatPictureTagger.Core.Exceptions;
using VRChatPictureTagger.Interfaces.Navigation;

namespace VRChatPictureTagger.Services.Exceptions
{
	public class NavigationException : LoggedException
	{
		public IViewModel ToViewModel { get; set; }
		public string ViewModelName { get; set; }

		public IView ToView { get; set; }
		public string ViewName { get; set; }

		public string FriendlyName { get; set; }

		public NavigationException() { }
		public NavigationException(string message) : base(message) { }
		public NavigationException(bool isLogged) : base(isLogged) { }
		public NavigationException(string message, Exception innerException) : base(message, innerException) { }
		public NavigationException(string message, bool isLogged) : base(message, isLogged) { }
		public NavigationException(string message, bool isLogged, Exception innerException) : base(message, isLogged, innerException) { }



	}
}
