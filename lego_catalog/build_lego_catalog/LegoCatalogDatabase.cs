using System;
using System.Collections.Generic;
using System.Data.SQLite;
using build_lego_catalog.Models;
using build_lego_catalog;
using Dapper;

namespace LegoCatalogLib
{
    public class LegoCatalogDatabase
    {
        private readonly string _connectionString;

        public LegoCatalogDatabase(string dbPath)
        {
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        public void CreateDatabase()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Categories (
                    CategoryID TEXT PRIMARY KEY,
                    CategoryName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Colors (
                    ColorID TEXT PRIMARY KEY,
                    ColorName TEXT NOT NULL,
                    RGB TEXT,
                    Type TEXT,
                    Parts INTEGER,
                    InSets INTEGER,
                    Wanted INTEGER,
                    ForSale INTEGER,
                    YearFrom INTEGER,
                    YearTo INTEGER
                );

                CREATE TABLE IF NOT EXISTS ItemTypes (
                    ItemTypeID TEXT PRIMARY KEY,
                    ItemTypeName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Parts (
                    Number TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    CategoryID TEXT,
                    AlternateItemNumber TEXT,
                    Weight TEXT,
                    Dimensions TEXT,
                    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
                );

                CREATE TABLE IF NOT EXISTS Sets (
                    Number TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    CategoryID TEXT,
                    YearReleased INTEGER,
                    Weight TEXT,
                    Dimensions TEXT,
                    ItemTypeID TEXT,
                    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
                    FOREIGN KEY (ItemTypeID) REFERENCES ItemTypes(ItemTypeID)
                );

                CREATE TABLE IF NOT EXISTS Minifigures (
                    Number TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    CategoryID TEXT,
                    YearReleased INTEGER,
                    Weight TEXT,
                    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
                );

                CREATE TABLE IF NOT EXISTS Codes (
                    ItemNo TEXT,
                    Color TEXT,
                    CodeValue TEXT,
                    PRIMARY KEY (ItemNo, Color)
                );
            ");
        }

        public void ImportData(DataStore dataStore)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                ImportCategories(connection, dataStore.Categories);
                ImportColors(connection, dataStore.Colors);
                ImportItemTypes(connection, dataStore.ItemTypes);
                ImportParts(connection, dataStore.Parts);
                ImportSets(connection, dataStore.Books);
                ImportMinifigures(connection, dataStore.Minifigures);
                ImportCodes(connection, dataStore.Codes);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private void ImportCategories(SQLiteConnection connection, List<Category> categories)
        {
            connection.Execute(@"
                INSERT OR REPLACE INTO Categories (CategoryID, CategoryName) 
                VALUES (@CategoryID, @CategoryName)", categories);
        }

        private void ImportColors(SQLiteConnection connection, List<Color> colors)
        {
            connection.Execute(@"
                INSERT OR REPLACE INTO Colors 
                (ColorID, ColorName, RGB, Type, Parts, InSets, Wanted, ForSale, YearFrom, YearTo) 
                VALUES (@ColorID, @ColorName, @RGB, @Type, @Parts, @InSets, @Wanted, @ForSale, @YearFrom, @YearTo)", colors);
        }

        private void ImportItemTypes(SQLiteConnection connection, List<ItemType> itemTypes)
        {
            connection.Execute(@"
                INSERT OR REPLACE INTO ItemTypes (ItemTypeID, ItemTypeName) 
                VALUES (@ItemTypeID, @ItemTypeName)", itemTypes);
        }

        private void ImportParts(SQLiteConnection connection, List<Part> parts)
        {
            connection.Execute(@"
                INSERT OR REPLACE INTO Parts 
                (Number, Name, CategoryID, AlternateItemNumber, Weight, Dimensions) 
                VALUES (@Number, @Name, @CategoryID, @AlternateItemNumber, @Weight, @Dimensions)", parts);
        }

        private void ImportSets(SQLiteConnection connection, List<Book> books)
        {
            connection.Execute(@"
                INSERT OR REPLACE INTO Sets 
                (Number, Name, CategoryID, YearReleased, Weight, Dimensions, ItemTypeID) 
                VALUES (@Number, @Name, @CategoryID, @YearReleased, @Weight, @Dimensions, @ItemTypeID)",
                books.Select(b => new
                {
                    b.Number,
                    b.Name,
                    b.CategoryID,
                    YearReleased = int.TryParse(b.YearReleased, out int year) ? year : (int?)null,
                    b.Weight,
                    b.Dimensions,
                    ItemTypeID = (string)null // You might need to set this based on the type of book
                }));
        }

        private void ImportMinifigures(SQLiteConnection connection, List<Catalog> minifigures)
        {
            connection.Execute(@"
                INSERT OR REPLACE INTO Minifigures 
                (Number, Name, CategoryID, YearReleased, Weight) 
                VALUES (@Number, @Name, @CategoryID, @YearReleased, @Weight)",
                minifigures.Select(m => new
                {
                    m.Number,
                    m.Name,
                    m.CategoryID,
                    YearReleased = int.TryParse(m.YearReleased, out int year) ? year : (int?)null,
                    m.Weight
                }));
        }

        private void ImportCodes(SQLiteConnection connection, List<Code> codes)
        {
            connection.Execute(@"
                INSERT OR REPLACE INTO Codes (ItemNo, Color, CodeValue) 
                VALUES (@ItemNo, @Color, @CodeValue)", codes);
        }

        public int GetCategoryCount()
        {
            using var connection = new SQLiteConnection(_connectionString);
            return connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Categories");
        }
    }
}
