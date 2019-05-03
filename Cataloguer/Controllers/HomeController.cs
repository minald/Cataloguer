using Cataloguer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        Repository Database { get; set; } = new Repository();
        LastFMParser Parser { get; set; } = new LastFMParser();

        public const int FirstPage = 1;
        public int artistsPerPage = 48;
        public int albumsPerPage = 48;
        public int tracksPerPage = 48;
        public int newSearchElements = 8;

        public ActionResult Index()
        {
            List<Artist> artists = Parser.GetTopArtists(1, artistsPerPage);
            return View(artists);
        }

        public ActionResult TopArtists(int page)
        {
            List<Artist> artists = Parser.GetTopArtists(page, artistsPerPage);
            return PartialView("PartialArtists", artists);
        }

        public ActionResult ArtistProfile(string name)
        {
            Artist artist = Parser.GetArtist(name);
            return View(artist);
        }

        public ActionResult ArtistBiography(string name)
        {
            Artist artist = Parser.GetArtistWithBiography(name);
            return View(artist);
        }

        public ActionResult ArtistAllTracks(string name)
        {
            Artist artist = Parser.GetArtistWithAllTracks(name);
            return View(artist);
        }
        
        public ActionResult ArtistTracks(string name, int page)
        {
            List<Track> tracks = Parser.GetTracksOfArtist(name, page, tracksPerPage);
            return PartialView("PartialTracksInPanels", tracks);
        }

        public ActionResult ArtistAllAlbums(string name)
        {
            Artist artist = Parser.GetArtistWithAllAlbums(name);
            return View(artist);
        }

        public ActionResult ArtistAlbums(string name, int page)
        {
            List<Album> albums = Parser.GetAlbumsOfArtist(name, page, albumsPerPage);
            return PartialView("PartialAlbums", albums);
        }

        public ActionResult Album(string albumName, string artistName)
        {
            Album album = Parser.GetAlbum(albumName, artistName);
            return View(album);
        }

        public ActionResult Track(string trackName, string artistName)
        {
            Track track = Parser.GetTrack(trackName, artistName);
            return View(track);
        }

        [HttpGet]
        public ActionResult Search(string value)
        {
            SearchingResults results = new SearchingResults
            {
                LastFMArtists = Parser.SearchArtists(value, FirstPage, newSearchElements),
                LastFMAlbums = Parser.SearchAlbums(value, FirstPage, newSearchElements),
                LastFMTracks = Parser.SearchTracks(value, FirstPage, newSearchElements),
                LocalArtists = Database.GetArtistsByName(value),
                LocalAlbums = Database.GetAlbumsByName(value),
                LocalTracks = Database.GetTracksByName(value)
            };
            ViewBag.SearchingValue = value;
            return View(results);
        }

        public ActionResult SearchArtists(string value, int page)
        {
            List<Artist> artists = Parser.SearchArtists(value, page, newSearchElements);
            return PartialView("PartialArtists", artists);
        }

        public ActionResult SearchAlbums(string value, int page)
        {
            List<Album> albums = Parser.SearchAlbums(value, page, newSearchElements);
            return PartialView("PartialAlbums", albums);
        }

        public ActionResult SearchTracks(string value, int page)
        {
            List<Track> tracks = Parser.SearchTracks(value, page, newSearchElements);
            return PartialView("PartialTracksInPanels", tracks);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
