using System;

public class Geburtstag
{
    public string Name { get; set; }
    public DateTime Datum { get; set; }

    public Geburtstag(string name, DateTime datum)
    {
        Name = name;
        Datum = datum;
    }

    // nicht Birthday, sondern Geburtstag!
#warning NICHT Birthday, sondern Geburtstag!

    public override string ToString()
    {
        return $"{Name} - Geburtstag: {Datum:dd.MM.yyyy}";
    }
}
