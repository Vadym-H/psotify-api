using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Psotify.Data;
using Psotify.HelpClasses;
using Psotify.Models.SongModels;

namespace Psotify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class SongsController : ControllerBase
    {
        private readonly PsotifyDbContext _psotifyDbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<SongUpdateModel> _songUpdateModelValidator;  
        private readonly IValidator<SongCreateModel> _songCreateModelValidator;
        public SongsController(PsotifyDbContext psotifyDbContext, IMapper mapper, IValidator<SongUpdateModel> songUpdateModelValidato, IValidator<SongCreateModel> songCreateModelValidator)
        {
            _psotifyDbContext = psotifyDbContext;
            _mapper = mapper;
            _songUpdateModelValidator = songUpdateModelValidato;
            _songCreateModelValidator = songCreateModelValidator;
        }

        [HttpGet("get-all-songs")]
        public IActionResult GetAllSongs(int pageNumber = 1, int pageSize = 5)
        {
            var totalSongs = _psotifyDbContext.Songs.Count();

            pageNumber = Math.Max(1, pageNumber);
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var totalPages = Math.Ceiling(totalSongs / (double)pageSize);

            var songs = _psotifyDbContext.Songs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var(prev, next) = Paginator.GeneatePageURls(Request, pageNumber, (int)totalPages, pageSize);
            Paginator.AddPaginationToHeader(next, prev, Response);

            var paginationMetaData = new
            {
                totalSongs,
                totalPages,
                currentPage = pageNumber,
                pageSize
            };
            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetaData));
            return Ok(_mapper.Map<IEnumerable<SongDto>>(songs));

        }

        [HttpGet("{id}")]
        public IActionResult GetSongById(int id)
        {
            var songById = _psotifyDbContext.Songs.Find(id);
            if (songById == null)
                return NotFound($"Song with Id {id} not found.");
            var songByIdDto = _mapper.Map<SongDto>(songById);
            return Ok(songByIdDto);
        }

        [HttpPost]
        public IActionResult AddSong([FromBody] SongCreateModel model)
        {

            var result = _songCreateModelValidator.Validate(model);
            if (!result.IsValid)
                return BadRequest(result.Errors);

            var newEntity = new Song
            {
                Artist = model.Artist,
                Length = model.Length,
                Title = model.Title,
                CreatedDate = DateTime.UtcNow,
                LastModifiredDate = null
            };

            _psotifyDbContext.Songs.Add(newEntity);
            _psotifyDbContext.SaveChanges();
            return Ok($"Song with  ID {newEntity.Id} added successfully: ");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSong(int id, [FromBody] SongUpdateModel model)
        {
            var result = _songUpdateModelValidator.Validate(model);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            var existingSong = _psotifyDbContext.Songs.Find(id);

            if (existingSong == null)
                return NotFound($"Song with Id {id} not found.");

            var map = _mapper.Map(model, existingSong);
            existingSong.LastModifiredDate = DateTime.UtcNow;

            _psotifyDbContext.SaveChanges();

            return Ok(map);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var songById = _psotifyDbContext.Songs.Find(id);
            if (songById == null)
                return NotFound($"Song with Id {id} not found.");

            _psotifyDbContext.Songs.Remove(songById);
            _psotifyDbContext.SaveChanges();
            return Ok($"Song with id:{id} deleted successfully");
        }
    }
}


