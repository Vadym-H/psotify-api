using AutoMapper;
using Psotify.Models.PlaylistModels;
using Psotify.Models.SongModels;

namespace Psotify.Data
{
    public class PsotifyMappingProfile : Profile
    {
        public PsotifyMappingProfile()
        {
            CreateMap<Song, SongDto>();
            CreateMap<SongUpdateModel, Song>();
            CreateMap<PlaylistCreateModel, Playlist>();
        }
    }
}
