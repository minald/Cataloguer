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
                Artist artist = new Artist
                {
                    Name = node.SelectSingleNode("name").InnerText,
                    PictureLink = node.SelectSingleNode("image[@size='large']").InnerText,
                    ProfileLink = node.SelectSingleNode("url").InnerText
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
            string non_normalizedShortBiography = artistInfoMainNode.SelectSingleNode("//bio/summary").InnerText;
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                Listeners = artistInfoMainNode.SelectSingleNode(".//stats/listeners").InnerText,
                Scrobbles = artistInfoMainNode.SelectSingleNode(".//stats/playcount").InnerText,
                Tags = GetTopTagsFrom(artistInfoMainNode),
                ShortBiography = NormalizeBiography(non_normalizedShortBiography),
                Tracks = GetTracksOfArtist(10, artistName),
                Albums = GetAlbumsOfArtist(5, artistName)
            };
            return artist;
        }

        public Artist GetArtistWithAllTracks(string artistName, string artistProfileLink, string artistPictureLink)
        {
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                Tracks = GetTracksOfArtist(50, artistName)
            };
            return artist;
        }

        private List<Track> GetTracksOfArtist(int numberOfTracks, string name)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist="
                + name + "&limit=" + numberOfTracks + "&api_key=" + ApiKey;
            XmlDocument artistTracksDocument = GetXmlDocumentFrom(url);
            XmlNodeList nodesWithTracks = artistTracksDocument.SelectNodes("//track");
            List<Track> tracks = new List<Track>();
            foreach (XmlNode nodeWithTrack in nodesWithTracks)
            {
                Track track = new Track
                {
                    Rank = Convert.ToInt32(nodeWithTrack.Attributes["rank"].Value),
                    Name = nodeWithTrack.SelectSingleNode(".//name").InnerText,
                    Listeners = nodeWithTrack.SelectSingleNode(".//listeners").InnerText,
                    Scrobbles = nodeWithTrack.SelectSingleNode(".//playcount").InnerText
                };
                tracks.Add(track);
            }
            return tracks;
        }

        public Artist GetArtistWithAllAlbums(string artistName, string artistProfileLink, string artistPictureLink)
        {
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                Albums = GetAlbumsOfArtist(50, artistName)
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
                Album album = new Album
                {
                    Name = nodeWithTopAlbum.SelectSingleNode(".//name").InnerText,
                    Scrobbles = nodeWithTopAlbum.SelectSingleNode(".//playcount").InnerText,
                    PageLink = nodeWithTopAlbum.SelectSingleNode(".//url").InnerText,
                    PictureLink = nodeWithTopAlbum.SelectSingleNode(".//image[@size='large']").InnerText
                };
                albums.Add(album);
            }
            return albums;
        }  

        public Artist GetArtistWithBiography(string artistName, string artistProfileLink, string artistPictureLink)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + artistName + "&api_key=" + ApiKey;
            XmlDocument artistInfoDocument = GetXmlDocumentFrom(url);
            string non_normalizedFullBiography = artistInfoDocument.SelectSingleNode("//artist/bio/content").InnerText;
            Artist artist = new Artist
            {
                Name = artistName,
                ProfileLink = artistProfileLink,
                PictureLink = artistPictureLink,
                FullBiography = NormalizeBiography(non_normalizedFullBiography)
            };
            return artist;
        }

        private string NormalizeBiography(string non_normalizedBiography)
        {
            int indexOfUnnecessaryLink = non_normalizedBiography.IndexOf("<a href=");
            string normalizedBiography = non_normalizedBiography.Substring(0, indexOfUnnecessaryLink);
            return normalizedBiography;
        }

        public Album GetAlbum(string albumName, string artistName)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=" + artistName  
                + "&album=" + albumName + "&api_key=" + ApiKey;
            XmlDocument albumInfoDocument = GetXmlDocumentFrom(url);
            XmlNode albumInfoMainNode = albumInfoDocument.SelectSingleNode("//album");
            Artist artist = new Artist
            {
                Name = albumInfoMainNode.SelectSingleNode(".//tracks/track/artist/name").InnerText,
                ProfileLink = albumInfoMainNode.SelectSingleNode(".//tracks/track/artist/url").InnerText
            };
            XmlNodeList nodesWithTracks = albumInfoMainNode.SelectNodes(".//tracks/track");
            List<Track> tracks = new List<Track>();
            foreach (XmlNode nodeWithTrack in nodesWithTracks)
            {
                Track track = new Track
                {
                    Rank = Convert.ToInt32(nodeWithTrack.Attributes["rank"].Value),
                    Name = nodeWithTrack.SelectSingleNode(".//name").InnerText,
                    Duration = nodeWithTrack.SelectSingleNode(".//duration").InnerText + " seconds",
                    PageLink = nodeWithTrack.SelectSingleNode(".//url").InnerText
                };
                tracks.Add(track);
            }
            Album album = new Album
            {
                Name = albumInfoMainNode.SelectSingleNode(".//name").InnerText,
                PageLink = albumInfoMainNode.SelectSingleNode(".//url").InnerText,
                PictureLink = albumInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText,
                Listeners = albumInfoMainNode.SelectSingleNode(".//listeners").InnerText,
                Scrobbles = albumInfoMainNode.SelectSingleNode(".//playcount").InnerText,
                Tags = GetTopTagsFrom(albumInfoMainNode),
                Artist = artist,
                Tracks = tracks
            };
            return album;
        }

        private List<string> GetTopTagsFrom(XmlNode mainNode)
        {
            XmlNodeList nodesWithTags = mainNode.SelectNodes("//tags/tag");
            List<string> tags = new List<string>();
            foreach (XmlNode nodeWithTag in nodesWithTags)
            {
                string tag = nodeWithTag.SelectSingleNode(".//name").InnerText;
                tags.Add(tag);
            }
            return tags;
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
