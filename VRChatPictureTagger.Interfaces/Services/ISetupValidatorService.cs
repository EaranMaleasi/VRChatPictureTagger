using VRChatPictureTagger.Core.Enums;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface ISetupValidatorService
	{
		void SetupDefaults();
		(bool isValid, ValidationResult result) ValidateSetup();
	}
}