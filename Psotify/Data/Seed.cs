using Psotify.Models;
using Psotify.Models.PlaylistModels;
using Psotify.Models.SongModels;

namespace Psotify.Data
{
    public static class Seed
    {
        public static List<Song> GetSongs()
        {
            return new List<Song> {
                new Song { Title = "Master of Puppets", Artist = "Metallica", Length = "8:35" },
                new Song { Title = "Enter Sandman", Artist = "Metallica", Length = "5:32" },
                new Song { Title = "Nothing Else Matters", Artist = "Metallica", Length = "6:28" },
                new Song { Title = "One", Artist = "Metallica", Length = "7:26" },
                new Song { Title = "Fade to Black", Artist = "Metallica", Length = "6:57" },
                new Song { Title = "Comfortably Numb", Artist = "Pink Floyd", Length = "6:22" },
                new Song { Title = "Wish You Were Here", Artist = "Pink Floyd", Length = "5:40" },
                new Song { Title = "Time", Artist = "Pink Floyd", Length = "6:53" },
                new Song { Title = "Another Brick in the Wall", Artist = "Pink Floyd", Length = "5:37" },
                new Song { Title = "Shine On You Crazy Diamond", Artist = "Pink Floyd", Length = "13:33" },
                new Song { Title = "Paranoid", Artist = "Black Sabbath", Length = "2:52" },
                new Song { Title = "Iron Man", Artist = "Black Sabbath", Length = "5:55" },
                new Song { Title = "War Pigs", Artist = "Black Sabbath", Length = "7:57" },
                new Song { Title = "Black Sabbath", Artist = "Black Sabbath", Length = "6:18" },
                new Song { Title = "Sabbath Bloody Sabbath", Artist = "Black Sabbath", Length = "5:45" },
                new Song { Title = "Smoke on the Water", Artist = "Deep Purple", Length = "5:40" },
                new Song { Title = "Highway Star", Artist = "Deep Purple", Length = "6:05" },
                new Song { Title = "Child in Time", Artist = "Deep Purple", Length = "10:17" },
                new Song { Title = "Lazy", Artist = "Deep Purple", Length = "7:23" },
                new Song { Title = "Burn", Artist = "Deep Purple", Length = "6:00" }
            };
        }
        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Admin" },
                new User { Username = "user", PasswordHash = BCrypt.Net.BCrypt.HashPassword("hello123"), Role = "User" },
                new User { Username = "guest", PasswordHash = BCrypt.Net.BCrypt.HashPassword("guest"), Role = "User" },
                new User { Username = "guest2", PasswordHash = BCrypt.Net.BCrypt.HashPassword("guest"), Role = "User" },
                new User { Username = "guest3", PasswordHash = BCrypt.Net.BCrypt.HashPassword("guest"), Role = "User" }
            };
        }
        public static List<Playlist> GetPlaylists()
        {
            return new List<Playlist>
            {
                new Playlist { Name = "My Favorite Songs", UserId = 1 },
                new Playlist { Name = "Chill Vibes", UserId = 2 },
                new Playlist { Name = "Workout Mix", UserId = 1 },
                new Playlist { Name = "Road Trip", UserId = 3 },
                new Playlist { Name = "Party Hits", UserId = 4 },
                new Playlist { Name = "Classic Rock", UserId = 5 }
            };
        }
        public static List<PlaylistSong> GetPlaylistSongs()
        {
            var rawData = new List<PlaylistSong>
            {
                new PlaylistSong { PlaylistId = 1, SongId = 1 },
                new PlaylistSong { PlaylistId = 1, SongId = 1 },
                new PlaylistSong { PlaylistId = 1, SongId = 2 },
                new PlaylistSong { PlaylistId = 2, SongId = 3 },
                new PlaylistSong { PlaylistId = 3, SongId = 4 },
                new PlaylistSong { PlaylistId = 4, SongId = 5 },
                new PlaylistSong { PlaylistId = 5, SongId = 6 },
                new PlaylistSong { PlaylistId = 6, SongId = 7 },
                new PlaylistSong { PlaylistId = 1, SongId = 3 },
                new PlaylistSong { PlaylistId = 1, SongId = 7 },
                new PlaylistSong { PlaylistId = 1, SongId = 12 },
                new PlaylistSong { PlaylistId = 2, SongId = 1 },
                new PlaylistSong { PlaylistId = 2, SongId = 5 },
                new PlaylistSong { PlaylistId = 2, SongId = 17 },
                new PlaylistSong { PlaylistId = 3, SongId = 2 },
                new PlaylistSong { PlaylistId = 3, SongId = 9 },
                new PlaylistSong { PlaylistId = 3, SongId = 14 },
                new PlaylistSong { PlaylistId = 4, SongId = 4 },
                new PlaylistSong { PlaylistId = 4, SongId = 8 },
                new PlaylistSong { PlaylistId = 4, SongId = 19 },
                new PlaylistSong { PlaylistId = 5, SongId = 6 },
                new PlaylistSong { PlaylistId = 5, SongId = 10 },
                new PlaylistSong { PlaylistId = 5, SongId = 15 },
                new PlaylistSong { PlaylistId = 6, SongId = 11 },
                new PlaylistSong { PlaylistId = 6, SongId = 13 },
                new PlaylistSong { PlaylistId = 6, SongId = 20 },
                new PlaylistSong { PlaylistId = 2, SongId = 18 },
                new PlaylistSong { PlaylistId = 1, SongId = 16 }
            };
            return rawData
            .GroupBy(ps => new { ps.PlaylistId, ps.SongId })
            .Select(g => g.First())
            .ToList();
        }
    }
}
