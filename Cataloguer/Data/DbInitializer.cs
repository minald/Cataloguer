using Cataloguer.Models;
using System.Linq;

namespace Cataloguer.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (!db.Countries.Any())
            {
                db.Add(new Country(/*1, */"Belarus", 1));
                db.Add(new Country(/*2, */"Ukraine", 2));
                db.Add(new Country(/*3, */"Russia", 3));
            }

            if (!db.Languages.Any())
            {
                db.Add(new Language("Russian", 1));
                db.Add(new Language("English", 2));
                db.Add(new Language("Spanish", 3));
            }

            if (!db.Temperaments.Any())
            {
                db.Add(new Temperament("TestTemperament1", 1));
                db.Add(new Temperament("TestTemperament2", 2));
                db.Add(new Temperament("TestTemperament3", 3));
                db.Add(new Temperament("TestTemperament4", 4));
            }

            db.SaveChanges();
        }
    }
}
