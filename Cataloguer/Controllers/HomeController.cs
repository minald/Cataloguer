﻿using Cataloguer.Models;
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

        public ActionResult ArtistProfile(string artistName, string artistPictureLink, string artistProfileLink)
        {
            Artist artist = new LastFMParser().GetArtist(artistName, artistPictureLink, artistProfileLink);
            return View(artist);
        }

        public ActionResult ArtistTopTracks(string artistName, string artistPictureLink, string artistProfileLink)
        {
            string linkToPageWithAllTracks = artistProfileLink + "/+tracks?date_preset=ALL";
            Artist artist = new LastFMParser().GetArtistWithAllTracks(artistName, artistPictureLink, linkToPageWithAllTracks);
            return View(artist);
        }

        public ActionResult ArtistTopAlbums(string artistName, string artistPictureLink, string artistProfileLink)
        {
            string linkToPageWithAllAlbums = artistProfileLink + "/+albums";
            Artist artist = new LastFMParser().GetArtistWithAllAlbums(artistName, artistPictureLink, linkToPageWithAllAlbums);
            return View(artist);
        }

        //TODO : Parse biography

        public ActionResult Local()
        {
            return View();
        }
    }
}
