using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class ChartViewModel
    {
        public List<Track> Chart { get; set; }

        public List<Track> UserRating { get; set; }

        public List<KeyValuePair<Track, float>> AssumptiveRating { get; set; }

        public ChartViewModel(List<Track> chart, List<Track> userRating, List<KeyValuePair<Track, float>> assumptiveRating)
        {
            Chart = chart;
            UserRating = userRating;
            AssumptiveRating = assumptiveRating;
        }
    }
}
