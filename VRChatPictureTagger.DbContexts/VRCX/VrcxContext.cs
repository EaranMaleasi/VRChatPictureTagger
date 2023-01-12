// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using VRChatPictureTagger.Core.Defaults;
using VRChatPictureTagger.Core.Settings;
using VRChatPictureTagger.Models.VRCX_SQLite;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VRChatPictureTagger.DbContexts.VRCX
{
	public partial class VrcxContext : DbContext
	{
		public string DbPath { get; }

		public VrcxContext()
		{
		}

		public VrcxContext(DbContextOptions<VrcxContext> contextOptions, IOptions<Paths> pathOptions)
			: base(contextOptions)
		{
			DbPath = Default.Paths.defaultVrcxDbPath;
		}


		public virtual DbSet<GamelogJoinLeave> GamelogJoinLeaves { get; set; }

		public virtual DbSet<GamelogLocation> GamelogLocations { get; set; }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		   => optionsBuilder.UseSqlite(DbPath);

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<GamelogJoinLeave>(entity =>
			{
				entity.ToTable("gamelog_join_leave");

				entity.HasIndex(e => new { e.CreatedAt, e.Type, e.DisplayName }, "IX_gamelog_join_leave_created_at_type_display_name").IsUnique();

				entity.Property(e => e.Id)
					.ValueGeneratedNever()
					.HasColumnName("id");
				entity.Property(e => e.CreatedAt).HasColumnName("created_at");
				entity.Property(e => e.DisplayName).HasColumnName("display_name");
				entity.Property(e => e.Location).HasColumnName("location");
				entity.Property(e => e.Time).HasColumnName("time");
				entity.Property(e => e.Type).HasColumnName("type");
				entity.Property(e => e.UserId).HasColumnName("user_id");
			});

			modelBuilder.Entity<GamelogLocation>(entity =>
			{
				entity.ToTable("gamelog_location");

				entity.HasIndex(e => new { e.CreatedAt, e.Location }, "IX_gamelog_location_created_at_location").IsUnique();

				entity.Property(e => e.Id)
					.ValueGeneratedNever()
					.HasColumnName("id");
				entity.Property(e => e.CreatedAt).HasColumnName("created_at");
				entity.Property(e => e.Location).HasColumnName("location");
				entity.Property(e => e.Time).HasColumnName("time");
				entity.Property(e => e.WorldId).HasColumnName("world_id");
				entity.Property(e => e.WorldName).HasColumnName("world_name");
			});
			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
