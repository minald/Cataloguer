using System;
using System.Collections.Generic;

namespace Cataloguer.Models
{
    public abstract class CommonObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureLink { get; set; }
        public string Scrobbles { get; set; }
        public string Listeners { get; set; }

        public virtual List<Tag> Tags { get; set; } = new List<Tag>();

        public abstract string GetDefaultPictureLink();

        public void SetPictureLink(string pictureLink) 
            => PictureLink = String.IsNullOrWhiteSpace(PictureLink) ? GetDefaultPictureLink() : pictureLink;

        public void SetScrobbles(string scrobbles) => Scrobbles = NormalizeNumber(scrobbles);

        public void SetListeners(string listeners) => Listeners = NormalizeNumber(listeners);

        private string NormalizeNumber(string number)
        {
            int digits = number.Length;
            if (digits <= 3)
                return number;
            else
                number = number.Insert(digits - 3, " ");
            if (digits <= 6)
                return number;
            else
                number = number.Insert(digits - 6, " ");
            if (digits <= 9)
                return number;
            else
                return number.Insert(digits - 9, " ");
        }
    }
}
