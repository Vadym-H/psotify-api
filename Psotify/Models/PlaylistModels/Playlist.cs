﻿namespace Psotify.Models.PlaylistModels
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; } 
        public User User { get; set; }
        public List<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
}
