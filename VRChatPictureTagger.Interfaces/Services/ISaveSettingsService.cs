using System.Threading.Tasks;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface ISaveSettingsService
	{
		Task Save<TSetting>(TSetting updatedSettingsInstance);
	}
}