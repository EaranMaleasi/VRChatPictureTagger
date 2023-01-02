using System.Collections.Generic;

namespace VRChatPictureTagger.Models.Settings
{
	public class SearchSettings
	{
		public List<string> AdditionalPictureSearchPaths { get; set; } = new List<string>();
		public bool DontSearchVRChatFolder { get; set; } = false;
	}
}
