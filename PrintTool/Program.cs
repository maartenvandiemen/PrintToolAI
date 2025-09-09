// See https://aka.ms/new-console-template for more information
Console.WriteLine($"Print: {Splitter.Split(args)}");

internal static class Splitter
{
    internal static string Split(string[] args)
    {
        if (args.Length == 0)
            return string.Empty;
        if (args.Length == 1)
            return args[0];
        if (args.Length == 2)
            return $"{args[0]}, ";
        
        return string.Join(", ", args);
    }
}
