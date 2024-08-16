using System;

internal class Program
{
    private static void Main(string[] args)
    {

        static bool MatchPattern(string inputLine, string pattern)
        {

            // Handle the case where pattern starts with '^'
            if (pattern.StartsWith('^'))
            {
                pattern = pattern.Substring(1);
                return MatchHere(inputLine, pattern);
            }

            // Handle the case where pattern ends with '$'
            if (pattern.EndsWith('$'))
            {
                pattern = pattern.Substring(0, pattern.Length - 1);
                // Ensure the match is at the end
                int startIndex = inputLine.Length - pattern.Length;
                if (startIndex >= 0)
                {
                    string endSubstring = inputLine.Substring(startIndex);
                    return MatchHere(endSubstring, pattern);
                }
                return false;
            }

            // General case: check if the pattern matches anywhere in the inputLine
            for (int i = 0; i <= inputLine.Length; i++)
            {
                if (MatchHere(inputLine.Substring(i), pattern))
                {
                    return true;
                }
            }
            return false;
        }

        static bool MatchHere(string inputLine, string pattern, int inputIndex = 0, int patternIndex = 0)
        {
            //escapes
            if (patternIndex == pattern.Length) return true;
            if (inputIndex == inputLine.Length) return false;
            if (IsPlusOperator(pattern, patternIndex) || IsPlusZeroOrOneOperator(pattern, patternIndex))
            {
                return MatchOneOrMore(inputLine, pattern, inputIndex, patternIndex);
            }

            if (pattern[patternIndex] == '\\')
            {
                if (MatchEscapeSequence(inputLine, pattern, inputIndex, patternIndex))
                    return MatchHere(inputLine, pattern, inputIndex + 1, patternIndex + 2);
            }

            if (pattern[patternIndex] == '[')
            {
                if (MatchCharacterClass(inputLine, pattern, inputIndex, patternIndex))
                    return MatchHere(inputLine, pattern, inputIndex + 1, pattern.IndexOf(']', patternIndex) + 1);

            }

            if (pattern[patternIndex] == '$')
            {
                return inputIndex == inputLine.Length;
            }
            if (inputLine[inputIndex] == pattern[patternIndex])
            {
                return MatchHere(inputLine, pattern, inputIndex + 1, patternIndex + 1);
            }
            return false;
        }

        static bool IsPlusOperator(string pattern, int patternIndex)
        {
            if (patternIndex + 1 < pattern.Length)
            {
                if (pattern[patternIndex] == '\\')
                    return patternIndex + 2 < pattern.Length && pattern[patternIndex + 2] == '+';
                if (pattern[patternIndex] == '[')
                    return patternIndex + 1 < pattern.Length && pattern.IndexOf(']', patternIndex) + 1 == '+';

                return pattern[patternIndex + 1] == '+';
            }
            return false;
        }

        static bool IsPlusZeroOrOneOperator(string pattern, int patternIndex)
        {
            if (patternIndex + 1 < pattern.Length)
            {
                if (pattern[patternIndex] == '\\')
                    return patternIndex + 2 < pattern.Length && pattern[patternIndex + 2] == '?';
                if (pattern[patternIndex] == '[')
                    return patternIndex + 1 < pattern.Length && pattern.IndexOf(']', patternIndex) + 1 == '?';

                return pattern[patternIndex + 1] == '?';
            }
            return false;
        }

        static bool MatchOneOrMore(string inputLine, string pattern, int inputIndex, int patternIndex, int timesChecked = 0)
        {
            if (inputIndex == inputLine.Length)
            {
                if (pattern[patternIndex] == '\\')
                    return MatchHere(inputLine, pattern, inputIndex, patternIndex + 2);
                if (pattern[patternIndex] == '[')
                    return MatchHere(inputLine, pattern, inputIndex, pattern.IndexOf(']', patternIndex) + 2);
                return MatchHere(inputLine, pattern, inputIndex, patternIndex + 2);
            }
            if (pattern[patternIndex] == '\\')
            {
                if (MatchEscapeSequence(inputLine, pattern, inputIndex, patternIndex))
                    return MatchOneOrMore(inputLine, pattern, inputIndex + 1, patternIndex, timesChecked + 1);
            }
            if (pattern[patternIndex] == '[')
            {
                if (MatchCharacterClass(inputLine, pattern, inputIndex, patternIndex))
                    return MatchOneOrMore(inputLine, pattern, inputIndex + 1, patternIndex, timesChecked + 1);

            }
            if (inputLine[inputIndex] == pattern[patternIndex])
            {
                return MatchOneOrMore(inputLine, pattern, inputIndex + 1, patternIndex, timesChecked + 1);
            }

            if (timesChecked <= 1 && IsPlusZeroOrOneOperator(pattern, patternIndex))
            {
                if (pattern[patternIndex] == '\\')
                    return MatchHere(inputLine, pattern, inputIndex, patternIndex + 2);
                if (pattern[patternIndex] == '[')
                    return MatchHere(inputLine, pattern, inputIndex, pattern.IndexOf(']', patternIndex) + 2);
                return MatchHere(inputLine, pattern, inputIndex, patternIndex + 2);
            }

            if (timesChecked >= 1 && IsPlusOperator(pattern, patternIndex))
            {
                if (pattern[patternIndex] == '\\')
                    return MatchHere(inputLine, pattern, inputIndex, patternIndex + 2);
                if (pattern[patternIndex] == '[')
                    return MatchHere(inputLine, pattern, inputIndex, pattern.IndexOf(']', patternIndex) + 2);
                return MatchHere(inputLine, pattern, inputIndex, patternIndex + 2);
            }
            return false;



        }

        static bool MatchEscapeSequence(string inputLine, string pattern, int inputIndex, int patternIndex)
        {
            if (patternIndex + 1 >= pattern.Length) return false;

            switch (pattern[patternIndex + 1])
            {
                case 'd':
                    if (Char.IsDigit(inputLine[inputIndex]))
                        return true;
                    break;
                case 'w':
                    if (Char.IsLetterOrDigit(inputLine[inputIndex]))
                        return true;
                    break;
            }

            return false;
        }

        static bool MatchCharacterClass(string inputLine, string pattern, int inputIndex, int patternIndex)
        {
            int closingBracketIndex = pattern.IndexOf(']', patternIndex);
            if (closingBracketIndex == -1) return false;

            bool isNegated = pattern[patternIndex + 1] == '^';
            int charGroupStartIndex = isNegated ? patternIndex + 2 : patternIndex + 1;
            string charactersInGroup = pattern.Substring(charGroupStartIndex, closingBracketIndex - charGroupStartIndex);
            char inputChar = inputLine[inputIndex];
            bool charInGroup = charactersInGroup.Contains(inputChar);

            if ((isNegated && !charInGroup) || (!isNegated && charInGroup))
            {
                return true;
            }

            return false;
        }

        #region Main
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
        #endregion
    }
}