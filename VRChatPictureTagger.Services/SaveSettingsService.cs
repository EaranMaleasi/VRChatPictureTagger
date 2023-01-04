﻿using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using VRChatPictureTagger.Interfaces.Services;
using VRChatPictureTagger.Models.Settings;

namespace VRChatPictureTagger.Services
{
	public class SaveSettingsService : ISaveSettingsService
	{
		private const string settingsFile = "appsettings.json";
		readonly ILogger<SaveSettingsService> _logger;

		public SaveSettingsService(ILogger<SaveSettingsService> logger)
		{ _logger = logger; }

		public async Task SaveSearchSettings(SearchSettings newSearchSettings)
		{
			try
			{
				string json = await File.ReadAllTextAsync(settingsFile);
				JObject jsonObj = (JObject)JsonConvert.DeserializeObject(json);
				JObject searchObject = jsonObj[nameof(SearchSettings)] as JObject;

				searchObject.Property(nameof(SearchSettings.DontSearchVRChatFolder)).Value = newSearchSettings.DontSearchVRChatFolder;
				searchObject.Property(nameof(SearchSettings.AdditionalPictureSearchPaths)).Value = JArray.FromObject(newSearchSettings.AdditionalPictureSearchPaths);

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
