using Cataloguer.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Artist> artists = new LastFMParser().GetTopArtists();
            return View(artists);
        }

        public ActionResult ArtistProfile(string artistName)
        {
            Artist artist = new LastFMParser().GetArtist(artistName);
            return View(artist);
        }

        public ActionResult ArtistBiography(string artistName)
        {
            Artist artist = new LastFMParser().GetArtistWithBiography(artistName);
            return View(artist);
        }

        public ActionResult ArtistAllTracks(string artistName, string artistPictureLink)
        {
            Artist artist = new LastFMParser().
                GetArtistWithAllTracks(artistName, artistPictureLink);
            return View(artist);
        }

        public ActionResult ArtistAllAlbums(string artistName, string artistPictureLink)
        {
            Artist artist = new LastFMParser().
                GetArtistWithAllAlbums(artistName, artistPictureLink);
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
                Artists = new LastFMParser().SearchArtists(value),
                Albums = new LastFMParser().SearchAlbums(value),
                Tracks = new LastFMParser().SearchTracks(value)
            };
            ViewBag.SearchingValue = value;
            return View(results);
        }
    }
}
