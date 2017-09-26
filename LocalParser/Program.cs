using System;
using System.IO;
using Cataloguer.Models;


namespace LocalParser
{
    public class Program
    {
        public static void Main()
        {
            ArtistContext database = new ArtistContext();
            Artist artist1 = new Artist
            {
                Name = "qwerty"
            };
            database.Artists.Add(artist1);
            database.SaveChanges();

            string mainDirectoryName = @"D:\Music\";
            string[] artists = Directory.GetDirectories(mainDirectoryName);
            foreach (string artist in artists)
            {
                Console.WriteLine(artist);
                string[] folders = Directory.GetDirectories(artist);
                foreach (string folder in folders)
                {
                    Console.WriteLine(folder);
                }
            }
        }
    }
}
