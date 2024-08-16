using CsvHelper.Configuration.Attributes;

namespace build_lego_catalog.Models;

public class Part
{
    [Name("Category ID")]
    public string CategoryID { get; set; }

    [Name("Category Name")]
    public string CategoryName { get; set; }

    [Name("Number")]
    public string Number { get; set; }

    [Name("Name")]
    public string Name { get; set; }

    [Name("Alternate Item Number")]
    public string AlternateItemNumber { get; set; }

    [Name("Weight (in Grams)")]
    public string Weight { get; set; }

    [Name("Dimensions")]
    public string Dimensions { get; set; }
}