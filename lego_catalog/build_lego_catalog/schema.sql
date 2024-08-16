-- Categories
CREATE TABLE Categories (
    CategoryID TEXT PRIMARY KEY,
    CategoryName TEXT NOT NULL
);

-- Colors
CREATE TABLE Colors (
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

-- ItemTypes
CREATE TABLE ItemTypes (
    ItemTypeID TEXT PRIMARY KEY,
    ItemTypeName TEXT NOT NULL
);

-- Parts
CREATE TABLE Parts (
    Number TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    CategoryID TEXT,
    AlternateItemNumber TEXT,
    Weight TEXT,
    Dimensions TEXT,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Sets (including Books, Gear, Original Boxes, Instructions)
CREATE TABLE Sets (
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

-- Minifigures
CREATE TABLE Minifigures (
    Number TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    CategoryID TEXT,
    YearReleased INTEGER,
    Weight TEXT,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Codes
CREATE TABLE Codes (
    ItemNo TEXT,
    Color TEXT,
    CodeValue TEXT,
    PRIMARY KEY (ItemNo, Color)
);