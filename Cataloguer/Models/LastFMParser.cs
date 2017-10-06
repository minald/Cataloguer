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
        public const string СommonUrl = "http://ws.audioscrobbler.com/2.0/?api_key=f39425750fc23d743fbf853d9585a46c&";

        public string CounrtyForSearch = "belarus";

        public List<Artist> GetTopArtists(int page, int limit)
        {
            string url = СommonUrl + "method=geo.gettopartists" + 
                "&country=" + CounrtyForSearch + "&page=" + page + "&limit=" + limit;
            List<Artist> artists = new List<Artist>();
            foreach (XmlNode artistNode in GetXmlDocumentFrom(url).SelectNodes("//artist"))
            {
                Artist artist = new Artist(artistNode.SelectSingleNode("name").InnerText);
                artist.SetPictureLink(artistNode.SelectSingleNode("image[@size='large']").InnerText);
                artists.Add(artist);
            }
            return artists;
        }

        public Artist GetArtist(string name)
        {
            string url = СommonUrl + "method=artist.getinfo&artist=" + name;
            XmlNode artistInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//artist");
            Artist artist = new Artist(name)
            {
                Albums = GetAlbumsOfArtist(name, 1, 8),
                Tracks = GetTracksOfArtist(name, 1, 12),
                Tags = GetTopTagsFrom(artistInfoMainNode.SelectNodes("//tags/tag"))
            };
            artist.SetPictureLink(artistInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            artist.SetScrobbles(artistInfoMainNode.SelectSingleNode(".//stats/playcount").InnerText);
            artist.SetListeners(artistInfoMainNode.SelectSingleNode(".//stats/listeners").InnerText);
            artist.SetShortBiography(artistInfoMainNode.SelectSingleNode(".//bio/summary").InnerText);
            return artist;
        }

        public Artist GetArtistWithAllTracks(string name)
        {
            Artist artist = new Artist(name)
            {
                Tracks = GetTracksOfArtist(name, 1, 48)
            };
            artist.SetPictureLink(GetPictureLinkOfArtist(name));
            return artist;
        }

        public List<Track> GetTracksOfArtist(string name, int page, int limit)
        {
            string url = СommonUrl + "method=artist.gettoptracks&artist=" + name +
                "&page=" + page + "&limit=" + limit;
            XmlDocument artistTracksDocument = GetXmlDocumentFrom(url);
            Artist artist = new Artist(artistTracksDocument.SelectSingleNode("//toptracks").
                    Attributes["artist"].Value);
            List<Track> tracks = new List<Track>();
            foreach (XmlNode nodeWithTrack in artistTracksDocument.SelectNodes("//track"))
            {
                Track track = new Track(nodeWithTrack.SelectSingleNode(".//name").InnerText)
                {
                    Rank = Convert.ToInt32(nodeWithTrack.Attributes["rank"].Value)
                };
                track.SetPictureLink(nodeWithTrack.SelectSingleNode(".//image[@size='large']").InnerText);
                track.SetScrobbles(nodeWithTrack.SelectSingleNode(".//playcount").InnerText);
                track.SetListeners(nodeWithTrack.SelectSingleNode(".//listeners").InnerText);
                track.Artist = artist;
                tracks.Add(track);
            }
            return tracks;
        }

        public Artist GetArtistWithAllAlbums(string name)
        {
            Artist artist = new Artist(name)
            {
                Albums = GetAlbumsOfArtist(name, 1, 48)
            };
            artist.SetPictureLink(GetPictureLinkOfArtist(name));
            return artist;
        }

        public List<Album> GetAlbumsOfArtist(string name, int page, int limit)
        {
            string url = СommonUrl + "method=artist.gettopalbums&artist=" + name +
                "&page=" + page + "&limit=" + limit;
            XmlDocument artistAlbumsDocument = GetXmlDocumentFrom(url);
            Artist artist = new Artist(artistAlbumsDocument.SelectSingleNode("//topalbums").
                    Attributes["artist"].Value);
            List<Album> albums = new List<Album>();
            foreach (XmlNode nodeWithTopAlbum in artistAlbumsDocument.SelectNodes("//album"))
            {
                Album album = new Album(nodeWithTopAlbum.SelectSingleNode(".//name").InnerText);
                album.SetPictureLink(nodeWithTopAlbum.SelectSingleNode(".//image[@size='large']").InnerText);
                album.SetScrobbles(nodeWithTopAlbum.SelectSingleNode(".//playcount").InnerText);
                album.Artist = artist;
                albums.Add(album);
            }
            return albums;
        }

        public string GetPictureLinkOfArtist(string name)
        {
            string url = СommonUrl + "method=artist.getinfo&artist=" + name;
            return GetXmlDocumentFrom(url).SelectSingleNode("//artist/image[@size='large']").InnerText;
        }

        public Artist GetArtistWithBiography(string name)
        {
            string url = СommonUrl + "method=artist.getinfo&artist=" + name;
            XmlNode artistInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//artist");
            Artist artist = new Artist(name);
            artist.SetPictureLink(artistInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            artist.SetFullBiography(artistInfoMainNode.SelectSingleNode(".//bio/content").InnerText);
            return artist;
        }

        public Album GetAlbum(string albumName, string artistName)
        {
            string url = СommonUrl + "method=album.getinfo&artist=" + artistName + "&album=" + albumName;
            XmlNode albumInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//album");
            Album album = new Album(albumInfoMainNode.SelectSingleNode(".//name").InnerText)
            {
                Tags = GetTopTagsFrom(albumInfoMainNode.SelectNodes(".//tags/tag")),
                Artist = new Artist(albumInfoMainNode.SelectSingleNode(".//artist").InnerText),
                Tracks = GetTracksOfAlbumFrom(albumInfoMainNode.SelectNodes(".//tracks/track"))
            };
            album.SetPictureLink(albumInfoMainNode.SelectSingleNode(".//image[@size='large']").InnerText);
            album.SetScrobbles(albumInfoMainNode.SelectSingleNode(".//playcount").InnerText);
            album.SetListeners(albumInfoMainNode.SelectSingleNode(".//listeners").InnerText);
            return album;
        }

        private List<Track> GetTracksOfAlbumFrom(XmlNodeList nodesWithTracks)
        {
            List<Track> tracks = new List<Track>();
            foreach (XmlNode nodeWithTrack in nodesWithTracks)
            {
                Track track = new Track(nodeWithTrack.SelectSingleNode(".//name").InnerText)
                {
                    Rank = Convert.ToInt32(nodeWithTrack.Attributes["rank"].Value)
                };
                track.SetDuration(nodeWithTrack.SelectSingleNode(".//duration").InnerText);
                tracks.Add(track);
            }
            return tracks;
        }

        public Track GetTrack(string trackName, string artistName)
        {
            string url = СommonUrl + "method=track.getInfo&artist=" + artistName + "&track=" + trackName;
            XmlNode trackInfoMainNode = GetXmlDocumentFrom(url).SelectSingleNode("//track");
            Artist artist = new Artist(trackInfoMainNode.SelectSingleNode(".//artist/name").InnerText);
            Album album = GetAlbumOfTrackFrom(trackInfoMainNode, artist);
            Track track = new Track(trackInfoMainNode.SelectSingleNode(".//name").InnerText)
            {
                Artist = artist,
                Album = album,
                Tags = GetTopTagsFrom(trackInfoMainNode.SelectNodes(".//toptags/tag"))
            };
            track.SetPictureLink(trackInfoMainNode.SelectSingleNode(".//album/image[@size='large']")?.InnerText ?? "");
            track.SetDurationInMilliseconds(trackInfoMainNode.SelectSingleNode(".//duration").InnerText);
            track.SetListeners(trackInfoMainNode.SelectSingleNode(".//listeners").InnerText);
            track.SetScrobbles(trackInfoMainNode.SelectSingleNode(".//playcount").InnerText);
            track.SetInfo(trackInfoMainNode.SelectSingleNode(".//wiki/summary")?.InnerText ?? "");
            return track;
        }

        private Album GetAlbumOfTrackFrom(XmlNode trackInfoMainNode, Artist artist)
        {
            string albumName = trackInfoMainNode.SelectSingleNode(".//album/title")?.InnerText ?? "";
            Album album = null;
            if (albumName != "")
            {
                album = new Album(albumName);
                album.SetPictureLink(trackInfoMainNode.SelectSingleNode(".//album/image[@size='large']").InnerText);
                album.Artist = artist;
            }
            return album;
        }

        private List<string> GetTopTagsFrom(XmlNodeList nodesWithTags)
        {
            List<string> tags = new List<string>();
            foreach (XmlNode nodeWithTag in nodesWithTags)
            {
                string tag = nodeWithTag.SelectSingleNode(".//name").InnerText;
                tags.Add(tag);
            }
            return tags;
        }

        public List<Artist> SearchArtists(string value, int page, int limit)
        {
            string url = СommonUrl + "method=artist.search&artist=" + value + "&page=" + page + "&limit=" + limit;
            List<Artist> artists = new List<Artist>();
            foreach (XmlNode artistNode in GetXmlDocumentFrom(url).SelectNodes("//artistmatches/artist"))
            {
                Artist artist = new Artist(artistNode.SelectSingleNode(".//name").InnerText);
                artist.SetPictureLink(artistNode.SelectSingleNode(".//image[@size='large']").InnerText);
                artists.Add(artist);
            }
            return artists;
        }

        public List<Album> SearchAlbums(string value, int page, int limit)
        {
            string url = СommonUrl + "method=album.search&album=" + value + "&page=" + page + "&limit=" + limit;
            List<Album> albums = new List<Album>();
            foreach (XmlNode albumNode in GetXmlDocumentFrom(url).SelectNodes("//albummatches/album"))
            {
                Album album = new Album(albumNode.SelectSingleNode(".//name").InnerText)
                {
                    Artist = new Artist(albumNode.SelectSingleNode(".//artist").InnerText)
                };
                album.SetPictureLink(albumNode.SelectSingleNode(".//image[@size='large']").InnerText);
                albums.Add(album);
            }
            return albums;
        }

        public List<Track> SearchTracks(string value, int page, int limit)
        {
            string url = СommonUrl + "method=track.search&track=" + value + "&page=" + page + "&limit=" + limit;
            List<Track> tracks = new List<Track>();
            foreach(XmlNode trackNode in GetXmlDocumentFrom(url).SelectNodes("//trackmatches/track"))
            {
                Track track = new Track(trackNode.SelectSingleNode(".//name").InnerText)
                {
                    Artist = new Artist(trackNode.SelectSingleNode(".//artist").InnerText)
                };
                track.SetPictureLink(trackNode.SelectSingleNode(".//image[@size='large']").InnerText);
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

        public string GetPictureLinkOfAlbum(string albumName, string artistName)
        {
            string url = СommonUrl + "method=album.getinfo&artist=" + artistName + "&album=" + albumName;
            return GetXmlDocumentFrom(url).SelectSingleNode("//album/image[@size='large']").InnerText;
        }
    }
}
