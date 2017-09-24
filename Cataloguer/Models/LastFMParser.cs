using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Cataloguer.Models
{
    public class LastFMParser
    {
        public const string ApiKey = "f39425750fc23d743fbf853d9585a46c";

        public List<Artist> GetTopArtists()
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists&api_key=" + ApiKey;
            XmlDocument document = GetXmlDocumentFrom(url);
            List<Artist> artists = new List<Artist>();
            foreach (XmlNode node in document.SelectNodes("//artist"))
            {
                string name = node.SelectSingleNode("name").InnerText;
                string pictureLink = node.SelectSingleNode("image[@size='large']").InnerText;
                string profileLink = node.SelectSingleNode("url").InnerText;
                Artist artist = new Artist
                {
                    Name = name,
                    PictureLink = pictureLink,
                    ProfileLink = profileLink
                };
                artists.Add(artist);
            }
            return artists;
        }

        public Artist GetArtist(string artistName, string artistProfileLink, string artistPictureLink)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + artistName + "&api_key=" + ApiKey;
            XmlDocument artistInfoDocument = GetXmlDocumentFrom(url);
            XmlNode artistInfoMainNode = artistInfoDocument.SelectSingleNode("//artist");
            string listeners = artistInfoMainNode.SelectSingleNode("//stats/listeners").InnerText;
            string scrobbles = artistInfoMainNode.SelectSingleNode("//stats/playcount").InnerText;
            List<string> tags = GetTopTagsOfArtistFrom(artistInfoMainNode);
            string shortBiography = GetShortBiographyOfArtisFrom(artistInfoMainNode);
            List<Track> tracks = GetTracksOfArtist(10, artistName);
            List<Album> albums = GetAlbumsOfArtist(5, artistName);
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                Scrobbles = scrobbles,
                Listeners = listeners,
                Tags = tags,
                ShortBiography = shortBiography,
                Tracks = tracks,
                Albums = albums
            };
            return artist;
        }

        public Artist GetArtistWithAllTracks(string artistName, string artistProfileLink, string artistPictureLink)
        {
            List<Track> allTracks = GetTracksOfArtist(50, artistName);
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                Tracks = allTracks
            };
            return artist;
        }

        private List<Track> GetTracksOfArtist(int numberOfTracks, string name)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist="
                + name + "&limit=" + numberOfTracks + "&api_key=" + ApiKey;
            XmlDocument artistTopTracksDocument = GetXmlDocumentFrom(url);
            XmlNodeList nodesWithTopTracks = artistTopTracksDocument.SelectNodes("//track");
            List<Track> tracks = new List<Track>();
            foreach (XmlNode nodeWithTopTrack in nodesWithTopTracks)
            {
                int rank = Convert.ToInt32(nodeWithTopTrack.Attributes["rank"].Value);
                string trackName = nodeWithTopTrack.SelectSingleNode(".//name").InnerText;
                string listeners = nodeWithTopTrack.SelectSingleNode(".//listeners").InnerText;
                string scrobbles = nodeWithTopTrack.SelectSingleNode(".//playcount").InnerText;
                Track track = new Track
                {
                    Rank = rank,
                    Name = trackName,
                    Listeners = listeners,
                    Scrobbles = scrobbles
                };
                tracks.Add(track);
            }
            return tracks;
        }

        public Artist GetArtistWithAllAlbums(string artistName, string artistProfileLink, string artistPictureLink)
        {
            List<Album> allAlbums = GetAlbumsOfArtist(50, artistName);
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                Albums = allAlbums
            };
            return artist;
        }

        private List<Album> GetAlbumsOfArtist(int numberOfAlbums, string name)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist="
                + name + "&limit=" + numberOfAlbums + "&api_key=" + ApiKey;
            XmlDocument artistTopAlbumsDocument = GetXmlDocumentFrom(url);
            XmlNodeList nodesWithTopAlbums = artistTopAlbumsDocument.SelectNodes("//album");
            List<Album> albums = new List<Album>();
            foreach (XmlNode nodeWithTopAlbum in nodesWithTopAlbums)
            {
                string albumName = nodeWithTopAlbum.SelectSingleNode(".//name").InnerText;
                string scrobbles = nodeWithTopAlbum.SelectSingleNode(".//playcount").InnerText;
                string pageLink = nodeWithTopAlbum.SelectSingleNode(".//url").InnerText;
                string pictureLink = nodeWithTopAlbum.SelectSingleNode(".//image[@size='large']").InnerText;
                Album album = new Album
                {
                    Name = albumName,
                    Scrobbles = scrobbles,
                    PageLink = pageLink,
                    PictureLink = pictureLink
                };
                albums.Add(album);
            }
            return albums;
        }

        private List<string> GetTopTagsOfArtistFrom(XmlNode artistInfoMainNode)
        {
            XmlNodeList nodesWithTags = artistInfoMainNode.SelectNodes("//tags/tag");
            List<string> tags = new List<string>();
            foreach (XmlNode nodeWithTag in nodesWithTags)
            {
                string tag = nodeWithTag.SelectSingleNode(".//name").InnerText;
                tags.Add(tag);
            }
            return tags;
        }

        private string GetShortBiographyOfArtisFrom(XmlNode artistInfoMainNode)
        {
            string shortBiography = artistInfoMainNode.SelectSingleNode("//bio/summary").InnerText;
            int indexOfUnnecessaryLink = shortBiography.IndexOf("<a href=");
            shortBiography = shortBiography.Substring(0, indexOfUnnecessaryLink);
            return shortBiography;
        }

        public Artist GetArtistWithBiography(string artistName, string artistProfileLink, string artistPictureLink)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + artistName + "&api_key=" + ApiKey;
            XmlDocument artistInfoDocument = GetXmlDocumentFrom(url);
            XmlNode artistFullBiographyNode = artistInfoDocument.SelectSingleNode("//artist/bio/content");
            string fullBiography = artistFullBiographyNode.InnerText;
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                FullBiography = fullBiography
            };
            return artist;
        }

        private XmlDocument GetXmlDocumentFrom(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            XmlDocument document = new XmlDocument();
            document.LoadXml(result);
            return document;
        }
    }
}
