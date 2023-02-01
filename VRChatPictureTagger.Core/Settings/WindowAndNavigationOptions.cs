using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace VRChatPictureTagger.Core.Settings
{
	public class WindowAndNavigationOptions
	{
		public Window MainWindow { get; set; }
		public Frame ContentFrame { get; set; }
		public DispatcherQueue UIDispatcher { get; set; }
	}
}
