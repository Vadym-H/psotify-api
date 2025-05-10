using System.ComponentModel.DataAnnotations;

namespace Psotify.Models.SongModels
{
    public class SongCreateModel : ISongModel
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Length { get; set; }

    }
}
