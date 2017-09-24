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

        public ActionResult ArtistProfile(string artistName, string artistPictureLink, string artistProfileLink)
        {
            Artist artist = new LastFMParser().GetArtist(artistName, artistProfileLink, artistPictureLink);
            return View(artist);
        }

        public ActionResult ArtistBiography(string artistName, string artistPictureLink, string artistProfileLink)
        {
            Artist artist = new LastFMParser().
                GetArtistWithBiography(artistName, artistProfileLink, artistPictureLink);
            return View(artist);
        }

        public ActionResult ArtistAllTracks(string artistName, string artistPictureLink, string artistProfileLink)
        {
            Artist artist = new LastFMParser().
                GetArtistWithAllTracks(artistName, artistProfileLink, artistPictureLink);
            return View(artist);
        }

        public ActionResult ArtistAllAlbums(string artistName, string artistPictureLink, string artistProfileLink)
        {
            Artist artist = new LastFMParser().
                GetArtistWithAllAlbums(artistName, artistProfileLink, artistPictureLink);
            return View(artist);
        }

        public ActionResult Album(string albumName, string artistName)
        {
            Album album = new LastFMParser().GetAlbum(albumName, artistName);
            return View(album);
        }

        public ActionResult Local()
        {
            return View();
        }
    }
}
