using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Enums;
using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	public class SetupValidatorService : ISetupValidatorService
	{
		static readonly string defaultVrcptDbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VRCPT", "VRCPT.sqlite3");
		static readonly string defaultVrcxDbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VRCX", "VRCX.sqlite3");
		static readonly string defaultVrcFolderPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "VRChat");

		readonly IOptions<Paths> _options;
		readonly ILogger<SetupValidatorService> _logger;
		readonly ISaveSettingsService _settingsService;

		public SetupValidatorService(IOptions<Paths> options, ILogger<SetupValidatorService> logger, ISaveSettingsService settingsService)
		{
			_options = options;
			_logger = logger;
			_settingsService = settingsService;
		}

		public (bool, ValidationResult) ValidateSetup()
		{
			_logger.LogInformation($"Validating settings");

			bool isValid = true;

			Paths pathSettings = _options.Value;

			if (pathSettings == null)
				return (false, ValidationResult.NoSettingsObject);
			if (isValid && string.IsNullOrWhiteSpace(pathSettings.VrcxDbPath))
				return (false, ValidationResult.VrcxDbPathNull);
			if (isValid && string.IsNullOrWhiteSpace(pathSettings.VrcptDbPath))
				return (false, ValidationResult.VrcptDbPathNull);

			FileInfo fiVrcxDb = new(pathSettings.VrcxDbPath);
			FileInfo fiVrptDb = new(pathSettings.VrcptDbPath);

			if (!fiVrcxDb.Exists)
				return (false, ValidationResult.VrcxDbPathNotFound);
			if (!fiVrptDb.Exists)
				return (false, ValidationResult.VrcptDbPathNull);

			if (pathSettings.PictureSearchPaths != null || pathSettings.PictureSearchPaths.Count == 0)
				return (false, ValidationResult.NoSearchPaths);

			return (true, ValidationResult.Valid);
		}

		public void SetupDefaults()
		{
			_logger.LogInformation("Set path settings to default");
			Paths paths = new()
			{
				VrcptDbPath = defaultVrcptDbPath,
				VrcxDbPath = defaultVrcxDbPath,
				PictureSearchPaths = new List<string> { defaultVrcFolderPath }
			};

			_logger.LogInformation("Override old path settings");
			_settingsService.SavePathSettings(paths);
		}
	}
}
