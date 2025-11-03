namespace ConsoleRenderer;

public class DefaultValues {

    public static int width = 60;
    public static int height = 30;
    public static Dictionary<int, ConsoleColor> Colors2 = new Dictionary<int, ConsoleColor>() {
                { 0, ConsoleColor.Black },
                { 1, ConsoleColor.White },
            };
    public static Dictionary<int, ConsoleColor> Colors3 = new Dictionary<int, ConsoleColor>() {
                { 0, ConsoleColor.Black },
                { 1, ConsoleColor.DarkGray },
                { 3, ConsoleColor.White },
            };
    public static Dictionary<int, ConsoleColor> Colors4 = new Dictionary<int, ConsoleColor>() {
                { 0, ConsoleColor.Black },
                { 1, ConsoleColor.DarkGray },
                { 2, ConsoleColor.Gray },
                { 3, ConsoleColor.White },
            };

    public static ConsoleColor c_Text = ConsoleColor.White;
    public static ConsoleColor c_Cell = ConsoleColor.Black;

    public static int pixelsToCell = 20;

    public static List<KeyValuePair<char, char>> videoCellPairChars = new List<KeyValuePair<char, char>>() {
        new('[', ']'),
        new('(', ')'),
        new('{', '}'),
    };

    public static string loading = "|/―\\";

}
