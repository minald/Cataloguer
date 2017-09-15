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
            ArtistContext db = new ArtistContext();
            db.Artists.Add(new Artist { Name="ABC", PictureLink="QWE", Scrobbles = 2, Listeners = 1});
            db.SaveChanges();
            return View();
        }

        public ActionResult Local()
        {
            return View();
        }
    }
}