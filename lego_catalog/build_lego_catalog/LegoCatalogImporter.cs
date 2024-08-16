using build_lego_catalog.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace build_lego_catalog;

public class LegoCatalogImporter
{
    private readonly string _baseDir;
    private readonly CsvConfiguration _config;

    public LegoCatalogImporter(string baseDir)
    {
        _baseDir = baseDir;
        _config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null,
            HeaderValidated = null,
            BadDataFound = null,
            IgnoreBlankLines = true,
            Mode = CsvMode.RFC4180,
        };
    }

    public DataStore ImportAll()
    {
        var dataStore = new DataStore
        {
            Parts = ImportAndStore<Part>("Parts.txt"),
            Catalogs = ImportAndStore<Catalog>("Catalogs.txt"),
            Minifigures = ImportAndStore<Catalog>("Minifigures.txt"),
            Books = new List<Book>(),
            ItemTypes = ImportAndStore<ItemType>("itemtypes.txt"),
            Codes = ImportAndStore<Code>("codes.txt"),
            Categories = ImportAndStore<Category>("categories.txt"),
            Colors = ImportAndStore<Color>("colors.txt")
        };

        dataStore.Books.AddRange(ImportAndStore<Book>("Books.txt"));
        dataStore.Books.AddRange(ImportAndStore<Book>("Gear.txt"));
        dataStore.Books.AddRange(ImportAndStore<Book>("Sets.txt"));
        dataStore.Books.AddRange(ImportAndStore<Book>("Original Boxes.txt"));
        dataStore.Books.AddRange(ImportAndStore<Book>("Instructions.txt"));

        return dataStore;
    }

    private List<T> ImportAndStore<T>(string fileName)
    {
        string filePath = Path.Combine(_baseDir, fileName);
        try
        {
            var items = ImportCsv<T>(filePath);
            Console.WriteLine($"Imported {items.Count} items from {fileName}");
            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing {fileName}: {ex.Message}");
            return new List<T>();
        }
    }

    private List<T> ImportCsv<T>(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, _config))
        {
            var records = new List<T>();
            var recordsRead = 0;
            var errorCount = 0;

            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<T>();
                    if (record != null)
                    {
                        records.Add(record);
                    }
                    recordsRead++;
                }
                catch (Exception ex)
                {
                    errorCount++;
                    Console.WriteLine($"Error reading record {recordsRead + 1} in {Path.GetFileName(filePath)}: {ex.Message}");
                }
            }

            if (errorCount > 0)
            {
                Console.WriteLine($"Encountered {errorCount} errors while reading {Path.GetFileName(filePath)}");
            }

            return records;
        }
    }
}