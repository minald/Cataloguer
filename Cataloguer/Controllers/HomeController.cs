using Cataloguer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        Repository Database { get; set; }

        public HomeController(Repository repository)
        {
            Database = repository;
        }

        public const int FirstPage = 1;
        public int artistsPerPage = 48;
        public int albumsPerPage = 48;
        public int tracksPerPage = 48;
        public int newSearchElements = 8;

        public ActionResult Index()
        {
            List<Artist> artists = LastFMParser.GetTopArtists(1, artistsPerPage);
            return View(artists);
        }

        public ActionResult TopArtists(int page)
        {
            List<Artist> artists = LastFMParser.GetTopArtists(page, artistsPerPage);
            return PartialView("PartialArtists", artists);
        }

        public ActionResult ArtistProfile(string name)
        {
            Artist artist = LastFMParser.GetArtist(name);
            return View(artist);
        }

        public ActionResult ArtistBiography(string name)
        {
            Artist artist = LastFMParser.GetArtistWithBiography(name);
            return View(artist);
        }

        public ActionResult ArtistAllTracks(string name)
        {
            Artist artist = LastFMParser.GetArtistWithAllTracks(name);
            return View(artist);
        }
        
        public ActionResult ArtistTracks(string name, int page)
        {
            List<Track> tracks = LastFMParser.GetTracksOfArtist(name, page, tracksPerPage);
            return PartialView("PartialTracksInPanels", tracks);
        }

        public ActionResult ArtistAllAlbums(string name)
        {
            Artist artist = LastFMParser.GetArtistWithAllAlbums(name);
            return View(artist);
        }

        public ActionResult ArtistAlbums(string name, int page)
        {
            List<Album> albums = LastFMParser.GetAlbumsOfArtist(name, page, albumsPerPage);
            return PartialView("PartialAlbums", albums);
        }

        public ActionResult Album(string albumName, string artistName)
        {
            Album album = LastFMParser.GetAlbum(albumName, artistName);
            return View(album);
        }

        public ActionResult Track(string trackName, string artistName)
        {
            Track track = LastFMParser.GetTrack(trackName, artistName);
            return View(track);
        }

        [HttpGet]
        public ActionResult Search(string value)
        {
            var artists = LastFMParser.SearchArtists(value, FirstPage, newSearchElements);
            var albums = LastFMParser.SearchAlbums(value, FirstPage, newSearchElements);
            var tracks = LastFMParser.SearchTracks(value, FirstPage, newSearchElements);
            var results = new SearchingResults(artists, albums, tracks);
            ViewBag.SearchingValue = value;
            return View(results);
        }

        public ActionResult SearchArtists(string value, int page)
        {
            List<Artist> artists = LastFMParser.SearchArtists(value, page, newSearchElements);
            return PartialView("PartialArtists", artists);
        }

        public ActionResult SearchAlbums(string value, int page)
        {
            List<Album> albums = LastFMParser.SearchAlbums(value, page, newSearchElements);
            return PartialView("PartialAlbums", albums);
        }

        public ActionResult SearchTracks(string value, int page)
        {
            List<Track> tracks = LastFMParser.SearchTracks(value, page, newSearchElements);
            return PartialView("PartialTracksInPanels", tracks);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
