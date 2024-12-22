using System;


public class Task
{
    public string Beschreibung { get; set; }
    public DateTime Fälligkeitsdatum { get; set; }
    public bool IstErledigt { get; set; }

    public Task(string beschreibung, DateTime fälligkeitsdatum)
    {
        Beschreibung = beschreibung;
        Fälligkeitsdatum = fälligkeitsdatum;
        IstErledigt = false;
    }

    public void AlsErledigtMarkieren()
    {
        IstErledigt = true;
    }

    public override string ToString()
    {
        return $"{(IstErledigt ? "[X]" : "[ ]")} {Beschreibung} - Fällig am: {Fälligkeitsdatum:dd.MM.yyyy}";
    }
}
