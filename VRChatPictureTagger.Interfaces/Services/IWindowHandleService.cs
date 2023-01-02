using Microsoft.UI.Xaml;

using Windows.Storage.Pickers;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface IWindowHandleService
	{
		void Initialize(Window window);
		void SetWindowHandleOnPicker(FolderPicker folderPicker);
	}
}