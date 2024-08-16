using System;
using build_lego_catalog;
using LegoCatalogLib;


namespace LegoCatalogApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define paths
                string baseDir = "../../../lego_catalog/src/";
                string dbPath = "LegoCatalog.db";

                Console.WriteLine("Starting Lego Catalog import process...");

                // Import data from CSV files
                Console.WriteLine("Importing data from CSV files...");
                var importer = new LegoCatalogImporter(baseDir);
                var dataStore = importer.ImportAll();

                Console.WriteLine("CSV import completed. Data summary:");
                dataStore.PrintSummary();

                // Create and populate SQLite database
                Console.WriteLine("\nCreating and populating SQLite database...");
                var database = new LegoCatalogDatabase(dbPath);

                Console.WriteLine("Creating database schema...");
                database.CreateDatabase();

                Console.WriteLine("Importing data into SQLite...");
                database.ImportData(dataStore);

                Console.WriteLine("Database import completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during the import process: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
