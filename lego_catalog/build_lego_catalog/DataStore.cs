using build_lego_catalog.Models;

namespace build_lego_catalog;

public class DataStore
{
    public List<Part> Parts { get; set; } = new List<Part>();
    public List<Catalog> Catalogs { get; set; } = new List<Catalog>();
    public List<Catalog> Minifigures { get; set; } = new List<Catalog>();
    public List<Book> Books { get; set; } = new List<Book>();
    public List<ItemType> ItemTypes { get; set; } = new List<ItemType>();
    public List<Code> Codes { get; set; } = new List<Code>();
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Color> Colors { get; set; } = new List<Color>();

    public void PrintSummary()
    {
        Console.WriteLine("\nData Summary:");
        Console.WriteLine($"Parts: {Parts.Count}");
        Console.WriteLine($"Catalogs: {Catalogs.Count}");
        Console.WriteLine($"Minifigures: {Minifigures.Count}");
        Console.WriteLine($"Books (including Gear, Sets, Boxes, Instructions): {Books.Count}");
        Console.WriteLine($"Item Types: {ItemTypes.Count}");
        Console.WriteLine($"Codes: {Codes.Count}");
        Console.WriteLine($"Categories: {Categories.Count}");
        Console.WriteLine($"Colors: {Colors.Count}");

        if (Colors.Any())
        {
            var randomColor = Colors[new Random().Next(Colors.Count)];
            Console.WriteLine($"\nRandom Color: {randomColor.ColorName} (RGB: {randomColor.RGB})");
        }

        if (Parts.Any())
        {
            var randomPart = Parts[new Random().Next(Parts.Count)];
            Console.WriteLine($"Random Part: {randomPart.Name} (Number: {randomPart.Number})");
        }
    }
}