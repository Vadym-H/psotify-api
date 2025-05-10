namespace Psotify.Models.PlaylistModels
{
    public class PlaylistUpdateModel
    {
        public string Name { get; set; }
        public List<int> SongIds { get; set; } = new List<int>();
    }
}
