namespace Psotify.Models.SongModels
{
    public class SongUpdateModel : ISongModel
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Length { get; set; }
    }
}
