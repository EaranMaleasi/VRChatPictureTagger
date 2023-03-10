using System.Threading.Tasks;

using VRChatPictureTagger.Core.Settings;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface ISaveSettingsService
	{
		Task SaveMainSettings(MainSettings newPathSettings);
	}
}