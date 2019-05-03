using Cataloguer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cataloguer.Controllers
{
    public class LocalController : Controller
    {
        Repository database = new Repository();

        public ActionResult Index()
        {
            return View(database.GetArtists());
        }

        public ActionResult ArtistProfile(string name)
        {
            return View(database.GetArtist(name));
        }

        public ActionResult Album(string albumName, string artistName)
        {
            return View(database.GetAlbum(albumName, artistName));
        }
    }
}
