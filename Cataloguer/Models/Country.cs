﻿namespace Cataloguer.Models
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Value1 { get; set; }

        public double Value2 { get; set; }

        public Country() { }

        public Country(int id, string name, double value1, double value2) 
            : this(name, value1, value2) => Id = id;

        public Country(string name, double value1, double value2)
        {
            Name = name;
            Value1 = value1;
            Value2 = value2;
        }
    }
}
