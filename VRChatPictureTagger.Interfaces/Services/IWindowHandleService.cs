using Windows.Storage.Pickers;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface IWindowHandleService
	{
		void SetWindowHandleOnPicker(FolderPicker folderPicker);
	}
}