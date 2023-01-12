using Microsoft.EntityFrameworkCore;

using VRChatPictureTagger.Core.Defaults;
using VRChatPictureTagger.DbContexts.VRCX;

namespace VRChatPictureTagger.DbContexts.VRCPT
{
	internal class VRCPTContext : DbContext
	{
		public string DbPath { get; }
		public virtual DbSet<VRCPT_Picture> Pictures { get; set; }

		public virtual DbSet<VRCPT_Tag> Tags { get; set; }

		public VRCPTContext()
		{
		}

		public VRCPTContext(DbContextOptions<VrcxContext> contextOptions) : base(contextOptions)
			=> DbPath = Default.Paths.defaultVrcptDbPath;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		   => optionsBuilder.UseSqlite(DbPath);
	}
}
