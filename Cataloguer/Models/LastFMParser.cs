using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cataloguer.Models
{
    public class LastFMParser
    {
        public List<Artist> GetTopArtists()
        {
            List<Artist> topArtists = new List<Artist>();
            HtmlWeb htmlWeb = new HtmlWeb();
            var musicPage = htmlWeb.Load("https://www.last.fm/music");
            var artists = musicPage.DocumentNode.SelectNodes("//*[@class='music-charts']/div/div[2]/table/tbody/tr");
            foreach (var element in artists)
            {
                string elementClassWithoutSpaces = element.Attributes["class"].Value.Replace(" ", "").Replace("\n", "");
                if (elementClassWithoutSpaces == "js-link-blockglobalchart-item")
                {
                    string profileLink = "https://www.last.fm" +
                        element.SelectSingleNode(".//td[3]/a").Attributes["href"].Value;
                    string name = element.SelectSingleNode(".//td[3]/a").InnerText;
                    string pictureLink = element.SelectSingleNode(".//td[2]/img").Attributes["src"].Value; 
                    Artist artist = new Artist(name, pictureLink, profileLink);
                    topArtists.Add(artist);
                }
            }
            return topArtists;
        }

        public Artist GetArtist(string name, string pictureLink, string profileLink)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            var profilePage = htmlWeb.Load(profileLink);
            string xpathToScrobbles = "//*[@id='content']/div[2]/header/div[3]/div/div[2]/div[2]/ul/li[1]/p/abbr";
            string scrobbles = profilePage.DocumentNode.
                SelectSingleNode(xpathToScrobbles).Attributes["title"].Value;
            string xpathToListeners = "//*[@id='content']/div[2]/header/div[3]/div/div[2]/div[2]/ul/li[2]/p/abbr";
            string listeners = profilePage.DocumentNode.
                SelectSingleNode(xpathToListeners).Attributes["title"].Value;

            string xpathToTags = "//*[@id='mantle_skin']/div[4]/div/div[1]/section[1]/ul/li";
            var untreatedTags = profilePage.DocumentNode.SelectNodes(xpathToTags);
            List<string> tags = new List<string>();
            foreach (var untreatedTag in untreatedTags)
            {
                string tag = untreatedTag.SelectSingleNode(".//a").InnerText;
                tags.Add(tag);
            }

            string xpathToTracks = "//*[@id='top-tracks-section']/div/table/tbody/tr";
            var untreatedTracks = profilePage.DocumentNode.SelectNodes(xpathToTracks);
            List<Track> tracks = new List<Track>();
            foreach (var untreatedTrack in untreatedTracks)
            {
                int trackRating = Convert.ToInt32(untreatedTrack.SelectSingleNode(".//td").InnerText);
                string trackName = untreatedTrack.SelectSingleNode(".//td[4]/span/a").InnerText;
                string trackListeners = untreatedTrack.SelectSingleNode(".//td[7]/span/span/span").InnerText;
                Track track = new Track(trackRating, trackName, trackListeners);
                tracks.Add(track);
            }

            Artist artist = new Artist(name, pictureLink, profileLink, scrobbles, listeners, tracks, tags);
            return artist;
        }
    }
}