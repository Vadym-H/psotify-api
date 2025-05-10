using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Psotify.Data;
using Psotify.Models.PlaylistModels;

namespace Psotify.Validators.PlaylistValidators
{
    public class PlaylistCreateModelValidator : AbstractValidator<PlaylistCreateModel>
    {
        private readonly PsotifyDbContext _psotifyDbContext;
        public PlaylistCreateModelValidator(PsotifyDbContext psotifyDbContext)
        {
            _psotifyDbContext = psotifyDbContext;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Playlist name is required.")
                .Length(1, 100).WithMessage("Playlist name must be between 1 and 100 characters.");
            RuleFor(x => x.SongIds)
                .NotEmpty().WithMessage("Playlist must contain at least one song.")
                .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Duplicate songs are not allowed.")
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
