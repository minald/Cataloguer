using System.Linq;
using System.Web.Mvc;
using Cataloguer.Models;

namespace Cataloguer.Controllers
{
    public class LocalController : Controller
    {
        public ArtistContext database = new ArtistContext();

        public ActionResult Index()
        {
            return View(database.Artists.ToList());
        }

        public ActionResult ArtistProfile(string artistName)
        {
            return View(database.Artists.Where(a => a.Name == artistName).First());
        }

        public ActionResult Album(string albumName, string artistName)
        {
            return View(database.Artists.Where(a => a.Name == artistName).First().
                Albums.Where(a => a.Name == albumName).First());
        }
    }
}