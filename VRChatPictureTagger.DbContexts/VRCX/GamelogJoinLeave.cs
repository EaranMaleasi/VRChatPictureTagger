using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatPictureTagger.Models.VRCX_SQLite
{
	public partial class GamelogJoinLeave
	{
		public long Id { get; set; }

		public string? CreatedAt { get; set; }

		public string? Type { get; set; }

		public string? DisplayName { get; set; }

		public string? Location { get; set; }

		public string? UserId { get; set; }

		public long? Time { get; set; }
	}
}
