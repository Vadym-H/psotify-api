using FluentValidation;
using Psotify.Data;
using Psotify.Models.SongModels;

namespace Psotify.Validators.SongValidators
{
    public class SongCreateModelValidator : SongBaseValidation<SongCreateModel>
    {

        private static readonly string[] RestrictedArtists = { "Saylor Twift", "Slack Babath" };

        public SongCreateModelValidator(PsotifyDbContext dbContext) : base(dbContext)
        {
            RuleFor(x => x.Artist)
                .Must(artist => ArtistIsValid(artist, RestrictedArtists))
                .WithMessage(x => $"{x.Artist} is invalid due to licence limitation");
        }

        private bool ArtistIsValid(string artist, string[] restrictedArtists)
        {
            return !restrictedArtists.Contains(artist);

        }
    }
}
