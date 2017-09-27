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

        public List<Artist> GetTopArtists(string page, int limit)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists" +
                 "&page=" + page + "&limit=" + limit + "&api_key=" + ApiKey;
            XmlDocument document = GetXmlDocumentFrom(url);
            XmlNodeList nodesWithArtists = document.SelectNodes("//artist");
            List<Artist> artists = new List<Artist>();
            foreach (XmlNode artistNode in nodesWithArtists)
            {
                Artist artist = new Artist(artistNode.SelectSingleNode("name").InnerText);
                artist.SetPictureLink(artistNode.SelectSingleNode("image[@size='large']").InnerText);
                artists.Add(artist);
            }
            return artists;
        }

        public Artist GetArtist(string name)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + name + "&api_key=" + ApiKey;
            XmlDocument artistInfoDocument = GetXmlDocumentFrom(url);
            XmlNode artistInfoMainNode = artistInfoDocument.SelectSingleNode("//artist");
            string non_normalizedShortBiography = artistInfoMainNode.SelectSingleNode("//bio/summary").InnerText;
            Artist artist = new Artist
            {
                Name = name,
                Tags = GetTopTagsFrom(artistInfoMainNode),
                Tracks = GetTracksOfArtist(10, name),
                Albums = GetAlbumsOfArtist(10, name)
            };
            artist.SetPictureLink(artistInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            artist.SetScrobbles(artistInfoMainNode.SelectSingleNode(".//stats/playcount").InnerText);
            artist.SetListeners(artistInfoMainNode.SelectSingleNode(".//stats/listeners").InnerText);
            artist.SetShortBiography(non_normalizedShortBiography);
            return artist;
        }

        public Artist GetArtistWithAllTracks(string artistName, string artistPictureLink)
        {
            Artist artist = new Artist
            {
                Name = artistName,
                Tracks = GetTracksOfArtist(50, artistName)
            };
            artist.SetPictureLink(artistPictureLink);
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
                    Name = nodeWithTrack.SelectSingleNode(".//name").InnerText
                };
                track.SetScrobbles(nodeWithTrack.SelectSingleNode(".//playcount").InnerText);
                track.SetListeners(nodeWithTrack.SelectSingleNode(".//listeners").InnerText);
                tracks.Add(track);
            }
            return tracks;
        }

        public Artist GetArtistWithAllAlbums(string artistName, string artistPictureLink)
        {
            Artist artist = new Artist
            {
                Name = artistName,
                Albums = GetAlbumsOfArtist(50, artistName)
            };
            artist.SetPictureLink(artistPictureLink);
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
                Album album = new Album(nodeWithTopAlbum.SelectSingleNode(".//name").InnerText);
                album.SetPictureLink(nodeWithTopAlbum.SelectSingleNode(".//image[@size='large']").InnerText);
                album.SetScrobbles(nodeWithTopAlbum.SelectSingleNode(".//playcount").InnerText);
                albums.Add(album);
            }
            return albums;
        }  

        public Artist GetArtistWithBiography(string name)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + name + "&api_key=" + ApiKey;
            XmlDocument artistInfoDocument = GetXmlDocumentFrom(url);
            string non_normalizedFullBiography = artistInfoDocument.SelectSingleNode("//artist/bio/content").InnerText;
            Artist artist = new Artist(name);
            artist.SetPictureLink(artistInfoDocument.SelectSingleNode("//artist/image[@size='large']").InnerText);
            artist.SetFullBiography(non_normalizedFullBiography);
            return artist;
        }

        public Album GetAlbum(string albumName, string artistName)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=" + artistName  
                + "&album=" + albumName + "&api_key=" + ApiKey;
            XmlDocument albumInfoDocument = GetXmlDocumentFrom(url);
            XmlNode albumInfoMainNode = albumInfoDocument.SelectSingleNode("//album");
            Artist artist = new Artist(albumInfoMainNode.SelectSingleNode(".//artist").InnerText);
            XmlNodeList nodesWithTracks = albumInfoMainNode.SelectNodes(".//tracks/track");
            List<Track> tracks = new List<Track>();
            foreach (XmlNode nodeWithTrack in nodesWithTracks)
            {
                Track track = new Track
                {
                    Rank = Convert.ToInt32(nodeWithTrack.Attributes["rank"].Value),
                    Name = nodeWithTrack.SelectSingleNode(".//name").InnerText
                };
                track.SetDuration(nodeWithTrack.SelectSingleNode(".//duration").InnerText);
                tracks.Add(track);
            }
            Album album = new Album
            {
                Name = albumInfoMainNode.SelectSingleNode(".//name").InnerText,
                Tags = GetTopTagsFrom(albumInfoMainNode),
                Artist = artist,
                Tracks = tracks
            };
            album.SetPictureLink(albumInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            album.SetScrobbles(albumInfoMainNode.SelectSingleNode(".//playcount").InnerText);
            album.SetListeners(albumInfoMainNode.SelectSingleNode(".//listeners").InnerText);
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

        public List<Artist> SearchArtists(string value, int limit)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=artist.search&artist=" + value +
                "&limit=" + limit + "&api_key=" + ApiKey;
            XmlDocument document = GetXmlDocumentFrom(url);
            XmlNodeList nodesWithArtists = document.SelectNodes("//artistmatches/artist");
            List<Artist> artists = new List<Artist>();
            foreach (XmlNode artistNode in nodesWithArtists)
            {
                Artist artist = new Artist(artistNode.SelectSingleNode(".//name").InnerText);
                artist.SetPictureLink(artistNode.SelectSingleNode(".//image[@size='large']").InnerText);
                artists.Add(artist);
            }
            return artists;
        }

        public List<Album> SearchAlbums(string value, int limit)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=album.search&album=" + value +
                "&limit=" + limit + "&api_key=" + ApiKey;
            XmlDocument document = GetXmlDocumentFrom(url);
            XmlNodeList nodesWithAlbums = document.SelectNodes("//albummatches/album");
            List<Album> albums = new List<Album>();
            foreach (XmlNode albumNode in nodesWithAlbums)
            {
                Album album = new Album
                {
                    Name = albumNode.SelectSingleNode(".//name").InnerText,
                    Artist = new Artist(albumNode.SelectSingleNode(".//artist").InnerText)
                };
                album.SetPictureLink(albumNode.SelectSingleNode(".//image[@size='large']").InnerText);
                albums.Add(album);
            }
            return albums;
        }

        public List<Track> SearchTracks(string value, int limit)
        {
            string url = "http://ws.audioscrobbler.com/2.0/?method=track.search&track=" + value +
                "&limit=" + limit + "&api_key=" + ApiKey;
            XmlDocument document = GetXmlDocumentFrom(url);
            XmlNodeList nodesWithTracks = document.SelectNodes("//trackmatches/track");
            List<Track> tracks = new List<Track>();
            foreach(XmlNode trackNode in nodesWithTracks)
            {
                Track track = new Track
                {
                    Name = trackNode.SelectSingleNode(".//name").InnerText,
                    Artist = new Artist(trackNode.SelectSingleNode(".//artist").InnerText)
                };
                track.SetListeners(trackNode.SelectSingleNode(".//listeners").InnerText);
                tracks.Add(track);
            }
            return tracks;
        }

        private XmlDocument GetXmlDocumentFrom(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            XmlDocument document = new XmlDocument();
            using (var stringReader = new StringReader(result))
            {
                using (var xmlTextReader = new XmlTextReader(stringReader) { Namespaces = false })
                {
                    document.Load(xmlTextReader);
                }
            }
            return document;
        }
    }
}
