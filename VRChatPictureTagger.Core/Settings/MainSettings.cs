using System.Collections.Generic;

namespace VRChatPictureTagger.Core.Settings
{
	public class MainSettings
	{
		public bool UseBackNavigation { get; set; }
		public int intSetting { get; set; }
		public double doubleSetting { get; set; }
		public string stringSetting { get; set; }
		public object nullSetting { get; set; }

		public List<string> PictureSearchPaths { get; set; } = new List<string>();

	}
}
