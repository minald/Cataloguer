using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Cataloguer.Models
{
    //Documentation https://www.last.fm/ru/api
    public static class LastFMParser
    {
        // Alternative key - 96d047d302a8707f3a7410873466dbfd
        private const string СommonUrl = "http://ws.audioscrobbler.com/2.0/?api_key=f39425750fc23d743fbf853d9585a46c&";

        private readonly static string CounrtyForSearch = "belarus";

        public static IEnumerable<Artist> GetTopArtists(int limit, int page = 1)
        {
            string url = $"{СommonUrl}method=geo.gettopartists&country={CounrtyForSearch}&page={page}&limit={limit}";
            foreach (XmlNode artistNode in GetXmlDocumentFrom(url).SelectNodes("//artist"))
            {
                var artist = new Artist(artistNode.SelectSingleNode("name").InnerText);
                artist.SetPictureLink(artistNode.SelectSingleNode("image[@size='large']").InnerText);
                yield return artist;
            }
        }

        public static Artist GetArtist(string name)
        {
            string url = $"{СommonUrl}method=artist.getinfo&artist={name}";
            XmlNode artistInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//artist");
            List<Album> albums = GetAlbumsOfArtist(name, 8).ToList();
            List<Track> tracks = GetTracksOfArtist(name, 12).ToList();
            List<Tag> tags = GetTopTagsFrom(artistInfoMainNode.SelectNodes("//tags/tag")).ToList();
            var artist = new Artist(name) { Albums = albums, Tracks = tracks, Tags = tags };
            artist.SetPictureLink(artistInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            artist.SetScrobbles(artistInfoMainNode.SelectSingleNode(".//stats/playcount").InnerText);
            artist.SetListeners(artistInfoMainNode.SelectSingleNode(".//stats/listeners").InnerText);
            artist.SetShortBiography(artistInfoMainNode.SelectSingleNode(".//bio/summary").InnerText);
            return artist;
        }

        public static Artist GetArtistWithAllTracks(string name)
        {
            List<Track> tracks = GetTracksOfArtist(name, 48).ToList();
            var artist = new Artist(name) { Tracks = tracks };
            artist.SetPictureLink(GetPictureLinkOfArtist(name));
            return artist;
        }

        public static IEnumerable<Track> GetTracksOfArtist(string name, int limit, int page = 1)
        {
            string url = $"{СommonUrl}method=artist.gettoptracks&artist={name}&page={page}&limit={limit}";
            XmlDocument artistTracksDocument = GetXmlDocumentFrom(url);
            var artist = new Artist(artistTracksDocument.SelectSingleNode("//toptracks").Attributes["artist"].Value);
            foreach (XmlNode nodeWithTrack in artistTracksDocument.SelectNodes("//track"))
            {
                string trackName = nodeWithTrack.SelectSingleNode(".//name").InnerText;
                int rank = Convert.ToInt32(nodeWithTrack.Attributes["rank"].Value);
                var track = new Track(trackName) { Rank = rank , Artist = artist };
                track.SetPictureLink(nodeWithTrack.SelectSingleNode(".//image[@size='large']").InnerText);
                track.SetScrobbles(nodeWithTrack.SelectSingleNode(".//playcount").InnerText);
                track.SetListeners(nodeWithTrack.SelectSingleNode(".//listeners").InnerText);
                yield return track;
            }
        }

        public static Artist GetArtistWithAllAlbums(string name)
        {
            List<Album> albums = GetAlbumsOfArtist(name, 48).ToList();
            Artist artist = new Artist(name) { Albums = albums };
            artist.SetPictureLink(GetPictureLinkOfArtist(name));
            return artist;
        }

        public static IEnumerable<Album> GetAlbumsOfArtist(string name, int limit, int page = 1)
        {
            string url = $"{СommonUrl}method=artist.gettopalbums&artist={name}&page={page}&limit={limit}";
            XmlDocument artistAlbumsDocument = GetXmlDocumentFrom(url);
            var artist = new Artist(artistAlbumsDocument.SelectSingleNode("//topalbums").Attributes["artist"].Value);
            foreach (XmlNode nodeWithTopAlbum in artistAlbumsDocument.SelectNodes("//album"))
            {
                string albumName = nodeWithTopAlbum.SelectSingleNode(".//name").InnerText;
                var album = new Album(albumName) { Artist = artist};
                album.SetPictureLink(nodeWithTopAlbum.SelectSingleNode(".//image[@size='large']").InnerText);
                album.SetScrobbles(nodeWithTopAlbum.SelectSingleNode(".//playcount").InnerText);
                yield return album;
            }
        }

        public static string GetPictureLinkOfArtist(string name)
        {
            string url = $"{СommonUrl}method=artist.getinfo&artist={name}";
            return GetXmlDocumentFrom(url).SelectSingleNode("//artist/image[@size='large']").InnerText;
        }

        public static Artist GetArtistWithBiography(string name)
        {
            string url = $"{СommonUrl}method=artist.getinfo&artist={name}";
            XmlNode artistInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//artist");
            var artist = new Artist(name);
            artist.SetPictureLink(artistInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            artist.SetFullBiography(artistInfoMainNode.SelectSingleNode(".//bio/content").InnerText);
            return artist;
        }

        public static Album GetAlbum(string albumName, string artistName)
        {
            string url = $"{СommonUrl}method=album.getinfo&artist={artistName}&album={albumName}";
            XmlNode albumInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//album");
            List<Tag> tags = GetTopTagsFrom(albumInfoMainNode.SelectNodes(".//tags/tag")).ToList();
            var artist = new Artist(albumInfoMainNode.SelectSingleNode(".//artist").InnerText);
            List<Track> tracks = GetTracksOfAlbumFrom(albumInfoMainNode.SelectNodes(".//tracks/track")).ToList();
            string name = albumInfoMainNode.SelectSingleNode(".//name").InnerText;
            var album = new Album(name) { Tags = tags, Artist = artist, Tracks = tracks };
            album.SetPictureLink(albumInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            album.SetScrobbles(albumInfoMainNode.SelectSingleNode(".//playcount").InnerText);
            album.SetListeners(albumInfoMainNode.SelectSingleNode(".//listeners").InnerText);
            return album;
        }

        private static IEnumerable<Track> GetTracksOfAlbumFrom(XmlNodeList nodesWithTracks)
        {
            foreach (XmlNode nodeWithTrack in nodesWithTracks)
            {
                string name = nodeWithTrack.SelectSingleNode(".//name").InnerText;
                int rank = Convert.ToInt32(nodeWithTrack.Attributes["rank"].Value);
                var track = new Track(name) { Rank = rank };
                track.SetDuration(nodeWithTrack.SelectSingleNode(".//duration").InnerText);
                yield return track;
            }
        }

        public static Track GetTrack(string searchedTrackName, string artistName)
        {
            string url = $"{СommonUrl}method=track.getInfo&artist={artistName}&track={searchedTrackName}";
            XmlNode trackInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//track");
            var artist = new Artist(trackInfoMainNode.SelectSingleNode(".//artist/name").InnerText);
            Album album = GetAlbumOfTrackFrom(trackInfoMainNode, artist);
            List<Tag> tags = GetTopTagsFrom(trackInfoMainNode.SelectNodes(".//toptags/tag")).ToList();
            string name = trackInfoMainNode.SelectSingleNode(".//name").InnerText;
            var track = new Track(name) { Artist = artist, Album = album, Tags = tags };
            track.SetPictureLink(trackInfoMainNode.SelectSingleNode(".//album/image[@size='large']")?.InnerText ?? "");
            track.SetDurationInMilliseconds(trackInfoMainNode.SelectSingleNode(".//duration").InnerText);
            track.SetListeners(trackInfoMainNode.SelectSingleNode(".//listeners").InnerText);
            track.SetScrobbles(trackInfoMainNode.SelectSingleNode(".//playcount").InnerText);
            track.SetInfo(trackInfoMainNode.SelectSingleNode(".//wiki/summary")?.InnerText ?? "");
            return track;
        }

        private static Album GetAlbumOfTrackFrom(XmlNode trackInfoMainNode, Artist artist)
        {
            string albumName = trackInfoMainNode.SelectSingleNode(".//album/title")?.InnerText ?? "";
            Album album = null;
            if (!String.IsNullOrWhiteSpace(albumName))
            {
                album = new Album(albumName) { Artist = artist};
                album.SetPictureLink(trackInfoMainNode.SelectSingleNode(".//album/image[@size='large']").InnerText);
            }

            return album;
        }

        private static IEnumerable<Tag> GetTopTagsFrom(XmlNodeList nodesWithTags)
        {
            foreach (XmlNode nodeWithTag in nodesWithTags)
            {
                string tag = nodeWithTag.SelectSingleNode(".//name").InnerText;
                yield return new Tag(tag);
            }
        }

        public static IEnumerable<Artist> SearchArtists(string value, int limit, int page = 1)
        {
            string url = $"{СommonUrl}method=artist.search&artist={value}&page={page}&limit={limit}";
            foreach (XmlNode artistNode in GetXmlDocumentFrom(url).SelectNodes("//artistmatches/artist"))
            {
                var artist = new Artist(artistNode.SelectSingleNode(".//name").InnerText);
                artist.SetPictureLink(artistNode.SelectSingleNode(".//image[@size='large']").InnerText);
                yield return artist;
            }
        }

        public static IEnumerable<Album> SearchAlbums(string value, int limit, int page = 1)
        {
            string url = $"{СommonUrl}method=album.search&album={value}&page={page}&limit={limit}";
            foreach (XmlNode albumNode in GetXmlDocumentFrom(url).SelectNodes("//albummatches/album"))
            {
                var artist = new Artist(albumNode.SelectSingleNode(".//artist").InnerText);
                string name = albumNode.SelectSingleNode(".//name").InnerText;
                var album = new Album(name) { Artist = artist };
                album.SetPictureLink(albumNode.SelectSingleNode(".//image[@size='large']").InnerText);
                yield return album;
            }
        }

        public static IEnumerable<Track> SearchTracks(string value, int limit, int page = 1)
        {
            string url = $"{СommonUrl}method=track.search&track={value}&page={page}&limit={limit}";
            foreach(XmlNode trackNode in GetXmlDocumentFrom(url).SelectNodes("//trackmatches/track"))
            {
                var artist = new Artist(trackNode.SelectSingleNode(".//artist").InnerText);
                string name = trackNode.SelectSingleNode(".//name").InnerText;
                var track = new Track(name) { Artist = artist };
                track.SetPictureLink(trackNode.SelectSingleNode(".//image[@size='large']").InnerText);
                track.SetListeners(trackNode.SelectSingleNode(".//listeners").InnerText);
                yield return track;
            }
        }

        private static XmlDocument GetXmlDocumentFrom(string url)
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

        public static string GetPictureLinkOfAlbum(string albumName, string artistName)
        {
            string url = $"{СommonUrl}method=album.getinfo&artist={artistName}&album={albumName}";
            return GetXmlDocumentFrom(url).SelectSingleNode("//album/image[@size='large']").InnerText;
        }
    }
}
