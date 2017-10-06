using System.Web.Mvc;
using Cataloguer.Models;

namespace Cataloguer.Controllers
{
    public class LocalController : Controller
    {
        MusicRepository database = new MusicRepository();

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
