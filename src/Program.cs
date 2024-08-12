using System;

internal class Program
{
    private static void Main(string[] args)
    {
        static bool MatchPattern(string inputLine, string pattern)
        {
            return MatchHere(inputLine, pattern, inputLine);
        }

        static bool MatchHere (string remainingInput, string pattern, string inputline)
        {
            if (pattern == "") return true;
            if (remainingInput == "") return false;

            if (pattern.StartsWith("\\d"))
            {
                if (Char.IsDigit(remainingInput[0]))
                    return MatchHere(remainingInput.Substring(1),
                        pattern.Substring(2),
                        inputline);

                else
                    return MatchHere(remainingInput.Substring(1),
                        pattern,
                        inputline);
            }

            else if (pattern.StartsWith("\\w"))
            {
                if (Char.IsLetterOrDigit(remainingInput[0]))
                    return MatchHere(remainingInput.Substring(1),
                        pattern.Substring(2),
                        inputLine);

                else
                    return MatchHere(remainingInput.Substring(1),
                        pattern,
                        inputLine);
            }

            else if (pattern.StartsWith("[^"))
            {

                string charactersInNegativeCharacterGroup =

                    pattern.Substring(2, pattern.IndexOf(']') - 2);

                if (!charactersInNegativeCharacterGroup.Contains(remainingInput[0]))

                    return MatchHere(remainingInput.Substring(1),

                                     pattern.Substring(pattern.IndexOf(']') + 1),

                                     inputLine);

                else

                    return false;
            }
            else if (pattern.StartsWith("["))
            {

                string charactersInPositiveCharacterGroup =

                    pattern.Substring(1, pattern.IndexOf(']') - 1);

                if (charactersInPositiveCharacterGroup.Contains(remainingInput[0]))

                    return MatchHere(remainingInput.Substring(1),

                                     pattern.Substring(pattern.IndexOf(']') + 1),

                                     inputLine);

                else

                    return false;
            }
            else
            {
                if (remainingInput[0] == pattern[0])

                    return MatchHere(remainingInput.Substring(1), pattern.Substring(1),

                                     inputLine);

                else

                    return false;
            }




                if (args[0] != "-E")
        {
            Console.WriteLine("Expected first argument to be '-E'");
            Environment.Exit(2);
        }

        string pattern = args[1];
        string inputLine = Console.In.ReadToEnd();

        // You can use print statements as follows for debugging, they'll be visible when running tests.
        Console.WriteLine("Logs from your program will appear here!");


        if (MatchPattern(inputLine, pattern))
        {
            Console.WriteLine("Exit:0");
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Exit:1");
            Environment.Exit(1);
        }
    }
}