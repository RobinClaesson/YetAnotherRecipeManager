﻿namespace RecipeManager.Shared.Models;

public enum Units
{
    None,
    Grams,
    Hectograms,
    Kilograms,
    Milliliters,
    Centiliters,
    Deciliters,
    Liters,
    Teaspoon,
    Tablespoon,
    Cup,
    Piece,
    ByTaste
}

public static class UnitsParser
{
    public static bool TryParse(string input, out Units result)
    {
        switch (input.Trim().ToLower())
        {
            case "g":
            case "grams":
            case "gram":
                result = Units.Grams;
                return true;
            case "hg":
            case "hectograms":
            case "hectogram":
                result = Units.Hectograms;
                return true;
            case "kg":
            case "kilogram":
            case "kilograms":
                result = Units.Kilograms;
                return true;
            case "ml":
            case "milliliter":
            case "milliliters":
                result = Units.Milliliters;
                return true;
            case "cl":
            case "centiliter":
            case "centiliters":
                result = Units.Centiliters;
                return true;
            case "dl":
            case "deciliter":
            case "deciliters":
                result = Units.Deciliters;
                return true;
            case "l":
            case "liter":
            case "liters":
                result = Units.Liters;
                return true;
            case "tsp":
            case "teaspoon":
            case "teaspoons":
                result = Units.Teaspoon;
                return true;
            case "tbsp":
            case "tablespoon":
            case "tablespoons":
                result = Units.Tablespoon;
                return true;
            case "cup":
            case "cups":
                result = Units.Cup;
                return true;
            case "piece":
            case "pieces":
                result = Units.Piece;
                return true;
            default:
                result = Units.None;
                return false;
        }
    }
}