using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class LastFMParser
    {
        public List<Artist> GetTopArtists()
        {
            List<Artist> topArtists = new List<Artist>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument musicPage = htmlWeb.Load("https://www.last.fm/music");
            HtmlNodeCollection untreatedArtists = musicPage.DocumentNode.SelectNodes("//*[@class='music-charts']/div/div[2]/table/tbody/tr[contains(@class, 'js-link-block')]");
            foreach (HtmlNode untreatedArtist in untreatedArtists)
            {
                string profileLink = "https://www.last.fm" +
                    untreatedArtist.SelectSingleNode(".//td[3]/a").Attributes["href"].Value;
                string name = untreatedArtist.SelectSingleNode(".//td[3]/a").InnerText;
                string pictureLink = untreatedArtist.SelectSingleNode(".//td[2]/img").Attributes["src"].Value;
                Artist artist = new Artist.Builder(name, profileLink).PictureLink(pictureLink).Build();
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
            string xpathToShortBiography = "//*[@id='mantle_skin']/div[4]/div/div[1]/section[2]/p";
            string shortBiography = documentNode.SelectSingleNode(xpathToShortBiography).InnerText;
            string xpathToAlbums = "//*[@id='mantle_skin']/div[4]/div/div[1]/section[4]/div/ol/li";
            List<Album> albums = GetTopAlbumsOfArtist(documentNode, xpathToAlbums);
            string xpathToTracks = "//*[@id='top-tracks-section']/div/table/tbody/tr";
            List<Track> tracks = GetTopTracksOfArtist(documentNode, xpathToTracks);
            string xpathToTags = "//*[@id='mantle_skin']/div[4]/div/div[1]/section[1]/ul/li";
            List<string> tags = GetTagsOfArtist(documentNode, xpathToTags);
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).Scrobbles(scrobbles).Listeners(listeners).
                ShortBiography(shortBiography).Albums(albums).Tracks(tracks).Tags(tags).Build();
            return artist;
        }

        private List<Album> GetTopAlbumsOfArtist(HtmlNode documentNode, string xpathToAlbums)
        {
            List<Album> albums = new List<Album>();
            HtmlNodeCollection untreatedAlbums = documentNode.SelectNodes(xpathToAlbums);
            foreach(HtmlNode untreatedAlbum in untreatedAlbums)
            {
                string name = untreatedAlbum.SelectSingleNode(".//div/div[2]/p/a").InnerText;
                string pictureLink = untreatedAlbum.SelectSingleNode(".//div/div/img").Attributes["src"].Value;
                string listeners = untreatedAlbum.SelectSingleNode(".//div/div[2]/p[2]").InnerText;
                Album album = new Album.Builder(name).PictureLink(pictureLink).
                    Listeners(listeners).Build();
                albums.Add(album);
            }
            return albums;
        }

        private List<Track> GetTopTracksOfArtist(HtmlNode documentNode, string xpathToTracks)
        {
            List<Track> tracks = new List<Track>();
            HtmlNodeCollection untreatedTracks = documentNode.SelectNodes(xpathToTracks);
            foreach (HtmlNode untreatedTrack in untreatedTracks)
            {
                int trackRating = Convert.ToInt32(untreatedTrack.SelectSingleNode(".//td").InnerText);
                string trackName = untreatedTrack.SelectSingleNode(".//td[4]/span/a").InnerText;
                string trackListeners = untreatedTrack.SelectSingleNode(".//td[7]/span/span/span").InnerText;
                Track track = new Track.Builder(trackName).Rating(trackRating).
                    Listeners(trackListeners).Build();
                tracks.Add(track);
            }
            return tracks;
        }

        private List<string> GetTagsOfArtist(HtmlNode documentNode, string xpathToTags)
        {
            List<string> tags = new List<string>();
            HtmlNodeCollection untreatedTags = documentNode.SelectNodes(xpathToTags);
            foreach (HtmlNode untreatedTag in untreatedTags)
            {
                string tag = untreatedTag.SelectSingleNode(".//a").InnerText;
                tags.Add(tag);
            }
            return tags;
        }

        //TODO : Delete last fourth parameter in 3 following "GetArtistWith" functions

        public Artist GetArtistWithAllTracks(string artistName, string artistPictureLink,
            string artistProfileLink, string linkToPageWithAllTracks)
        {
            List<Track> allTracks = new List<Track>();
            HtmlWeb htmlWeb = new HtmlWeb();
            for(int pageNumber = 1; pageNumber <= 10; pageNumber++)
            {
                string linkToCurrentPageWitAllTracks = linkToPageWithAllTracks + "&page=" + pageNumber;
                HtmlDocument currentPageWithAllTracks = htmlWeb.Load(linkToCurrentPageWitAllTracks);
                string xpathToTracks = "//*[@id='mantle_skin']/div[4]/div/div[1]/section/table/tbody/tr[contains(@class, 'js-link-block')]";
                HtmlNodeCollection untreatedTracks = currentPageWithAllTracks.DocumentNode.SelectNodes(xpathToTracks);
                foreach (HtmlNode untreatedTrack in untreatedTracks)
                {
                    int trackRating = Convert.ToInt32(untreatedTrack.SelectSingleNode(".//td").InnerText);
                    string trackName = untreatedTrack.SelectSingleNode(".//td[4]/span/a").InnerText;
                    string trackListeners = untreatedTrack.SelectSingleNode(".//td[7]/span/span/span").InnerText;
                    Track track = new Track.Builder(trackName).Rating(trackRating).
                        Listeners(trackListeners).Build();
                    allTracks.Add(track);
                }
            }
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).Tracks(allTracks).Build();
            return artist;
        }

        public Artist GetArtistWithAllAlbums(string artistName, string artistPictureLink,
            string artistProfileLink, string linkToPageWithAllAlbums)
        {
            List<Album> allAlbums = new List<Album>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument mainPageWithAllAlbums = htmlWeb.Load(linkToPageWithAllAlbums);
            int pagesQuantity = CountQuantityOfPages(mainPageWithAllAlbums);
            for (int pageNumber = 1; pageNumber <= pagesQuantity; pageNumber++)
            {
                string linkToCurrentPageWithAllAlbums = linkToPageWithAllAlbums + "?page=" + pageNumber;
                HtmlDocument currentPageWithAllAlbums = htmlWeb.Load(linkToCurrentPageWithAllAlbums);
                string xpathToAlbums = "//*[@id='artist-albums-section']/ol/li[@itemscope]";
                HtmlNodeCollection untreatedAlbums = currentPageWithAllAlbums.DocumentNode.SelectNodes(xpathToAlbums);
                foreach (HtmlNode untreatedAlbum in untreatedAlbums)
                {
                    string name = untreatedAlbum.SelectSingleNode(".//div/h3/a").InnerText;
                    string pictureLink = untreatedAlbum.SelectSingleNode(".//div/img").Attributes["src"].Value;
                    string listeners = untreatedAlbum.SelectSingleNode(".//div/p").InnerText;
                    Album unfinishedAlbum = new Album.Builder(name).PictureLink(pictureLink).
                        Listeners(listeners).Build();
                    Album album = InitializeRunningLenghtAndReleaseDate(unfinishedAlbum, untreatedAlbum);
                    allAlbums.Add(album);
                }
            }
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).Albums(allAlbums).Build();
            return artist;
        }

        private int CountQuantityOfPages(HtmlDocument mainPageWithAllAlbums)
        {
            HtmlNode node = mainPageWithAllAlbums.DocumentNode.
                SelectSingleNode("//*[@id='artist-albums-section']/nav/ul/li[contains(@class, 'pagination-page')][last()]");
            int count = Convert.ToInt32(node.SelectSingleNode(".//a").InnerText);
            return count;
        }

        private Album InitializeRunningLenghtAndReleaseDate(Album unfinishedAlbum, HtmlNode untreatedAlbum)
        {
            Album album = unfinishedAlbum;
            string runningLenght = "", releaseDate = "";
            HtmlNode runningLenghtAndReleaseDateNode = untreatedAlbum.SelectSingleNode(".//div/p[2]");
            if (runningLenghtAndReleaseDateNode != null)
            {
                string runningLenghtAndReleaseDate = runningLenghtAndReleaseDateNode.InnerText;
                int separatorIndex = runningLenghtAndReleaseDate.IndexOf("·");
                if (separatorIndex == -1)
                {
                    if (runningLenghtAndReleaseDate.Contains("track"))
                    {
                        runningLenght = runningLenghtAndReleaseDate;
                    }
                    else
                    {
                        releaseDate = runningLenghtAndReleaseDate;
                    }
                }
                else
                {
                    runningLenght = runningLenghtAndReleaseDate.Substring(0, separatorIndex);
                    releaseDate = runningLenghtAndReleaseDate.Substring(separatorIndex + 1);
                }
            }
            album.RunningLenght = runningLenght;
            album.ReleaseDate = releaseDate;
            return album;
        }

        public Artist GetArtistWithBiography(string artistName, string artistPictureLink, 
            string artistProfileLink, string linkToBiographyPage)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument biographyPage = htmlWeb.Load(linkToBiographyPage);
            string xpathToFullBiography = "//*[@id='mantle_skin']/div[4]/div/div[1]/div[1]/div";
            HtmlNode untreatedFullBiography = biographyPage.DocumentNode.SelectSingleNode(xpathToFullBiography);
            string fullBiography = untreatedFullBiography.InnerText;
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).FullBiography(fullBiography).Build();
            return artist;
        }
    }
}
