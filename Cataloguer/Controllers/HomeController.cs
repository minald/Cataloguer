using Cataloguer.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        LastFMParser parser = new LastFMParser();

        public int artistsPerPage = 48;

        public int albumsPerPage = 48;

        public int tracksPerPage = 50;

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
                Artists = parser.SearchArtists(value, 1, 8),
                Albums = parser.SearchAlbums(value, 1, 8),
                Tracks = parser.SearchTracks(value, 1, 8)
            };
            ViewBag.SearchingValue = value;
            return View(results);
        }

        public ActionResult SearchArtists(string value, int page)
        {
            List<Artist> artists = parser.SearchArtists(value, page, 8);
            return PartialView(artists);
        }

        public ActionResult SearchAlbums(string value, int page)
        {
            List<Album> albums = parser.SearchAlbums(value, page, 8);
            return PartialView(albums);
        }

        public ActionResult SearchTracks(string value, int page)
        {
            List<Track> tracks = parser.SearchTracks(value, page, 8);
            return PartialView(tracks);
        }
    }
}
