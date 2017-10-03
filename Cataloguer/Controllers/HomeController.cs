using Cataloguer.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        public int artistsPerPage = 48;

        public ActionResult Index()
        {
            Session["currentPage"] = 1;
            List<Artist> artists = new LastFMParser().
                GetTopArtists(Session["currentPage"].ToString(), artistsPerPage);
            return View(artists);
        }

        public ActionResult Artists()
        {
            Session["currentPage"] = Convert.ToInt32(Session["currentPage"]) + 1;
            List<Artist> artists = new LastFMParser().
                GetTopArtists(Session["currentPage"].ToString(), artistsPerPage);
            return PartialView(artists);
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
                Artists = new LastFMParser().SearchArtists(value, 10),
                Albums = new LastFMParser().SearchAlbums(value, 30),
                Tracks = new LastFMParser().SearchTracks(value, 50)
            };
            ViewBag.SearchingValue = value;
            return View(results);
        }
    }
}
