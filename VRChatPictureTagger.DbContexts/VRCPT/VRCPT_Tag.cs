using VRChatPictureTagger.Core.Enums;

using Windows.UI;

namespace VRChatPictureTagger.DbContexts.VRCPT
{
	public class VRCPT_Tag
	{
		public string Name { get; set; }
		public TagType Type { get; set; }
		public Color[] TagColor { get; set; }
	}
}
