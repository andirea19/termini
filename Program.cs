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


}
