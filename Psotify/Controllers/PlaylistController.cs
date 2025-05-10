using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Psotify.Data;
using Psotify.Models;
using Psotify.Models.PlaylistModels;
using System.Linq;

namespace Psotify.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IValidator<PlaylistCreateModel> _createValidator;
        private readonly IValidator<PlaylistUpdateModel> _updateValidator;
        private readonly PsotifyDbContext _psotifyDbContext;
        public PlaylistController(PsotifyDbContext psotifyDbContext, IMapper mapper, IValidator<PlaylistCreateModel> createValidator, IValidator<PlaylistUpdateModel> updateValidator)
        {
            _psotifyDbContext = psotifyDbContext;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-playlist-details/{name}")]
        public IActionResult GetPlaylistWithUserAndSongs(string name)
        {
            var playlist = _psotifyDbContext.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .FirstOrDefault(p => p.Name == name);

            if (playlist == null)
                return NotFound();

            var result = new
            {
                PlaylistName = playlist.Name,
                User = new
                {
                    playlist.User.Username,
                    playlist.User.Role,
                },
                Songs = playlist.PlaylistSongs.Select(ps => new
                {
                    ps.Song.Title,
                    ps.Song.Artist,
                    ps.Song.Length
                }).ToList()
            };

            return Ok(result);
        }
        [HttpGet("Get-all-playlists")]
        public IActionResult GetAllPlaylists()
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User not found or not logged in.");

            var playlists = _psotifyDbContext.Playlists
                .Where(p => p.UserId == userId)
                .Include(ps => ps.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .ToList();

            if (playlists == null || playlists.Count == 0)
                return NotFound("No playlists found for this user.");

            var result = playlists.Select(p => new
            {
                PlaylistName = p.Name,
                Songs = p.PlaylistSongs.Select(ps => new
                {
                    ps.Song.Title,
                    ps.Song.Artist,
                    ps.Song.Length
                }).ToList()
            }).ToList();

            return Ok(result);

        }
        [HttpDelete("delete-playlist/{name}")]
        public IActionResult DeletePlaylist(string name)
        {
            var userId = GetCurrentUserId();

            if (userId == 0)
                return Unauthorized("User not found or not logged in.");

            var playlist = _psotifyDbContext.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefault(p => p.Name == name && p.UserId == userId);

            if (playlist == null)
                return NotFound("Playlist not found.");

            _psotifyDbContext.Playlists.Remove(playlist);
            _psotifyDbContext.SaveChanges();
            return Ok($"Playlist '{name}' deleted successfully.");
        }
        [HttpPost("create-playlist")]
        public IActionResult CreatePlaylist([FromBody] PlaylistCreateModel model)
        {
            var validator = _createValidator.Validate(model);
            if (!validator.IsValid)
                return BadRequest(validator.Errors);

            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User not found or not logged in.");

            var playlist = _mapper.Map<Playlist>(model);
            playlist.UserId = userId;

            playlist.PlaylistSongs = model.SongIds.Select(songId => new PlaylistSong
            {
                SongId = songId
            }).ToList();

            _psotifyDbContext.Playlists.Add(playlist);
            _psotifyDbContext.SaveChanges();

            return Ok("Playlist created successfully.");

        }


        [HttpPut("update/{id}")]
        public IActionResult UpdatePlaylist(int id, [FromBody] PlaylistUpdateModel model)
        {
            var validation = _updateValidator.Validate(model);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var userId = GetCurrentUserId();
            if (userId == 0) return Unauthorized();

            var playlist = _psotifyDbContext.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefault(p => p.Id == id && p.UserId == userId);

            if (playlist == null) return NotFound("Playlist not found.");

            playlist.Name = model.Name;
            playlist.PlaylistSongs.Clear();
            playlist.PlaylistSongs.AddRange(from songId in model.SongIds
            select new PlaylistSong
            {
                PlaylistId = id,
                SongId = songId
            });
            _psotifyDbContext.SaveChanges();
            return Ok("Playlist updated successfully.");
        }
        
        [HttpPut("Admin-Update/{id}")]
        public IActionResult AdminUpdate(int id, [FromBody] PlaylistUpdateModel model)
        {
            var validation = _updateValidator.Validate(model);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var playlist = _psotifyDbContext.Playlists
            .Include(p => p.PlaylistSongs)
            .FirstOrDefault(p => p.Id == id);

            if (playlist == null) return NotFound("Playlist not found.");

            playlist.Name = model.Name;
            playlist.PlaylistSongs.Clear();
            playlist.PlaylistSongs.AddRange(from songId in model.SongIds
                                            select new PlaylistSong
                                            {
                                                PlaylistId = id,
                                                SongId = songId
                                            });
            _psotifyDbContext.SaveChanges();
            return Ok("Playlist updated successfully.");
        }
        
        private int GetCurrentUserId()
        {
            var username = User.Identity.Name;
            if (string.IsNullOrEmpty(username)) return 0;

            return _psotifyDbContext.Users
                .Where(u => u.Username == username)
                .Select(u => u.Id)
                .FirstOrDefault();
        }
    }
}
