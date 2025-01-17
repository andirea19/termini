using System;
using System.Data.SQLite;

namespace termini
{
    class Program
    {
        /// <summary>
        ///  Quellen für die Datenbanken definieren
        /// </summary>
        static readonly string geburtstageDbPath = "Data Source=geburtstage.db";
        static readonly string todosDbPath = "Data Source=to-dos.db";

        static void Main(string[] args)
        {
            //Datenbanken einbinden
            // ID noch anlegen! 
            // Foreign Key notwendig

        

                InitializeDatabase(geburtstageDbPath, "CREATE TABLE IF NOT EXISTS " +
                "Geburtstage (Id INTEGER PRIMARY KEY, Name TEXT NOT NULL," +
                "Datum TEXT NOT NULL, FOREIGN KEY (Id) REFERENCES todos(Id))");


            InitializeDatabase(todosDbPath, @"
    CREATE TABLE IF NOT EXISTS Todos (
        Id INTEGER PRIMARY KEY,
        Aufgabe TEXT NOT NULL,
        Datum TEXT,
        GeburtstagId INTEGER,
        FOREIGN KEY (GeburtstagId) REFERENCES Geburtstage(Id)
    )
");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Willkommen zu unserer super geilen To-Do- und Kalenderanwendung!");
                Console.WriteLine("1. Aufgabe hinzufügen.");
                Console.WriteLine("2. Aufgaben anzeigen.");
                Console.WriteLine("3. Geburtstag anlegen.");
                Console.WriteLine("4. Geburtstage anzeigen.");
                Console.WriteLine("5. Aufgabe zu Geburtstag hinzufügen.");
                Console.WriteLine("6. Geburtstage mit Aufgaben anzeigen.");

                Console.WriteLine("7. Geburtstag löschen");
                Console.WriteLine("8. To-do löschen");

                Console.WriteLine("9. Programm beenden.");

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
                        AufgabeZuGeburtstagHinzufügen();
                        break;
                    case "6":
                        GeburtstageMitAufgabenAnzeigen();
                        break;
                    case "7":
                        GeburtstagLöschen();
                        break;
                    case "8":
                        ToDoLöschen();
                        break;
                    case "9":
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

        // 2.5 Geburtstag speichern
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
#warning "Geburtstage und Todos sind noch nicht stabil verbunden"  
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

        /// <summary> Aufgabe hinzufügen
        /// Ok, schwierig. Wie verbinde ich die beiden Sachen?
        /// Info war, über ID.
        ///
        /// </summary>
        static void AufgabeZuGeburtstagHinzufügen()
        {
            Console.WriteLine("Wähle einen Geburtstag aus, um eine Aufgabe zuzuweisen:");
            using (var connection = new SQLiteConnection(geburtstageDbPath))
            {
                connection.Open();
                string query = "SELECT Id, Name, Datum FROM Geburtstage";
                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        DateTime datum = DateTime.Parse(reader.GetString(2));
                        Console.WriteLine($"{id}. {name} - {datum:dd.MM.yyyy}");
                    }
                }
            }

            Console.Write("Gib die ID des Geburtstags ein: ");
            if (int.TryParse(Console.ReadLine(), out int geburtstagId))
            {
                Console.Write("Gib die Beschreibung der Aufgabe ein: ");
                string aufgabe = Console.ReadLine();

                Console.Write("Gib das Fälligkeitsdatum ein (dd.MM.yyyy): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime fälligkeitsdatum))
                {
                    using (var connection = new SQLiteConnection(todosDbPath))
                    {
                        connection.Open();
                        string insertQuery = "INSERT INTO Todos (Aufgabe, Datum, GeburtstagId) VALUES (@aufgabe, @datum, @geburtstagId)";
                        using (var command = new SQLiteCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@aufgabe", aufgabe);
                            command.Parameters.AddWithValue("@datum", fälligkeitsdatum.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@geburtstagId", geburtstagId);
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("Aufgabe erfolgreich zugewiesen!");
                }
                else
                {
                    Console.WriteLine("Ungültiges Datum.");
                }
            }
            else
            {
                Console.WriteLine("Ungültige ID.");
            }
            Console.WriteLine("Drücke eine Taste, um fortzufahren...");
            Console.ReadKey();
        }
        // same as AufgabenAnzeigen, aber spicy

        static void GeburtstageMitAufgabenAnzeigen()
        {
            Console.WriteLine("Geburtstage mit zugehörigen Aufgaben:");
            using (var connection = new SQLiteConnection(geburtstageDbPath))
            {
                connection.Open();
                string query = @"
            SELECT g.Name, g.Datum, t.Aufgabe, t.Datum
            FROM Geburtstage g
            LEFT JOIN Todos t ON g.Id = t.GeburtstagId
            ORDER BY g.Datum";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        DateTime geburtstagsdatum = DateTime.Parse(reader.GetString(1));
                        string aufgabe = reader.IsDBNull(2) ? "Keine Aufgaben" : reader.GetString(2);
                        string aufgabedatum = reader.IsDBNull(3) ? "" : DateTime.Parse(reader.GetString(3)).ToString("dd.MM.yyyy");

                        Console.WriteLine($"{name} - {geburtstagsdatum:dd.MM.yyyy}");
                        if (!string.IsNullOrEmpty(aufgabe))
                        {
                            Console.WriteLine($"  -> Aufgabe: {aufgabe} (Fällig: {aufgabedatum})");
                        }
                    }
                }
            }
            Console.WriteLine("Drücke eine Taste, um fortzufahren...");
            Console.ReadKey();
        }

        /* static void GeburtstagLöschen()
         {
             Console.Write("Gib den Namen des Geburtstags ein, den du löschen möchtest: ");
             string name = Console.ReadLine();

                     command.Parameters.AddWithValue("@name", name);
                     int rowsAffected = command.ExecuteNonQuery();
                     if (rowsAffected > 0)
                     {
                         Console.WriteLine("Geburtstag erfolgreich gelöscht.");
                     }
                     else
                     {
                         Console.WriteLine("Keinen Geburtstag gefunden.");
                     }
                 }
             }
             Console.WriteLine("Drücke eine Taste, um fortzufahren...");
             Console.ReadKey();
         }

         */
        
        // TO DO - beides groß!

        static void ToDoLöschen()
        {
            Console.Write("Gib die Beschreibung der Aufgabe ein, die du löschen möchtest: ");
            string beschreibung = Console.ReadLine();

            using (var connection = new SQLiteConnection(todosDbPath))
            {
                connection.Open();
                string query = "DELETE FROM Todos WHERE Aufgabe = @aufgabe";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@aufgabe", beschreibung);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Aufgabe erfolgreich gelöscht.");
                    }
                    else
                    {
                        Console.WriteLine("Keine Aufgabe mit dieser Beschreibung gefunden.");
                    }
                }
            }
            Console.WriteLine("Drücke eine Taste, um fortzufahren...");
            Console.ReadKey();
        }


        static void GeburtstagLöschen()
        {
            Console.Write("Gib den Namen des Geburtstags ein, den du löschen möchtest: ");
            string name = Console.ReadLine();

            using (var connection = new SQLiteConnection(geburtstageDbPath))
            {
                connection.Open();
                string query = "DELETE FROM Geburtstage WHERE Name = @name";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Geburtstag erfolgreich gelöscht.");
                    }
                    else
                    {
                        Console.WriteLine("Kein Geburtstag mit diesem Namen gefunden.");
                    }
                }
            }
            Console.WriteLine("Drücke eine Taste, um fortzufahren...");
            Console.ReadKey();
        }




    }
}
