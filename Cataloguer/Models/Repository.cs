﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cataloguer.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Cataloguer.Models
{
    public class Repository
    {
        private ApplicationDbContext Db { get; set; }

        public Repository() => Db = new ApplicationDbContext();

        public Repository(ApplicationDbContext context) => Db = context;

        public List<Artist> GetArtists() => Db.Artists.ToList();

        public List<Artist> GetArtistsByName(string name) => Db.Artists.Where(a => a.Name == name).ToList();

        public List<Album> GetAlbums() => Db.Albums.ToList();

        public List<Album> GetAlbumsByName(string name) => Db.Albums.Where(a => a.Name == name).ToList();

        public Track GetTrack(int id) => Db.Tracks.Include(t => t.Artist).FirstOrDefault(t => t.Id == id);

        public Track GetTrack(string trackName, string artistName) 
            => Db.Tracks.FirstOrDefault(t => t.Name == trackName && t.Artist.Name == artistName);

        public List<Track> GetTracks() => Db.Tracks.ToList();

        public List<Track> GetTopTracks(int amount) => Db.Tracks.Include(t => t.Artist)
            .OrderByDescending(t => Convert.ToInt64(t.Scrobbles.Replace(" ", ""))).Take(amount).ToList();

        public IEnumerable<Track> GetTopUserTracks(string userId)
        {
            var tracks = Db.Ratings.Where(r => r.ApplicationUser.Id == userId)
                .OrderBy(r => r.Rank).Select(r => r.Track);
            foreach (Track track in tracks)
            {
                yield return GetTrack(track.Id);
            }
        }

        public List<Track> GetTracksByName(string name) => Db.Tracks.Where(a => a.Name == name).ToList();

        public Artist GetArtist(string name) => Db.Artists.First(a => a.Name == name);

        public Album GetAlbum(string albumName, string artistName) 
            => Db.Artists.First(a => a.Name == artistName).Albums.First(a => a.Name == albumName);

        public void InsertOrUpdate(ApplicationUser user)
        {
            var existingItem = Db.Users.Find(user.Id);
            if (existingItem == null)
            {
                Db.Add(user);
            }
            else
            {
                Db.Entry(existingItem).CurrentValues.SetValues(user);
            }

            Db.SaveChanges();
        }

        public void InsertOrUpdate(Artist artist)
        {
            var existingItem = Db.Set<Artist>().FirstOrDefault(x => x.Name == artist.Name);
            if (existingItem == null)
            {
                Db.Add(artist);
            }
            else if(!string.IsNullOrWhiteSpace(artist.Scrobbles))
            {
                existingItem.Scrobbles = artist.Scrobbles;
            }

            Db.SaveChanges();
        }

        public void InsertOrUpdate(Track track)
        {
            track.Artist = Db.Set<Artist>().FirstOrDefault(a => a.Name == track.Artist.Name);
            var existingItem = Db.Set<Track>()
                .FirstOrDefault(x => x.Name == track.Name && x.Artist.Id == track.Artist.Id);
            if (existingItem == null)
            {
                Db.Add(track);
            }
            else if(!string.IsNullOrWhiteSpace(track.Scrobbles))
            {
                existingItem.Scrobbles = track.Scrobbles;
            }

            Db.SaveChanges();
        }

        public void InsertRating(Rating rating)
        {
            Db.Ratings.Add(rating);
            Db.SaveChanges();
        }

        public void DeleteRating(ApplicationUser applicationUser, Track track)
        {
            Rating rating = Db.Ratings
                .FirstOrDefault(r => r.ApplicationUser.Id == applicationUser.Id && r.Track.Id == track.Id);
            if (rating != null)
            {
                Db.Ratings.Remove(rating);
                Db.SaveChanges();
            }
        }

        public SelectList GetCountries() => new SelectList(Db.Countries, "Id", "Name");

        public SelectList GetLanguages() => new SelectList(Db.Languages, "Id", "Name");

        public SelectList GetTemperaments() => new SelectList(Db.Temperaments, "Id", "Name");
    }
}
