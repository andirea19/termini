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

    public override string ToString()
    {
        return $"{Name} - Geburtstag: {Datum:dd.MM.yyyy}";
    }
}
