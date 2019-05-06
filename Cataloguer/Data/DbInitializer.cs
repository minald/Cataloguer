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

            db.SaveChanges();
        }
    }
}
