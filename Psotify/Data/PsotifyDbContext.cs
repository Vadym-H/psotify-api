using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Psotify.Models;
using BCrypt.Net;
using Psotify.Models.SongModels;
using Psotify.Models.PlaylistModels;

namespace Psotify.Data
{
    public class PsotifyDbContext(DbContextOptions<PsotifyDbContext> options) : DbContext(options)
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });
            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId);
            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongId);
        }
        public static void SeedData(PsotifyDbContext context)
        {
            if (!context.Songs.Any())
            {
                context.Songs.AddRange(Seed.GetSongs());
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(Seed.GetUsers());
                context.SaveChanges();
            }
            if (!context.Playlists.Any())
            {
                context.Playlists.AddRange(Seed.GetPlaylists());
                context.SaveChanges();
            }
            if (!context.PlaylistSongs.Any())
            {
                context.PlaylistSongs.AddRange(Seed.GetPlaylistSongs());
                context.SaveChanges();
            }
        }
    }
}
    
