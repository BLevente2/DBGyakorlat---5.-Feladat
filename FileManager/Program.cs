#nullable disable

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static string filePath;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Simple Database Application. This program allows you to add, modify, delete, and list people in a database.");
        InitializeDatabase();

        while (true)
        {
            Console.WriteLine("1. Add person");
            Console.WriteLine("2. Modify person");
            Console.WriteLine("3. Delete person");
            Console.WriteLine("4. List all people");
            Console.WriteLine("5. Exit");

            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddPerson();
                    break;
                case "2":
                    ModifyPerson();
                    break;
                case "3":
                    DeletePerson();
                    break;
                case "4":
                    ListPeople();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    // Initializes the database file path and creates the file if it does not exist
    static void InitializeDatabase()
    {
        Console.Write("Enter the file path for the database: ");
        filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            Console.WriteLine("Database file created.");
        }
        else
        {
            Console.WriteLine("Database file found.");
        }
    }

    static void AddPerson()
    {
        Console.Write("Enter person ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Console.Write("Enter person name: ");
        string name = Console.ReadLine();
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Invalid name.");
            return;
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Append)))
        {
            writer.Write(id);
            writer.Write(name);
        }

        Console.WriteLine("Person added successfully.");
    }

    static void ModifyPerson()
    {
        Console.Write("Enter person ID to modify: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        List<(int id, string name)> people = new List<(int id, string name)>();

        bool found = false;

        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.OpenOrCreate)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                int currentId = reader.ReadInt32();
                string currentName = reader.ReadString();
                if (currentId == id)
                {
                    Console.Write("Enter new name: ");
                    string newName = Console.ReadLine();
                    people.Add((currentId, newName));
                    found = true;
                }
                else
                {
                    people.Add((currentId, currentName));
                }
            }
        }

        if (found)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (var person in people)
                {
                    writer.Write(person.id);
                    writer.Write(person.name);
                }
            }

            Console.WriteLine("Person modified successfully.");
        }
        else
        {
            Console.WriteLine("Person not found.");
        }
    }

    static void DeletePerson()
    {
        Console.Write("Enter person ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        List<(int id, string name)> people = new List<(int id, string name)>();

        bool found = false;

        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.OpenOrCreate)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                int currentId = reader.ReadInt32();
                string currentName = reader.ReadString();
                if (currentId == id)
                {
                    found = true;
                }
                else
                {
                    people.Add((currentId, currentName));
                }
            }
        }

        if (found)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                foreach (var person in people)
                {
                    writer.Write(person.id);
                    writer.Write(person.name);
                }
            }

            Console.WriteLine("Person deleted successfully.");
        }
        else
        {
            Console.WriteLine("Person not found.");
        }
    }

    static void ListPeople()
    {
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.OpenOrCreate)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                int id = reader.ReadInt32();
                string name = reader.ReadString();
                Console.WriteLine($"ID: {id}, Name: {name}");
            }
        }
    }
}
