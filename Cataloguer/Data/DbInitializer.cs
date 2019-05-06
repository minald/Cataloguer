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
                //db.Add(new Country("Australia", 1));
                //db.Add(new Country("Ukraine", 2));
                //db.Add(new Country("Russia", 3));
            }

            if (!db.Languages.Any())
            {
                //db.Add(new Language("Russian", 1));
                //db.Add(new Language("English", 2));
                //db.Add(new Language("Spanish", 3));
            }

            if (!db.Temperaments.Any())
            {
                db.Add(new Temperament("Сholeric", 1));
                db.Add(new Temperament("Sanguine", 2));
                db.Add(new Temperament("Phlegmatic", 3));
                db.Add(new Temperament("Melancholic", 4));
            }

            db.SaveChanges();
        }
    }
}
