using FluentValidation;
using Psotify.Data;
using Psotify.Models.SongModels;

namespace Psotify.Validators.SongValidators
{
    public class SongUpdateModelValidator : SongBaseValidation<SongUpdateModel>
    {
        public SongUpdateModelValidator(PsotifyDbContext dbContext) : base(dbContext)
        {

        }
    }
}
