using Cataloguer.Models;
using Cataloguer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        private readonly int itemsPerPage = 48;
        private readonly int newSearchElements = 8;

        private Repository Repository { get; set; }

        public HomeController(Repository repository) => Repository = repository;

        public ActionResult Index()
        {
            IEnumerable<Artist> artists = LastFMParser.GetTopArtists(itemsPerPage);
            artists.ToList().ForEach(a => Repository.InsertOrUpdate(a));
            return View(artists);
        }

        public ActionResult TopArtists(int page)
        {
            IEnumerable<Artist> artists = LastFMParser.GetTopArtists(page, itemsPerPage);
            artists.ToList().ForEach(a => Repository.InsertOrUpdate(a));
            return PartialView("_Artists", LastFMParser.GetTopArtists(page, itemsPerPage));
        }

        public ActionResult Artist(string name)
        {
            Artist artist = LastFMParser.GetArtist(name);
            Repository.InsertOrUpdate(artist);
            artist.Tracks.ForEach(t => Repository.InsertOrUpdate(t));
            return View(artist);
        }

        public ActionResult ArtistBiography(string name)
            => View(LastFMParser.GetArtistWithBiography(name));

        public ActionResult ArtistAllTracks(string name)
        {
            Artist artist = LastFMParser.GetArtistWithAllTracks(name);
            Repository.InsertOrUpdate(artist);
            artist.Tracks.ForEach(t => Repository.InsertOrUpdate(t));
            return View(artist);
        }

        public ActionResult ArtistTracks(string name, int page)
        {
            IEnumerable<Track> tracks = LastFMParser.GetTracksOfArtist(name, itemsPerPage, page);
            tracks.ToList().ForEach(t => Repository.InsertOrUpdate(t));
            return PartialView("_Tracks", tracks);
        }

        public ActionResult ArtistAllAlbums(string name)
            => View(LastFMParser.GetArtistWithAllAlbums(name));

        public ActionResult ArtistAlbums(string name, int page)
            => PartialView("_Albums", LastFMParser.GetAlbumsOfArtist(name, itemsPerPage, page));

        public ActionResult Album(string albumName, string artistName)
            => View(LastFMParser.GetAlbum(albumName, artistName));

        public ActionResult Track(string trackName, string artistName)
        {
            Track track = LastFMParser.GetTrack(trackName, artistName);
            Repository.InsertOrUpdate(track);
            return View(track);
        }

        public ActionResult DownloadTrack(string trackName, string artistName, [FromServices]IDownloader downloader)
        {
            Track track = LastFMParser.GetTrack(trackName, artistName);
            downloader.Download(trackName, artistName);
            return View("Track", track);
        }

        #region Search

        public ActionResult Search(string value)
        {
            List<Artist> artists = LastFMParser.SearchArtists(value, newSearchElements).ToList();
            artists.ForEach(a => Repository.InsertOrUpdate(a));
            List<Album> albums = LastFMParser.SearchAlbums(value, newSearchElements).ToList();
            List<Track> tracks = LastFMParser.SearchTracks(value, newSearchElements).ToList();
            tracks.ForEach(t => Repository.InsertOrUpdate(t));
            var results = new SearchingResults(artists, albums, tracks);
            ViewBag.SearchingValue = value;
            return View(results);
        }

        public ActionResult SearchArtists(string value, int page)
        {
            IEnumerable<Artist> artists = LastFMParser.SearchArtists(value, newSearchElements, page);
            artists.ToList().ForEach(a => Repository.InsertOrUpdate(a));
            return PartialView("_Artists", artists);
        }

        public ActionResult SearchAlbums(string value, int page) 
            => PartialView("_Albums", LastFMParser.SearchAlbums(value, newSearchElements, page));

        public ActionResult SearchTracks(string value, int page)
        {
            IEnumerable<Track> tracks = LastFMParser.SearchTracks(value, newSearchElements, page);
            tracks.ToList().ForEach(t => Repository.InsertOrUpdate(t));
            return PartialView("_Tracks", tracks);
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));
    }
}
