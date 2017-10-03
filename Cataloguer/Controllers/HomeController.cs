using Cataloguer.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        public int artistsPerPage = 48;

        public ActionResult Index()
        {
            List<Artist> artists = new LastFMParser().GetTopArtists(1, artistsPerPage);
            return View(artists);
        }

        public ActionResult TopArtists(int page)
        {
            List<Artist> artists = new LastFMParser().GetTopArtists(page, artistsPerPage);
            return PartialView(artists);
        }

        public ActionResult ArtistProfile(string name)
        {
            Artist artist = new LastFMParser().GetArtist(name);
            return View(artist);
        }

        public ActionResult ArtistBiography(string name)
        {
            Artist artist = new LastFMParser().GetArtistWithBiography(name);
            return View(artist);
        }

        public ActionResult ArtistAllTracks(string name)
        {
            Artist artist = new LastFMParser().GetArtistWithAllTracks(name);
            return View(artist);
        }

        public ActionResult ArtistAllAlbums(string name)
        {
            Artist artist = new LastFMParser().GetArtistWithAllAlbums(name);
            return View(artist);
        }

        public ActionResult Album(string albumName, string artistName)
        {
            Album album = new LastFMParser().GetAlbum(albumName, artistName);
            return View(album);
        }

        [HttpGet]
        public ActionResult Search(string value)
        {
            SearchingResults results = new SearchingResults
            {
                Artists = new LastFMParser().SearchArtists(value, 10),
                Albums = new LastFMParser().SearchAlbums(value, 30),
                Tracks = new LastFMParser().SearchTracks(value, 50)
            };
            ViewBag.SearchingValue = value;
            return View(results);
        }
    }
}
