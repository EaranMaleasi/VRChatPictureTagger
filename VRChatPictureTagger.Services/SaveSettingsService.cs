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

		public async Task SaveMainSettings(MainSettings newPathSettings)
		{
			try
			{
				string json = await File.ReadAllTextAsync(settingsFile);
				JObject jsonObj = (JObject)JsonConvert.DeserializeObject(json);
				JObject searchObject = jsonObj[nameof(MainSettings)] as JObject;

				searchObject.Property(nameof(MainSettings.UseBackNavigation)).Value = JToken.FromObject(newPathSettings.UseBackNavigation);
				searchObject.Property(nameof(MainSettings.PictureSearchPaths)).Value = JArray.FromObject(newPathSettings.PictureSearchPaths);
				searchObject.Property(nameof(MainSettings.intSetting)).Value = JToken.FromObject(30);
				searchObject.Property(nameof(MainSettings.doubleSetting)).Value = JToken.FromObject(30.3);
				searchObject.Property(nameof(MainSettings.stringSetting)).Value = JToken.FromObject("A longer test string");
				searchObject.Property(nameof(MainSettings.nullSetting)).Value = JToken.FromObject("notNull anymore");

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
