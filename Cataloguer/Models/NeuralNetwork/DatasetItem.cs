using System;
using System.Collections.Generic;
using System.Linq;

namespace Cataloguer.Models.NeuralNetwork
{
    public class DatasetItem
    {
        public float[] InputData { get; set; } = new float[7];

        public float[] OutputData { get; set; }

        public DatasetItem(ApplicationUser user, List<Rating> ratings, List<Track> tracks, int outputLayerLength)
        {
            InitializeInputData(user);
            OutputData = new float[outputLayerLength];
            InitializeOutputData(ratings, tracks);
        }

        public void InitializeInputData(ApplicationUser user)
        {
            InputData[0] = (float)(user.Country.Value1 + 180) / 360;
            InputData[1] = (float)(user.Country.Value2 + 180) / 360;
            InputData[2] = (float)(user.SecondLanguage.Value1 + 180) / 360;
            InputData[3] = (float)(user.SecondLanguage.Value2 + 180) / 360;
            InputData[4] = (DateTime.Now.Year - user.BirthYear) / 100;
            InputData[5] = (float)user.Gender;
            InputData[6] = (user.Temperament.Value - 1) / 3;
        }

        public void InitializeOutputData(List<Rating> ratings, List<Track> tracks)
        {
            int maxRank = ratings.Select(r => r.Rank).Max();
            List<Track> userTracks = ratings.Select(r => r.Track).ToList();
            int i = 0;
            foreach (Track track in tracks.OrderBy(t => t.Id))
            {
                if (userTracks.Contains(track))
                {
                    Rating rating = ratings.First(r => r.Track.Id == track.Id);
                    OutputData[i] = (float)(2 * maxRank - rating.Rank) / (2 * maxRank);
                }
                else
                {
                    OutputData[i] = 0;
                }
                
                i++;
            }
        }
    }
}
