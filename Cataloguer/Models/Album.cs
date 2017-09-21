using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PageLink { get; set; }

        public string PictureLink { get; set; }

        public string ReleaseDate { get; set; }

        public string RunningLenght { get; set; }

        public string RunningTime { get; set; }

        public string Scrobbles { get; set; }

        public string Listeners { get; set; }

        public virtual List<Track> Tracks { get; set; }

        public virtual List<string> Tags { get; set; }

        //For binding in databases
        public virtual Artist Artist { get; set; }

        public class Builder
        {
            public string _name { get; set; }

            public string _pageLink { get; set; }

            public string _pictureLink { get; set; }

            public string _releaseDate { get; set; }

            public string _runningLenght { get; set; }

            public string _runningTime { get; set; }

            public string _scrobbles { get; set; }

            public string _listeners { get; set; }

            public List<Track> _tracks { get; set; }

            public List<string> _tags { get; set; }

            public Builder(string name)
            {
                _name = name;
            }

            public Builder PageLink(string pageLink)
            {
                _pageLink = pageLink;
                return this;
            }

            public Builder PictureLink(string pictureLink)
            {
                _pictureLink = pictureLink;
                return this;
            }

            public Builder ReleaseDate(string releaseDate)
            {
                _releaseDate = releaseDate;
                return this;
            }

            public Builder RunningLenght(string runningLenght)
            {
                _runningLenght = runningLenght;
                return this;
            }

            public Builder RunningTime(string runningTime)
            {
                _runningTime = runningTime;
                return this;
            }

            public Builder Scrobbles(string scrobbles)
            {
                _scrobbles = scrobbles;
                return this;
            }

            public Builder Listeners(string listeners)
            {
                _listeners = listeners;
                return this;
            }

            public Builder Tracks(List<Track> tracks)
            {
                _tracks = tracks;
                return this;
            }

            public Builder Tags(List<string> tags)
            {
                _tags = tags;
                return this;
            }

            public Album Build()
            {
                return new Album(this);
            }
        }

        public Album(Builder builder)
        {
            Name = builder._name;
            PageLink = builder._pageLink;
            PictureLink = builder._pictureLink;
            ReleaseDate = builder._releaseDate;
            RunningLenght = builder._runningLenght;
            RunningTime = builder._runningTime;
            Scrobbles = builder._scrobbles;
            Listeners = builder._listeners;
            Tracks = builder._tracks;
            Tags = builder._tags;
        }
    }
}
