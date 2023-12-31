using System.Collections.Specialized;

partial class Program
{
    private static void Main(string[] args)
    {
        Welcome0257();
        Welcome4079();
        Console.ReadKey();
    }

    static partial void Welcome4079();
    private static void Welcome0257()
    {
        Console.WriteLine("Enter your name: ");
        string theName = Console.ReadLine();
        Console.WriteLine("{0}, welcome to my first console application", theName);
    }
}