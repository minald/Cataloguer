using Cataloguer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ActionResult ArtistProfile(string name, string pictureLink, string profileLink)
        {
            Artist artist = new LastFMParser().GetArtist(name, pictureLink, profileLink);
            return View(artist);
        }

        public ActionResult Local()
        {
            return View();
        }
    }
}