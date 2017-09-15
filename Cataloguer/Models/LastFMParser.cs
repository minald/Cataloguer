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
            if (artists != null)
            {
                foreach (var element in artists)
                {
                    if(element.SelectSingleNode(".//td[3]/a") != null)
                    {
                        string linkToArtistProfile = "https://www.last.fm" + element.SelectSingleNode(".//td[3]/a").Attributes["href"].Value;
                        var artistProfilePage = htmlWeb.Load(linkToArtistProfile);
                        string name = element.SelectSingleNode(".//td[3]/a").InnerText;
                        string pictureLink = element.SelectSingleNode(".//td[2]/img").Attributes["src"].Value;
                        //string xpathToScrobbles = "//*[@id='content']/div[4]/header/div[3]/div/div[2]/div[2]/ul/li[1]/p/abbr";
                        string scrobbles = artistProfilePage.DocumentNode.
                            SelectSingleNode("//*[@class='intabbr']").Attributes["title"].Value;
                        Artist artist = new Artist(name, pictureLink, scrobbles, "");
                        topArtists.Add(artist);
                    }
                }
            }
            return topArtists;
        }
    }
}