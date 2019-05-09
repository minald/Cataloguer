using Cataloguer.Models;
using Cataloguer.Models.NeuralNetwork;
using System.Linq;

namespace Cataloguer.Data
{
    public static class DbInitializer
    { 
        // Coordinates from https://www.mir-geo.ru/vse-stran/geogr/geog-koor
        public static void Seed(ApplicationDbContext db)
        {
            if (!db.Countries.Any())
            {
                db.Add(new Country("Australia", -127, 133));
                db.Add(new Country("Argentina", -34, -64));
                db.Add(new Country("Bangladesh", 24, 90));
                db.Add(new Country("Belarus", 53, 28));
                db.Add(new Country("Belgium", 50, 4));
                db.Add(new Country("Botswana", -22, 24));
                db.Add(new Country("Brazil", -10, -55));
                db.Add(new Country("Great Britain", 54, 2));
                db.Add(new Country("Venezuela", 8, -66));
                db.Add(new Country("Vietnam", 16, 106));
                db.Add(new Country("Ghana", 8, 2));
                db.Add(new Country("Germany", 51, 9));
                db.Add(new Country("Egypt", 27, 30));
                db.Add(new Country("Israel", 31, 34));
                db.Add(new Country("India", 20, 77));
                db.Add(new Country("Indonesia", -5, 120));
                db.Add(new Country("Iraq", 33, 44));
                db.Add(new Country("Iceland", 65, 18));
                db.Add(new Country("Spain", 40, 4));
                db.Add(new Country("Italy", 42, 12));
                db.Add(new Country("Canada", 60, -95));
                db.Add(new Country("China", 35, 105));
                db.Add(new Country("Mexico", 23, -102));
                db.Add(new Country("Nigeria", 10, 8));
                db.Add(new Country("Netherlands", 52, 5));
                db.Add(new Country("Pakistan", 30, 70));
                db.Add(new Country("Peru", -10, -76));
                db.Add(new Country("Poland", 52, 20));
                db.Add(new Country("Russia", 60, 100));
                db.Add(new Country("Slovakia", 48, 19));
                db.Add(new Country("USA", 38, -97));
                db.Add(new Country("Sudan", 15, 30));
                db.Add(new Country("Turkey", 39, 35));
                db.Add(new Country("Ukraine", 49, 32));
                db.Add(new Country("Uruguay", -33, -56));
                db.Add(new Country("Finland", 64, 26));
                db.Add(new Country("France", 46, 2));
                db.Add(new Country("Chile", -30, 71));
                db.Add(new Country("Switzerland", 47, 8));
                db.Add(new Country("Sweden", 62, 15));
                db.Add(new Country("South Korea", 37, 127));
                db.Add(new Country("Japan", 36, 138));
            }

            if (!db.Languages.Any())
            {
                db.Add(new Language("Chinese", 35, 105));
                db.Add(new Language("Spanish", 40, 4));
                db.Add(new Language("English", 54, 2));
                db.Add(new Language("Arabic", 27, 30));
                db.Add(new Language("Hindi", 20, 77));
                db.Add(new Language("Bengali", 24, 90));
                db.Add(new Language("Portuguese", 39, 8));
                db.Add(new Language("Russian", 60, 100));
                db.Add(new Language("Japanese", 36, 138));
                db.Add(new Language("Lahnda", 30, 70));
                db.Add(new Language("Marathi", 23, 74));
                db.Add(new Language("Telugu", 17, 80));
                db.Add(new Language("Turkish", 39, 35));
                db.Add(new Language("Korean", 37, 127));
                db.Add(new Language("French", 46, 2));
                db.Add(new Language("German", 51, 9));
                db.Add(new Language("Vietnamese", 16, 106));
                db.Add(new Language("Urdu", 25, 74));
                db.Add(new Language("Italian", 42, 12));
                db.Add(new Language("Hausa", 10, 8));
                db.Add(new Language("Indonesian", -5, 120));
            }

            if (!db.Temperaments.Any())
            {
                db.Add(new Temperament("Сholeric", 1));
                db.Add(new Temperament("Sanguine", 2));
                db.Add(new Temperament("Phlegmatic", 3));
                db.Add(new Temperament("Melancholic", 4));
            }

            if (!db.ConfigKeys.Any())
            {
                db.Add(new ConfigKey("LearningIterations", "1000"));
                db.Add(new ConfigKey("LearningRate", "0.15"));
                db.Add(new ConfigKey("HiddenLayerLength", "100"));
            }

            db.SaveChanges();
        }
    }
}
