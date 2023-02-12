using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Defaults;
using VRChatPictureTagger.Core.Enums;
using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Interfaces.Services;

namespace VRChatPictureTagger.Services
{
	public class SetupValidatorService : ISetupValidatorService
	{
		readonly IOptions<MainSettings> _options;
		readonly ILogger<SetupValidatorService> _logger;
		readonly ISaveSettingsService _settingsService;

		public SetupValidatorService(IOptions<MainSettings> options, ILogger<SetupValidatorService> logger, ISaveSettingsService settingsService)
		{
			_options = options;
			_logger = logger;
			_settingsService = settingsService;
		}

		public ValidationResult ValidateSetup()
		{
			_logger.LogInformation($"Validating settings");

			ValidationResult result = ValidationResult.Valid;

			MainSettings pathSettings = _options.Value;

			if (pathSettings.PictureSearchPaths.Count == 0)
				result |= ValidationResult.NoSearchPaths;

			return result;
		}

		public void SetupDefaultSearchPath()
		{
			_logger.LogInformation("Set search paths to default");
			_options.Value.PictureSearchPaths = new() { Default.Paths.defaultVrcFolderPath };

			_logger.LogInformation("Overwrite old settings");
			_settingsService.SaveMainSettings(_options.Value);
		}
	}
}
