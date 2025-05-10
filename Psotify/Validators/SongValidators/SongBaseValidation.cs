using FluentValidation;
using Psotify.Data;
using Psotify.Models.SongModels;

namespace Psotify.Validators.SongValidators
{
    public class SongBaseValidation<T> : AbstractValidator<T> where T : ISongModel
    {
        private readonly PsotifyDbContext _dbContext;
        public SongBaseValidation(PsotifyDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The title field is required.")
                .MinimumLength(1).WithMessage("The title cannot be empty");
            RuleFor(x => x.Artist)
                .NotEmpty().WithMessage("The artist field is required.")
                .MinimumLength(1).WithMessage("The artist cannot be empty");
            RuleFor(x => x.Length)
                .NotEmpty().WithMessage("The length field is required.")
                .Matches(@"^\d{1,2}:\d{2}$").WithMessage("The length must be in the format mm:ss.");
            RuleFor(x => new { x.Title, x.Artist })
            .Must(pair => !CombinationExists(pair.Title, pair.Artist))
            .WithMessage("The song with this title and artist already exists.");
        }
        private bool CombinationExists(string title, string artist)
        {
            return _dbContext.Songs.Any(s => s.Title == title && s.Artist == artist);
        }
    }
}
