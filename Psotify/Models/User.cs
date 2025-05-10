using Psotify.Models.PlaylistModels;

namespace Psotify.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // e.g., "Admin", "User"
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
