using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	public class SaveSettingsService : ISaveSettingsService
	{
		private const string settingsFile = "appsettings.json";
		readonly ILogger<SaveSettingsService> _logger;

		public SaveSettingsService(ILogger<SaveSettingsService> logger)
		{ _logger = logger; }

		public async Task SavePathSettings(Paths newPathSettings)
		{
			try
			{
				string json = await File.ReadAllTextAsync(settingsFile);
				JObject jsonObj = (JObject)JsonConvert.DeserializeObject(json);
				JObject searchObject = jsonObj[nameof(Paths)] as JObject;

				searchObject.Property(nameof(Paths.PictureSearchPaths)).Value = JArray.FromObject(newPathSettings.PictureSearchPaths);
				searchObject.Property(nameof(Paths.VrcptDbPath)).Value = JArray.FromObject(newPathSettings.VrcptDbPath);
				searchObject.Property(nameof(Paths.VrcxDbPath)).Value = JArray.FromObject(newPathSettings.VrcxDbPath);

				string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
				await File.WriteAllTextAsync(settingsFile, output);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Exception occurred trying to write settings to appsettings.json file.");
				throw;
			}
		}

	}
}
