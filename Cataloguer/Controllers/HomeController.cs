using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cataloguer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        private readonly int itemsPerPage = 48;
        private readonly int newSearchElements = 8;

        public ActionResult Index()
            => View(LastFMParser.GetTopArtists(itemsPerPage));

        public ActionResult TopArtists(int page)
            => PartialView("_Artists", LastFMParser.GetTopArtists(page, itemsPerPage));

        public ActionResult Artist(string name)
            => View(LastFMParser.GetArtist(name));

        public ActionResult ArtistBiography(string name)
            => View(LastFMParser.GetArtistWithBiography(name));

        public ActionResult ArtistAllTracks(string name)
            => View(LastFMParser.GetArtistWithAllTracks(name));

        public ActionResult ArtistTracks(string name, int page)
            => PartialView("_Tracks", LastFMParser.GetTracksOfArtist(name, itemsPerPage, page));

        public ActionResult ArtistAllAlbums(string name)
            => View(LastFMParser.GetArtistWithAllAlbums(name));

        public ActionResult ArtistAlbums(string name, int page)
            => PartialView("_Albums", LastFMParser.GetAlbumsOfArtist(name, itemsPerPage, page));

        public ActionResult Album(string albumName, string artistName)
            => View(LastFMParser.GetAlbum(albumName, artistName));

        public ActionResult Track(string trackName, string artistName)
            => View(LastFMParser.GetTrack(trackName, artistName));

        public ActionResult Search(string value)
        {
            List<Artist> artists = LastFMParser.SearchArtists(value, newSearchElements).ToList();
            List<Album> albums = LastFMParser.SearchAlbums(value, newSearchElements).ToList();
            List<Track> tracks = LastFMParser.SearchTracks(value, newSearchElements).ToList();
            var results = new SearchingResults(artists, albums, tracks);
            ViewBag.SearchingValue = value;
            return View(results);
        }

        public ActionResult SearchArtists(string value, int page)
            => PartialView("_Artists", LastFMParser.SearchArtists(value, newSearchElements, page));

        public ActionResult SearchAlbums(string value, int page)
            => PartialView("_Albums", LastFMParser.SearchAlbums(value, newSearchElements, page));

        public ActionResult SearchTracks(string value, int page)
            => PartialView("_Tracks", LastFMParser.SearchTracks(value, newSearchElements, page));

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel(Activity.Current?.Id ?? HttpContext.TraceIdentifier));

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
