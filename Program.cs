using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace termini
{
using System;
using System.Collections.Generic;

class Program
{
    // Listen für Aufgaben und Geburtstage
    static List<Task> toDoListe = new List<Task>();
    static List<Birthday> geburtstage = new List<Birthday>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Willkommen zur To-Do- und Kalenderanwendung!");
            // Aufgaben an Geburtstage koppeln? 
            // Zeitversetzt anzeigen?
            Console.WriteLine("1. Aufgabe hinzufügen");
            Console.WriteLine("2. Aufgaben anzeigen");
            Console.WriteLine("3. Geburtstag anlegen");
            // Alternativ: Ende? 
            Console.WriteLine("4. Geburtstage anzeigen");
            Console.WriteLine("5. Programm beenden");
            Console.Write("Wähle eine Option: ");

            string auswahl = Console.ReadLine();
            switch (auswahl)
            {
                case "1":
                    AufgabeHinzufügen();
                    break;
                case "2":
                    AufgabenAnzeigen();
                    break;
                case "3":
                    GeburtstagSpeichern();
                    break;
                case "4":
                    GeburtstageAnzeigen();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Ungültige Auswahl. Drücke eine beliebige Taste...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Funktion: Aufgabe hinzufügen
    static void AufgabeHinzufügen()
    {
        Console.Write("Gib die Beschreibung der Aufgabe ein: ");
        string beschreibung = Console.ReadLine();

        Console.Write("Gib das Fälligkeitsdatum ein (dd.MM.yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime fälligkeitsdatum))
        {
            toDoListe.Add(new Task(beschreibung, fälligkeitsdatum));
            Console.WriteLine("Aufgabe erfolgreich hinzugefügt!");
        }
        else
        {
            Console.WriteLine("Ungültiges Datum.");
        }
        Console.WriteLine("Drücke eine Taste, um fortzufahren...");
        Console.ReadKey();
    }

    // Funktion: Aufgaben anzeigen
    static void AufgabenAnzeigen()
    {
        Console.WriteLine("Deine Aufgaben:");
        foreach (var aufgabe in toDoListe)
        {
            Console.WriteLine(aufgabe);
        }
        Console.WriteLine("Drücke eine Taste, um fortzufahren...");
        Console.ReadKey();
    }

    // Funktion definiert: Geburtstag anlegen (und speichern - Name??)
    static void GeburtstagSpeichern()
    {
        Console.Write("Gib den Namen ein: ");
        string name = Console.ReadLine();

// Info, wie das Datum angegeben wird
// Ev. spalten auf Tag, Monat und Jahr?

        Console.Write("Gib das Geburtsdatum ein (dd.MM.yyyy): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime datum))
        {
            geburtstage.Add(new Birthday(name, datum));
            Console.WriteLine("Geburtstag erfolgreich gespeichert!");
        }
        else
        {
            Console.WriteLine("Ungültiges Datum.");
        }
        Console.WriteLine("Drücke eine Taste, um fortzufahren...");
        Console.ReadKey();
    }

    // Funktion: Geburtstage anzeigen
    // Anzahl definieren? Ansichen pro Seite?
    static void GeburtstageAnzeigen()
    {
        Console.WriteLine("Gespeicherte Geburtstage:");
        foreach (var geburtstag in geburtstage)
        {
            Console.WriteLine(geburtstag);
        }
        Console.WriteLine("Drücke eine Taste, um fortzufahren...");
        Console.ReadKey();
    }
}
