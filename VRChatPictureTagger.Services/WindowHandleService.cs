using System;

using Microsoft.UI.Xaml;

using VRChatPictureTagger.Interfaces.Services;

using Windows.Storage.Pickers;

namespace VRChatPictureTagger.Services
{
	public class WindowHandleService : IWindowHandleService
	{
		private Window _rootWindow;
		private bool _isInitialized = false;

		public void Initialize(Window window)
		{
			if (_isInitialized)
				return;

			_isInitialized = true;
			_rootWindow = window;
		}

		public void SetWindowHandleOnPicker(FolderPicker folderPicker)
		{
			if (!_isInitialized)
				return;

			IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_rootWindow);
			WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
		}
	}
}
