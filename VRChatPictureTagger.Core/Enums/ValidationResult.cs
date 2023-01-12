using System;

namespace VRChatPictureTagger.Core.Enums
{
	[Flags]
	public enum ValidationResult
	{
		None = 0,
		NoSearchPaths = 1,
		Valid = 2,
	}
}
