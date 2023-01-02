using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Foundation;

namespace VRChatPictureTagger.DbContexts.VRCPT
{
	public class VRCPT_Picture
	{
		public string Path { get; set; }
		public DateTime Date { get; set; }
		public List<VRCPT_Tag> Tags { get; set; }
		public Size Size { get; set; }
	}
}
