using System;
using System.IO;

namespace VRChatPictureTagger.Core.Defaults
{
	public partial class Default
	{
		public static class Paths
		{
			public static readonly string defaultVrcptDbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VRCPT", "VRCPT.sqlite3");
			public static readonly string defaultVrcxDbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VRCX", "VRCX.sqlite3");
			public static readonly string defaultVrcFolderPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "VRChat");
		}
	}
}
