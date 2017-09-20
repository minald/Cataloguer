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
            HtmlDocument musicPage = htmlWeb.Load("https://www.last.fm/music");
            var untreatedArtists = musicPage.DocumentNode.SelectNodes("//*[@class='music-charts']/div/div[2]/table/tbody/tr[contains(@class, 'js-link-block')]");
            foreach (var untreatedArtist in untreatedArtists)
            {
                string profileLink = "https://www.last.fm" +
                    untreatedArtist.SelectSingleNode(".//td[3]/a").Attributes["href"].Value;
                string name = untreatedArtist.SelectSingleNode(".//td[3]/a").InnerText;
                string pictureLink = untreatedArtist.SelectSingleNode(".//td[2]/img").Attributes["src"].Value; 
                Artist artist = new Artist(name, pictureLink, profileLink);
                topArtists.Add(artist);
            }
            return topArtists;
        }

        public Artist GetArtist(string artistName, string artistPictureLink, string artistProfileLink)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument profilePage = htmlWeb.Load(artistProfileLink);
            HtmlNode documentNode = profilePage.DocumentNode;
            string xpathToScrobbles = "//*[@id='content']/div[2]/header/div[3]/div/div[2]/div[2]/ul/li[1]/p/abbr";
            string scrobbles = documentNode.SelectSingleNode(xpathToScrobbles).Attributes["title"].Value;
            string xpathToListeners = "//*[@id='content']/div[2]/header/div[3]/div/div[2]/div[2]/ul/li[2]/p/abbr";
            string listeners = documentNode.SelectSingleNode(xpathToListeners).Attributes["title"].Value;
            string xpathToAlbums = "//*[@id='mantle_skin']/div[4]/div/div[1]/section[4]/div/ol/li";
            List<Album> albums = GetTopAlbumsOfArtist(documentNode, xpathToAlbums);
            string xpathToTracks = "//*[@id='top-tracks-section']/div/table/tbody/tr";
            List<Track> tracks = GetTopTracksOfArtist(documentNode, xpathToTracks);
            string xpathToTags = "//*[@id='mantle_skin']/div[4]/div/div[1]/section[1]/ul/li";
            List<string> tags = GetTagsOfArtist(documentNode, xpathToTags);
            Artist artist = new Artist(artistName, artistPictureLink, artistProfileLink, scrobbles, listeners, albums, tracks, tags);
            return artist;
        }

        private List<Album> GetTopAlbumsOfArtist(HtmlNode documentNode, string xpathToAlbums)
        {
            List<Album> albums = new List<Album>();
            var untreatedAlbums = documentNode.SelectNodes(xpathToAlbums);
            foreach(var untreatedAlbum in untreatedAlbums)
            {
                string name = untreatedAlbum.SelectSingleNode(".//div/div[2]/p/a").InnerText;
                string pictureLink = untreatedAlbum.SelectSingleNode(".//div/div/img").Attributes["src"].Value;
                string listeners = untreatedAlbum.SelectSingleNode(".//div/div[2]/p[2]").InnerText;
                Album album = new Album(name, pictureLink, listeners);
                albums.Add(album);
            }
            return albums;
        }

        private List<Track> GetTopTracksOfArtist(HtmlNode documentNode, string xpathToTracks)
        {
            List<Track> tracks = new List<Track>();
            var untreatedTracks = documentNode.SelectNodes(xpathToTracks);
            foreach (var untreatedTrack in untreatedTracks)
            {
                int trackRating = Convert.ToInt32(untreatedTrack.SelectSingleNode(".//td").InnerText);
                string trackName = untreatedTrack.SelectSingleNode(".//td[4]/span/a").InnerText;
                string trackListeners = untreatedTrack.SelectSingleNode(".//td[7]/span/span/span").InnerText;
                Track track = new Track(trackRating, trackName, trackListeners);
                tracks.Add(track);
            }
            return tracks;
        }

        private List<string> GetTagsOfArtist(HtmlNode documentNode, string xpathToTags)
        {
            var untreatedTags = documentNode.SelectNodes(xpathToTags);
            List<string> tags = new List<string>();
            foreach (var untreatedTag in untreatedTags)
            {
                string tag = untreatedTag.SelectSingleNode(".//a").InnerText;
                tags.Add(tag);
            }
            return tags;
        }

        public Artist GetArtistWithAllTracks(string artistName, string artistPictureLink, string linkToPageWithTopTracks)
        {
            List<Track> allTracks = new List<Track>();
            HtmlWeb htmlWeb = new HtmlWeb();
            for(int pageNumber = 1; pageNumber <= 10; pageNumber++)
            {
                string linkToCurrentPageWithTopTracks = linkToPageWithTopTracks + "&page=" + pageNumber;
                HtmlDocument currentPageWithTopTracks = htmlWeb.Load(linkToCurrentPageWithTopTracks);
                string xpathToTracks = "//*[@id='mantle_skin']/div[4]/div/div[1]/section/table/tbody/tr[contains(@class, 'js-link-block')]";
                var untreatedTracks = currentPageWithTopTracks.DocumentNode.SelectNodes(xpathToTracks);
                foreach (var untreatedTrack in untreatedTracks)
                {
                    string str = untreatedTrack.SelectSingleNode(".//td").InnerText;
                    int trackRating = Convert.ToInt32(str);
                    string trackName = untreatedTrack.SelectSingleNode(".//td[4]/span/a").InnerText;
                    string trackListeners = untreatedTrack.SelectSingleNode(".//td[7]/span/span/span").InnerText;
                    Track track = new Track(trackRating, trackName, trackListeners);
                    allTracks.Add(track);
                }
            }
            Artist artist = new Artist(artistName, artistPictureLink, allTracks);
            return artist;
        }
    }
}
