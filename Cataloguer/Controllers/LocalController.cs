using Cataloguer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cataloguer.Controllers
{
    public class LocalController : Controller
    {
        private Repository Db { get; set; }

        public LocalController(Repository repository) => Db = repository;

        public ActionResult Index() => View(Db.GetArtists());

        public ActionResult ArtistProfile(string name) => View(Db.GetArtist(name));

        public ActionResult Album(string albumName, string artistName) => View(Db.GetAlbum(albumName, artistName));
    }
}
