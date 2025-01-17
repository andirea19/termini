using System;
using System.Data.SQLite;

namespace termini
{
    class Program
    {
        /// <summary>
        ///  Quellen für die Datenbanken definieren
        /// </summary>
        static string geburtstageDbPath = "Data Source=geburtstage.db";
        static string todosDbPath = "Data Source=to-dos.db";

        static void Main(string[] args)
        {
            //Datenbanken einbinden
            // TODO Datenbanken verbinden

            InitializeDatabase(geburtstageDbPath, "CREATE TABLE IF NOT EXISTS Geburtstage (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL, Datum TEXT NOT NULL)");
            InitializeDatabase(todosDbPath, "CREATE TABLE IF NOT EXISTS Todos (Id INTEGER PRIMARY KEY, Aufgabe TEXT NOT NULL, Datum TEXT)");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Willkommen zu unserer super geilen To-Do- und Kalenderanwendung!");
                Console.WriteLine("1. Aufgabe hinzufügen");
                Console.WriteLine("2. Aufgaben anzeigen");
                Console.WriteLine("3. Geburtstag anlegen");
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

        // Initialisiere SQLite-Datenbank --- using done
        static void InitializeDatabase(string dbPath, string createTableQuery)
        {
            using (var connection = new SQLiteConnection(dbPath))
            {
                connection.Open();
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // 1 To do hinzufügen
        static void AufgabeHinzufügen()
        {
            Console.Write("Gib die Beschreibung der Aufgabe ein: ");
            string beschreibung = Console.ReadLine();

            Console.Write("Gib das Fälligkeitsdatum ein (dd.MM.yyyy): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime datum))
            {
                using (var connection = new SQLiteConnection(todosDbPath))
                {
                    connection.Open();
                    string query = "INSERT INTO Todos (Aufgabe, Datum) VALUES (@aufgabe, @datum)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@aufgabe", beschreibung);
                        command.Parameters.AddWithValue("@datum", datum.ToString("yyyy-MM-dd"));
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Aufgabe erfolgreich hinzugefügt!");
            }
            else
            {
                Console.WriteLine("Datum Ungültig.");
            }
            Console.WriteLine("Drücke jede Taste, um fortzufahren...");
            Console.ReadKey();

            /* 2. Select nach dem Namen für todos
            / richtig zuordnen (Liste Gebtag - gibt dann notizen zuück)
            /boolean - zugriff (1:n) auf pri key
            */

        }

        // 4 Aufgaben anzeigen
        static void AufgabenAnzeigen()
        {
            Console.WriteLine("Deine Aufgaben:");
            using (var connection = new SQLiteConnection(todosDbPath))
            {
                connection.Open();
                string query = "SELECT Aufgabe, Datum FROM Todos";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string aufgabe = reader.GetString(0);
                        DateTime datum = DateTime.Parse(reader.GetString(1));
                        Console.WriteLine($"{aufgabe} - Fällig am {datum:dd.MM.yyyy}");
                    }
                }
            }
            Console.WriteLine("Drücke eine Taste, um fortzufahren...");
            Console.ReadKey();
        }

        // Geburtstag speichern
        static void GeburtstagSpeichern()
        {
            Console.Write("Gib den Namen ein: ");
            string name = Console.ReadLine();

            Console.Write("Gib das Geburtsdatum ein (dd.MM.yyyy): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime geburtsdatum))
            {
                using (var connection = new SQLiteConnection(geburtstageDbPath))
                {
                    connection.Open();
                    string query = "INSERT INTO Geburtstage (Name, Datum) VALUES (@name, @datum)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@datum", geburtsdatum.ToString("yyyy-MM-dd"));
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Geburtstag erfolgreich gespeichert!");
            }
            else
            {
                Console.WriteLine("Ungültiges Datum.");
            }
            Console.WriteLine("Drücke eine Taste, um fortzufahren...");
            Console.ReadKey();
        }

        // Geburtstage anzeigen
        static void GeburtstageAnzeigen()
        {
            Console.WriteLine("Gespeicherte Geburtstage:");
            using (var connection = new SQLiteConnection(geburtstageDbPath))
            {
                connection.Open();
                string query = "SELECT Name, Datum FROM Geburtstage";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        DateTime datum = DateTime.Parse(reader.GetString(1));
                        Console.WriteLine($"{name} - Geburtstag: {datum:dd.MM.yyyy}");
                    }
                }
            }
            Console.WriteLine("Drücke eine Taste, um fortzufahren...");
            Console.ReadKey();
        }
    }
}
