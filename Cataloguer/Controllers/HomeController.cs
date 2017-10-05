﻿using Cataloguer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        ArtistContext database = new ArtistContext();

        LastFMParser parser = new LastFMParser();

        public const int FirstPage = 1;

        public int artistsPerPage = 48;

        public int albumsPerPage = 48;

        public int tracksPerPage = 50;

        public int newSearchElements = 8;

        public ActionResult Index()
        {
            List<Artist> artists = parser.GetTopArtists(1, artistsPerPage);
            return View(artists);
        }

        public ActionResult TopArtists(int page)
        {
            List<Artist> artists = parser.GetTopArtists(page, artistsPerPage);
            return PartialView(artists);
        }

        public ActionResult ArtistProfile(string name)
        {
            Artist artist = parser.GetArtist(name);
            return View(artist);
        }

        public ActionResult ArtistBiography(string name)
        {
            Artist artist = parser.GetArtistWithBiography(name);
            return View(artist);
        }

        public ActionResult ArtistAllTracks(string name)
        {
            Artist artist = parser.GetArtistWithAllTracks(name);
            return View(artist);
        }

        public ActionResult ArtistTracks(string name, int page)
        {
            List<Track> tracks = parser.GetTracksOfArtist(name, page, tracksPerPage);
            return PartialView(tracks);
        }

        public ActionResult ArtistAllAlbums(string name)
        {
            Artist artist = parser.GetArtistWithAllAlbums(name);
            return View(artist);
        }

        public ActionResult ArtistAlbums(string name, int page)
        {
            List<Album> albums = parser.GetAlbumsOfArtist(name, page, albumsPerPage);
            return PartialView(albums);
        }

        public ActionResult Album(string albumName, string artistName)
        {
            Album album = parser.GetAlbum(albumName, artistName);
            return View(album);
        }

        public ActionResult Track(string trackName, string artistName)
        {
            Track track = parser.GetTrack(trackName, artistName);
            return View(track);
        }

        [HttpGet]
        public ActionResult Search(string value)
        {
            SearchingResults results = new SearchingResults
            {
                LastFMArtists = parser.SearchArtists(value, FirstPage, newSearchElements),
                LastFMAlbums = parser.SearchAlbums(value, FirstPage, newSearchElements),
                LastFMTracks = parser.SearchTracks(value, FirstPage, newSearchElements),
                LocalArtists = database.Artists.Where(a => a.Name == value).ToList(),
                LocalAlbums = database.Albums.Where(a => a.Name == value).ToList(),
                LocalTracks = database.Tracks.Where(t => t.Name == value).ToList()
            };
            ViewBag.SearchingValue = value;
            return View(results);
        }

        public ActionResult SearchArtists(string value, int page)
        {
            List<Artist> artists = parser.SearchArtists(value, page, newSearchElements);
            return PartialView(artists);
        }

        public ActionResult SearchAlbums(string value, int page)
        {
            List<Album> albums = parser.SearchAlbums(value, page, newSearchElements);
            return PartialView(albums);
        }

        public ActionResult SearchTracks(string value, int page)
        {
            List<Track> tracks = parser.SearchTracks(value, page, newSearchElements);
            return PartialView(tracks);
        }
    }
}
