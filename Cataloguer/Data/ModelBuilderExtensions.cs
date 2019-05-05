using Cataloguer.Models;
using Microsoft.EntityFrameworkCore;

namespace Cataloguer.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country(1, "Belarus", 1),
                new Country(2, "Ukraine", 2),
                new Country(3, "Russia", 3)
            );
        }
    }
}
