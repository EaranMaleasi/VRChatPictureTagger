using System;

using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Services;

using Windows.Storage.Pickers;

namespace VRChatPictureTagger.Services
{
	public class WindowHandleService : IWindowHandleService
	{
		readonly IOptions<MainWindowOption> _options;

		public WindowHandleService(IOptions<MainWindowOption> options)
			=> _options = options;

		public void SetWindowHandleOnPicker(FolderPicker folderPicker)
		{
			IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(_options.Value.MainWindow);
			WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
		}
	}
}
