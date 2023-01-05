using System.IO;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Settings;

namespace VRChatPictureTagger.Services
{
	public class SetupValidatorService
	{
		readonly IOptions<Paths> _options;
		readonly ILogger<SetupValidatorService> _logger;

		public SetupValidatorService(IOptions<Paths> options, ILogger<SetupValidatorService> logger)
		{
			_options = options;
			_logger = logger;
		}

		public bool ValidateSetup()
		{
			Paths pathSettings = _options.Value;

			if (pathSettings == null)
				return false;

			if (string.IsNullOrWhiteSpace(_options.Value.)
			{

			}


			FileInfo fi = new FileInfo(_options.Value.VrcxDbPath);
		}
	}
}
