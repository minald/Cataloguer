using Microsoft.AspNetCore.Identity;
using System;

namespace Cataloguer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int BirthYear { get; set; }

        public double Gender { get; set; } // 0 is female, 1 is male

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int SecondLanguageId { get; set; }
        public Language SecondLanguage { get; set; }

        public int TemperamentId { get; set; }
        public Temperament Temperament { get; set; }

        public float[] GetInputData()
        {
            float[] inputData = new float[7];
            inputData[0] = (float)(Country.Value1 + 180) / 360;
            inputData[1] = (float)(Country.Value2 + 180) / 360;
            inputData[2] = (float)(SecondLanguage.Value1 + 180) / 360;
            inputData[3] = (float)(SecondLanguage.Value2 + 180) / 360;
            inputData[4] = (DateTime.Now.Year - BirthYear) / 100;
            inputData[5] = (float)Gender;
            inputData[6] = (Temperament.Value - 1) / 3;
            return inputData;
        }
    }
}
