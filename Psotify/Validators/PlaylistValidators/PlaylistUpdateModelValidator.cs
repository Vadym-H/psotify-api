using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Psotify.Data;
using Psotify.Models.PlaylistModels;

namespace Psotify.Validators.PlaylistValidators
{

    public class PlaylistUpdateModelValidator : AbstractValidator<PlaylistUpdateModel>
    {
        private readonly PsotifyDbContext _psotifyDbContext;
        public PlaylistUpdateModelValidator(PsotifyDbContext psotifyDbContext)
        {
            _psotifyDbContext = psotifyDbContext;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Playlist name is required.")
                .MaximumLength(100).WithMessage("Playlist name must be less than 100 characters.");
                
                 RuleFor(x => x.SongIds)
                .NotEmpty().WithMessage("At least one song must be included in the playlist.")
                .Must(songIds => songIds.Distinct().Count() == songIds.Count).WithMessage("Duplicate song IDs are not allowed.")
                .Must(ContainOnlyValidSongs).WithMessage("One or more Song IDs do not exist.");

           
        }
        private bool ContainOnlyValidSongs(List<int> songIds)
        {
            if (songIds == null || !songIds.Any())
                return false;

            var existingIds = _psotifyDbContext.Songs
                .Where(s => songIds.Contains(s.Id))
                .Select(s => s.Id)
                .ToHashSet();

            return songIds.All(id => existingIds.Contains(id));
        }    
                
       
    }
}
