using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Announcer.Db;

namespace Announcer.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Announcer.Models.Dashboard", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("BroadcastEnd");

                    b.Property<DateTime>("BroadcastStart");

                    b.Property<string>("Channel");

                    b.Property<uint>("ChannelId");

                    b.Property<bool>("IsLive");

                    b.Property<uint>("MaxViewers");

                    b.Property<string>("Media");

                    b.Property<string>("Title");

                    b.Property<uint>("Viewers");

                    b.Property<uint?>("VkPostId");

                    b.HasKey("Id");

                    b.ToTable("Dashboards");
                });

            modelBuilder.Entity("Announcer.Models.DataUpdate", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("RunDate");

                    b.HasKey("Id");

                    b.ToTable("DataUpdates");
                });

            modelBuilder.Entity("Announcer.Models.Game", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("GiantBombId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Announcer.Models.GamePlatform", b =>
                {
                    b.Property<uint>("GameId");

                    b.Property<string>("PlatformAbbreviation");

                    b.HasKey("GameId", "PlatformAbbreviation");

                    b.HasIndex("PlatformAbbreviation");

                    b.ToTable("GamePlatform");
                });

            modelBuilder.Entity("Announcer.Models.Platform", b =>
                {
                    b.Property<string>("Abbreviation")
                        .ValueGeneratedOnAdd();

                    b.Property<uint>("GiantBombId");

                    b.Property<string>("Name");

                    b.HasKey("Abbreviation");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("Announcer.Models.PlatformAlias", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("PlatformForeignKey");

                    b.HasKey("Id");

                    b.HasIndex("PlatformForeignKey");

                    b.ToTable("PlatformAlias");
                });

            modelBuilder.Entity("Announcer.Models.GamePlatform", b =>
                {
                    b.HasOne("Announcer.Models.Game", "Game")
                        .WithMany("GamePlatforms")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Announcer.Models.Platform", "Platform")
                        .WithMany("GamePlatforms")
                        .HasForeignKey("PlatformAbbreviation")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Announcer.Models.PlatformAlias", b =>
                {
                    b.HasOne("Announcer.Models.Platform", "Platform")
                        .WithMany("Aliases")
                        .HasForeignKey("PlatformForeignKey");
                });
        }
    }
}
