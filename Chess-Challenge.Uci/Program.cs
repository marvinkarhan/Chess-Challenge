using ChessChallenge.Application;

namespace Chess_Challenge.Cli
{
    internal class Program
    {
        static (int, int) GetTokenCount()
        {
            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            string path = Path.Combine(parentDirectory.FullName, "Chess-Challenge", "src", "My Bot", "MyBot.cs");
            string txt = File.ReadAllText(path);
            return TokenCounter.CountTokens(txt);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Sebastian Lague's Chess Challenge submission by Gediminas Masaitis");
            var (totalTokens, debugTokens) = GetTokenCount();
            var tokensWithoutDebug = totalTokens - debugTokens;
            Console.WriteLine($"Current token count: {tokensWithoutDebug} ({totalTokens} total, {debugTokens} debug)");
            Console.WriteLine();

            var uci = new Uci();
            uci.Run();
        }
    }
}