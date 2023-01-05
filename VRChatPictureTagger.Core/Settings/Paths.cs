﻿using System.Collections.Generic;

namespace VRChatPictureTagger.Core.Settings
{
	public class Paths
	{
		public List<string> PictureSearchPaths { get; set; } = new List<string>();
		public string VrcxDbPath { get; set; }
		public string VrcptDbPath { get; set; }
	}
}