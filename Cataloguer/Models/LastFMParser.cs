using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class LastFMParser
    {
        public List<Artist> GetTopArtists()
        {
            string linkToMusicPage = "https://www.last.fm/music";
            HtmlNode musicPageNode = GetPageNodeFrom(linkToMusicPage);
            HtmlNodeCollection nodesWithArtists = musicPageNode.SelectNodes(XPath.MusicPage.Artists);
            List<Artist> topArtists = ParseTopArtistsFrom(nodesWithArtists);
            return topArtists;
        }

        private List<Artist> ParseTopArtistsFrom(HtmlNodeCollection nodesWithArtists)
        {
            List<Artist> topArtists = new List<Artist>();
            foreach (HtmlNode nodeWithArtist in nodesWithArtists)
            {
                string name = nodeWithArtist.SelectSingleNode(".//td[3]/a").InnerText;
                string relativeProfileLink = nodeWithArtist.SelectSingleNode(".//td[3]/a").Attributes["href"].Value;
                string profileLink = "https://www.last.fm" + relativeProfileLink;
                string pictureLink = nodeWithArtist.SelectSingleNode(".//td[2]/img").Attributes["src"].Value;
                Artist artist = new Artist.Builder(name, profileLink).PictureLink(pictureLink).Build();
                topArtists.Add(artist);
            }
            return topArtists;
        }

        public Artist GetArtist(string artistName, string artistProfileLink, string artistPictureLink)
        {
            HtmlNode artistProfileNode = GetPageNodeFrom(artistProfileLink);
            string scrobbles = artistProfileNode.SelectSingleNode(XPath.ArtistProfilePage.Scrobbles).Attributes["title"].Value;
            string listeners = artistProfileNode.SelectSingleNode(XPath.ArtistProfilePage.Listeners).Attributes["title"].Value;
            List<string> tags = GetTagsOfArtist(artistProfileNode);
            string shortBiography = artistProfileNode.SelectSingleNode(XPath.ArtistProfilePage.ShortBiography).InnerText;
            List<Track> tracks = GetTopTracksOfArtist(artistProfileNode);
            List<Album> albums = GetTopAlbumsOfArtist(artistProfileNode);
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).Scrobbles(scrobbles).Listeners(listeners).
                Tags(tags).ShortBiography(shortBiography).Tracks(tracks).Albums(albums).Build();
            return artist;
        }

        private List<Album> GetTopAlbumsOfArtist(HtmlNode artistProfileNode)
        {
            HtmlNodeCollection nodesWithAlbums = artistProfileNode.SelectNodes(XPath.ArtistProfilePage.Albums);
            List<Album> albums = ParseTopAlbumsFrom(nodesWithAlbums);
            return albums;
        }

        private List<Album> ParseTopAlbumsFrom(HtmlNodeCollection nodesWithAlbums)
        {
            List<Album> albums = new List<Album>();
            foreach (HtmlNode nodeWithAlbum in nodesWithAlbums)
            {
                string name = nodeWithAlbum.SelectSingleNode(".//div/div[2]/p/a").InnerText;
                string pictureLink = nodeWithAlbum.SelectSingleNode(".//div/div/img").Attributes["src"].Value;
                string listeners = nodeWithAlbum.SelectSingleNode(".//div/div[2]/p[2]").InnerText;
                Album album = new Album.Builder(name).PictureLink(pictureLink).
                    Listeners(listeners).Build();
                albums.Add(album);
            }
            return albums;
        }

        private List<Track> GetTopTracksOfArtist(HtmlNode artistProfileNode)
        {
            HtmlNodeCollection nodesWithTracks = artistProfileNode.SelectNodes(XPath.ArtistProfilePage.Tracks);
            List<Track> tracks = ParseTopTracksFrom(nodesWithTracks);
            return tracks;
        }

        private List<Track> ParseTopTracksFrom(HtmlNodeCollection nodesWithTracks)
        {
            List<Track> tracks = new List<Track>();
            foreach (HtmlNode nodeWithTrack in nodesWithTracks)
            {
                int trackRating = Convert.ToInt32(nodeWithTrack.SelectSingleNode(".//td").InnerText);
                string trackName = nodeWithTrack.SelectSingleNode(".//td[4]/span/a").InnerText;
                string trackListeners = nodeWithTrack.SelectSingleNode(".//td[7]/span/span/span").InnerText;
                Track track = new Track.Builder(trackName).Rating(trackRating).
                    Listeners(trackListeners).Build();
                tracks.Add(track);
            }
            return tracks;
        }

        private List<string> GetTagsOfArtist(HtmlNode artistProfileNode)
        {
            HtmlNodeCollection nodesWithTags = artistProfileNode.SelectNodes(XPath.ArtistProfilePage.Tags);
            List<string> tags = ParseTagsFrom(nodesWithTags);
            return tags;
        }

        private List<string> ParseTagsFrom(HtmlNodeCollection nodesWithTags)
        {
            List<string> tags = new List<string>();
            foreach (HtmlNode nodeWithTag in nodesWithTags)
            {
                string tag = nodeWithTag.SelectSingleNode(".//a").InnerText;
                tags.Add(tag);
            }
            return tags;
        }

        public Artist GetArtistWithAllTracks(string artistName, string artistProfileLink, string artistPictureLink)
        {
            string linkToPageWithAllTracks = artistProfileLink + "/+tracks?date_preset=ALL";
            List<Track> allTracks = new List<Track>();
            for(int pageNumber = 1; pageNumber <= 10; pageNumber++)
            {
                string linkToCurrentPageWitAllTracks = linkToPageWithAllTracks + "&page=" + pageNumber;
                HtmlNode currentPageWithAllTracksNode = GetPageNodeFrom(linkToCurrentPageWitAllTracks);
                HtmlNodeCollection nodesWithTracks = currentPageWithAllTracksNode.SelectNodes(XPath.TracksPage.Tracks);
                foreach (HtmlNode nodeWithTrack in nodesWithTracks)
                {
                    int trackRating = Convert.ToInt32(nodeWithTrack.SelectSingleNode(".//td").InnerText);
                    string trackName = nodeWithTrack.SelectSingleNode(".//td[4]/span/a").InnerText;
                    string trackListeners = nodeWithTrack.SelectSingleNode(".//td[7]/span/span/span").InnerText;
                    Track track = new Track.Builder(trackName).Rating(trackRating).
                        Listeners(trackListeners).Build();
                    allTracks.Add(track);
                }
            }
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).Tracks(allTracks).Build();
            return artist;
        }

        public Artist GetArtistWithAllAlbums(string artistName, string artistProfileLink, string artistPictureLink)
        {
            List<Album> allAlbums = new List<Album>();
            string linkToPageWithAllAlbums = artistProfileLink + "/+albums";
            HtmlNode mainPageWithAllAlbumsNode = GetPageNodeFrom(linkToPageWithAllAlbums);
            int pagesQuantity = CountNumberOfPagesWithAlbumsFrom(mainPageWithAllAlbumsNode);
            for (int pageNumber = 1; pageNumber <= pagesQuantity; pageNumber++)
            {
                string linkToCurrentPageWithAllAlbums = linkToPageWithAllAlbums + "?page=" + pageNumber;
                HtmlNode currentPageWithAllAlbumsNode = GetPageNodeFrom(linkToCurrentPageWithAllAlbums);
                HtmlNodeCollection nodesWithAlbums = currentPageWithAllAlbumsNode.SelectNodes(XPath.AlbumsPage.Albums);
                foreach (HtmlNode nodeWithAlbum in nodesWithAlbums)
                {
                    string name = nodeWithAlbum.SelectSingleNode(".//div/h3/a").InnerText;
                    string pictureLink = nodeWithAlbum.SelectSingleNode(".//div/img").Attributes["src"].Value;
                    string listeners = nodeWithAlbum.SelectSingleNode(".//div/p").InnerText;
                    Album unfinishedAlbum = new Album.Builder(name).PictureLink(pictureLink).
                        Listeners(listeners).Build();
                    Album album = InitializeRunningLenghtAndReleaseDate(unfinishedAlbum, nodeWithAlbum);
                    allAlbums.Add(album);
                }
            }
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).Albums(allAlbums).Build();
            return artist;
        }

        private int CountNumberOfPagesWithAlbumsFrom(HtmlNode mainPageWithAllAlbumsNode)
        {
            HtmlNode lastPageLinkNode = mainPageWithAllAlbumsNode.SelectSingleNode(XPath.AlbumsPage.LastPageLink);
            int numberOfPages = Convert.ToInt32(lastPageLinkNode.InnerText);
            return numberOfPages;
        }

        private Album InitializeRunningLenghtAndReleaseDate(Album unfinishedAlbum, HtmlNode nodeWithAlbum)
        {
            Album album = unfinishedAlbum;
            string runningLenght = "", releaseDate = "";
            HtmlNode runningLenghtAndReleaseDateNode = nodeWithAlbum.SelectSingleNode(".//div/p[2]");
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

        public Artist GetArtistWithBiography(string artistName, string artistProfileLink, string artistPictureLink)
        {
            string linkToBiographyPage = artistProfileLink + "/+wiki";
            HtmlNode biographyPageNode = GetPageNodeFrom(linkToBiographyPage);
            HtmlNode nodeWithFullBiography = biographyPageNode.SelectSingleNode(XPath.BiographyPage.FullBiography);
            string fullBiography = nodeWithFullBiography.InnerText;
            Artist artist = new Artist.Builder(artistName, artistProfileLink).
                PictureLink(artistPictureLink).FullBiography(fullBiography).Build();
            return artist;
        }

        private HtmlNode GetPageNodeFrom(string pageLink)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument page = htmlWeb.Load(pageLink);
            HtmlNode node = page.DocumentNode;
            return node;
        }
    }
}
