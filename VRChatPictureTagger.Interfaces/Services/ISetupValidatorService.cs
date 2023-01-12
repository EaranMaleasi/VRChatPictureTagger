using VRChatPictureTagger.Core.Enums;

namespace VRChatPictureTagger.Interfaces.Services
{
	public interface ISetupValidatorService
	{
		void SetupDefaultSearchPath();
		ValidationResult ValidateSetup();
	}
}