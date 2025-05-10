using Psotify.Models.PlaylistModels;

namespace Psotify.Models.SongModels
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Length { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiredDate { get; set; }
        public List<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
