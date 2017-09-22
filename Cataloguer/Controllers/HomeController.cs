using Cataloguer.Models;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;

namespace Cataloguer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string ApiKey = "f39425750fc23d743fbf853d9585a46c";
            //string url = "http://ws.audioscrobbler.com/2.0/?method=chart.gettopartist&api_key=" + ApiKey;
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.
                    Create("http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists&api_key=" 
                + ApiKey/* + "&format=json"*/);
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string tokenResult = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            XmlDocument document = new XmlDocument();
            Artist artist = new Artist(tokenResult);
            //List<Artist> artists = new LastFMParser().GetTopArtists();
            return View(artist/*artists*/);
        }

        public ActionResult ArtistProfile(string artistName, string artistPictureLink, string artistProfileLink)
        {
            Artist artist = new LastFMParser().GetArtist(artistName, artistProfileLink, artistPictureLink);
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

        public ActionResult ArtistBiography(string artistName, string artistPictureLink, string artistProfileLink)
        {
            Artist artist = new LastFMParser().
                GetArtistWithBiography(artistName, artistProfileLink, artistPictureLink);
            return View(artist);
        }

        public ActionResult Local()
        {
            return View();
        }
    }
}
