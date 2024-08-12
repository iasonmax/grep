using System;

internal class Program
{
    private static void Main(string[] args)
    {
        static bool MatchPattern(string inputLine, string pattern)
        {
            return MatchHere(inputLine, pattern, inputLine);
        }

        static bool MatchHere(string remainingInput, string pattern2, string inputline2)
        {
            if (pattern2 == "") return true;
            if (remainingInput == "") return false;

            if (pattern2.StartsWith("\\d"))
            {
                if (Char.IsDigit(remainingInput[0]))
                    return MatchHere(remainingInput.Substring(1),
                        pattern2.Substring(2),
                        inputline2);

                else
                    return MatchHere(remainingInput.Substring(1),
                        pattern2,
                        inputline2);
            }

            else if (pattern2.StartsWith("\\w"))
            {
                if (Char.IsLetterOrDigit(remainingInput[0]))
                    return MatchHere(remainingInput.Substring(1),
                        pattern2.Substring(2),
                        inputline2);

                else
                    return MatchHere(remainingInput.Substring(1),
                        pattern2,
                        inputline2);
            }

            else if (pattern2.StartsWith("[^"))
            {

                string charactersInNegativeCharacterGroup =

                    pattern2.Substring(2, pattern2.IndexOf(']') - 2);

                if (!charactersInNegativeCharacterGroup.Contains(remainingInput[0]))

                    return MatchHere(remainingInput.Substring(1),

                                     pattern2.Substring(pattern2.IndexOf(']') + 1),

                                     inputline2);

                else

                    return false;
            }
            else if (pattern2.StartsWith("["))
            {

                string charactersInPositiveCharacterGroup =

                    pattern2.Substring(1, pattern2.IndexOf(']') - 1);

                if (charactersInPositiveCharacterGroup.Contains(remainingInput[0]))

                    return MatchHere(remainingInput.Substring(1),

                                     pattern2.Substring(pattern2.IndexOf(']') + 1),

                                     inputline2);

                else

                    return false;
            }
            else
            {
                if (remainingInput[0] == pattern2[0])

                    return MatchHere(remainingInput.Substring(1), pattern2.Substring(1),

                                     inputline2);

                else

                    return false;
            }
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