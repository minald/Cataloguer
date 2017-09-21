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
            Artist artist = new LastFMParser().GetArtist(artistName, artistPictureLink, artistProfileLink);
            return View(artist);
        }

        public ActionResult ArtistAllTracks(string artistName, string artistPictureLink, string artistProfileLink)
        {
            string linkToPageWithAllTracks = artistProfileLink + "/+tracks?date_preset=ALL";
            Artist artist = new LastFMParser().
                GetArtistWithAllTracks(artistName, artistPictureLink, artistProfileLink, linkToPageWithAllTracks);
            return View(artist);
        }

        public ActionResult ArtistAllAlbums(string artistName, string artistPictureLink, string artistProfileLink)
        {
            string linkToPageWithAllAlbums = artistProfileLink + "/+albums";
            Artist artist = new LastFMParser().
                GetArtistWithAllAlbums(artistName, artistPictureLink, artistProfileLink, linkToPageWithAllAlbums);
            return View(artist);
        }

        public ActionResult ArtistBiography(string artistName, string artistPictureLink, string artistProfileLink)
        {
            string linkToBiographyPage = artistProfileLink + "/+wiki";
            Artist artist = new LastFMParser().GetArtistWithBiography(artistName,
                artistPictureLink, artistProfileLink, linkToBiographyPage);
            return View(artist);
        }

        public ActionResult Local()
        {
            return View();
        }
    }
}
