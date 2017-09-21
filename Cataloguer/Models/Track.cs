namespace Cataloguer.Models
{
    public class Track
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PageLink { get; set; }

        public int Rating { get; set; }

        public string Listeners { get; set; }

        public string Scrobbles { get; set; }

        //For binding in databases
        public virtual Album Album { get; set; }

        //For binding in databases
        public virtual Artist Artist { get; set; }

        public class Builder
        {
            public string _name { get; set; }

            public string _pageLink { get; set; }

            public int _rating { get; set; }

            public string _scrobbles { get; set; }

            public string _listeners { get; set; }

            public Builder(string name)
            {
                _name = name;
            }

            public Builder PageLink(string pageLink)
            {
                _pageLink = pageLink;
                return this;
            }

            public Builder Rating(int rating)
            {
                _rating = rating;
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

            public Track Build()
            {
                return new Track(this);
            }
        }

        public Track(Builder builder)
        {
            Name = builder._name;
            PageLink = builder._pageLink;
            Rating = builder._rating;
            Scrobbles = builder._scrobbles;
            Listeners = builder._listeners;
        }
    }
}