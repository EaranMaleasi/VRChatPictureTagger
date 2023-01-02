using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	public class SaveSettingsService : ISaveSettingsService
	{
		private const string settingsFile = "appsettings.json";

		public SaveSettingsService() { }

		public async Task Save<TSetting>(TSetting updatedSettingsInstance)
		{
			string settingsSection = nameof(TSetting);
			string json = await File.ReadAllTextAsync(settingsFile);
			dynamic jsonObj = JsonConvert.DeserializeObject(json);

			jsonObj[settingsSection] = updatedSettingsInstance;
			string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
			await File.WriteAllTextAsync(settingsFile, output);
		}

	}
}
