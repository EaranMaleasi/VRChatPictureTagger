using System.Collections.Generic;

namespace VRChatPictureTagger.Core.Settings
{
	public class MainSettings
	{
		public List<string> PictureSearchPaths { get; set; } = new List<string>();
		public bool UseBackNavigation { get; set; }
	}
}
