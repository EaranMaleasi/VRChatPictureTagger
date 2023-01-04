using System.Threading.Tasks;

using VRChatPictureTagger.Models.Settings;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface ISaveSettingsService
	{
		Task SaveSearchSettings(SearchSettings newSearchSettings);
	}
}