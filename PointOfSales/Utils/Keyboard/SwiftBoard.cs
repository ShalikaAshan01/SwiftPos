using System.Collections.Generic;
using PointOfSales.Core.Keyboard;

namespace PointOfSales.Utils.Keyboard;

public class SwiftBoard : IKeyboardLayout
{
    public List<List<VirtualKey>> GetLayout()
    {
        return
        [
            new List<VirtualKey>
            {
                new(("1", "1"), ("1", "1")),
                new(("2", "2"), ("2", "2")),
                new(("3", "3"), ("3", "3")),
                new(("4", "4"), ("4", "4")),
                new(("5", "5"), ("5", "5")),
                new(("6", "6"), ("6", "6")),
                new(("7", "7"), ("7", "7")),
                new(("8", "8"), ("8", "8")),
                new(("9", "9"), ("9", "9")),
                new(("0", "0"), ("0", "0")),
            },
            new List<VirtualKey>
            {
                new(("q", "q"), ("Q", "Q")),
                new(("w", "w"), ("W", "W")),
                new(("e", "e"), ("E", "E")),
                new(("r", "r"), ("R", "R")),
                new(("t", "t"), ("T", "T")),
                new(("y", "y"), ("Y", "Y")),
                new(("u", "u"), ("U", "U")),
                new(("i", "i"), ("I", "I")),
                new(("o", "o"), ("O", "O")),
                new(("p", "p"), ("P", "P")),

            },

            new List<VirtualKey>
            {
                new(("", ""), ("", ""), 0.5, KeyTypes.None),
                new(("a", "a"), ("A", "A")),
                new(("s", "s"), ("S", "S")),
                new(("d", "d"), ("D", "D")),
                new(("f", "f"), ("F", "F")),
                new(("g", "g"), ("G", "G")),
                new(("h", "h"), ("H", "H")),
                new(("j", "j"), ("J", "J")),
                new(("k", "k"), ("K", "K")),
                new(("l", "l"), ("L", "L")),
                new(("⏎", "⏎"), ("⏎", "⏎"), 1.5, KeyTypes.Enter),
            },

            new List<VirtualKey>
            {
                new(("↑", ""), ("↑", ""), 2, KeyTypes.Shift),
                new(("z", "z"), ("Z", "Z")),
                new(("x", "x"), ("X", "X")),
                new(("c", "c"), ("C", "C")),
                new(("v", "v"), ("V", "V")),
                new(("b", "b"), ("B", "B")),
                new(("n", "n"), ("N", "N")),
                new(("m", "m"), ("M", "M")),
                new(("⌫", ""), ("⌫", ""), 2, KeyTypes.Backspace),
            },

            new List<VirtualKey>
            {
                new(("+", "+"), ("+", "+"), 1, KeyTypes.Normal),
                new(("-", "-"), ("-", "-"), 1, KeyTypes.Normal),
                new(("*", "*"), ("*", "*"), 1, KeyTypes.Normal),
                new(("/", "/"), ("/", "/"), 1, KeyTypes.Normal),
                new(("=", "="), ("=", "="), 1, KeyTypes.Normal),
                new(("␣", " "), ("␣", " "), 7, KeyTypes.Space),
                new(("?", "?"), ("?", "?"), 1, KeyTypes.Normal),
                new(("@", "@"), ("@", "@"), 1, KeyTypes.Normal),
                new(("#", "#"), ("#", "#"), 1, KeyTypes.Normal),
                new((",", ","), (",", ","), 1, KeyTypes.Normal),
                new((".", "."), (".", "."), 1, KeyTypes.Normal),
            }
        ];
    }

    public (byte, byte, byte) GetBackgroundColor()
    {
        return (30, 32, 38);
    }

    public (byte, byte, byte) GetKeyBackgroundColor()
    {
        return (51, 53, 59);
    }

    public (byte, byte, byte) GetKeyBorderColor()
    {
        return (51, 53, 59);
    }
}