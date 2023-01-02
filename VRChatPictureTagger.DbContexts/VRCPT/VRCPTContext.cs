using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

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
			string dbPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VRChatPictureTagger");
			Directory.CreateDirectory(dbPath);
			DbPath = Path.Join(dbPath, "VRCPT.sqlite3");
		}

		public VRCPTContext(DbContextOptions<VrcxContext> options)
			: base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		   => optionsBuilder.UseSqlite(DbPath);
	}
}
