using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec04 : Solution
    {
        public void Go(string[] fileLines)
        {
            List<int> rangeEnds = parse(fileLines[0]);

            var timer = Stopwatch.StartNew();
            var validPins = evaluateRangeWithLookback(rangeEnds[0], rangeEnds[1]); //I know they're "passwords" in the problem statement but they're pin line and renaming is for suckers.
            timer.Stop();

            Console.WriteLine($"There are {validPins.Count} possible, valid passwords with threepeats permitted (case 1).");
            Console.WriteLine($"It took {timer.Elapsed.Milliseconds}ms. \n");
            //Console.WriteLine($"Here they are: {string.Join(',', validPins)}.");

            timer = Stopwatch.StartNew();
            validPins = evaluateRangeWithGroupings(rangeEnds[0], rangeEnds[1], 2);
            timer.Stop();

            Console.WriteLine($"There are {validPins.Count} possible, valid passwords without threepeats permitted (case 2) Then I reimplemented this version two more times for fun.");
            Console.WriteLine($"It took {timer.Elapsed.Milliseconds}ms. \n");
            //Console.WriteLine($"Here they are: {string.Join(',', validPins)}.");

            timer = Stopwatch.StartNew();
            validPins = evaluateRangeWithGroupingsCondensed(rangeEnds[0], rangeEnds[1], 2);
            timer.Stop();

            Console.WriteLine($"There are {validPins.Count} possible, valid passwords without threepeats permitted (case 2, reimplementation 1).");
            Console.WriteLine($"It took {timer.Elapsed.Milliseconds}ms. \n");
            //Console.WriteLine($"Here they are: {string.Join(',', validPins)}.");

            timer = Stopwatch.StartNew();
            var crimeCount = obscenityCounter(rangeEnds[0], rangeEnds[1], 2);
            timer.Stop();
            
            Console.WriteLine($"I wrote a stupid compact version of the same two implementations above. \nIt can't list all the successes, but it sure can count them! \nThere are" +
                $" {crimeCount} possible, valid passwords without threepeats permitted (case 2, reimplementation 2).");
            Console.WriteLine($"It took {timer.Elapsed.Milliseconds}ms. \n");

        }

        /// <summary>
        /// Ugly as sin but I got the answer to part one turned in short order
        /// Couldn't figure out how to make this one work without it getting huge and tangled for part 2. Interested in comments.
        /// </summary>
        private List<int> evaluateRangeWithLookback(int rangeStart, int rangeEnd)
        {
            List<int> validPins = new List<int>();
            for (int pinPossible = rangeStart; pinPossible < rangeEnd; pinPossible++) //go through all the ints in the range they gave us
            {
                string pinPossibleStr = pinPossible.ToString();
                List<int> pinPossibleExploded = pinPossibleStr.ToList().ConvertAll(digit => (int)char.GetNumericValue(digit));

                bool validPin = true; //innocent til proven guilty.
                bool adjacentRepeatFound = false; //good enough for case 1

                for (int digitIndex = 0; digitIndex < 6; digitIndex++)
                {
                    int pinDigit = pinPossibleExploded[digitIndex];
                    for (int lookbackIndex = digitIndex - 1; lookbackIndex >= 0; lookbackIndex--)
                    {
                        int lookbackDigit = pinPossibleExploded[lookbackIndex];
                        if (lookbackDigit > pinDigit) //unacceptable
                        {
                            validPin = false; //bail out immediately
                            break;
                        }
                        if (lookbackDigit == pinDigit)
                        {
                            adjacentRepeatFound = true;
                        }
                    }
                    if (!validPin)
                    {
                        break;
                    }
                }
                if (validPin && adjacentRepeatFound)
                {
                    validPins.Add(pinPossible);
                }
            }
            return validPins;
        }

        /// <summary>
        /// Actually pretty pleased with this implementation, its unconventional and, if I wanted to figure out something better than tuples, pretty efficient.
        /// All following implementations are just this boiled down.
        /// Faster even (barely) than the non-threepeat-checking implementation, which does a lot less string-and-tuple-fuckery.
        /// </summary>
        private List<int> evaluateRangeWithGroupings(int rangeStart, int rangeEnd, int maxGroupSize = 2)
        {
            List<int> validPins = new List<int>();
            for (int pinPossible = rangeStart; pinPossible < rangeEnd; pinPossible++)
            {
                string pinPossibleStr = pinPossible.ToString();
                var groups = groupDigits(pinPossibleStr);

                int previousValue = -1;
                bool doubleFound = false;
                bool decreaseFound = false;

                foreach (Tuple<char, int> group in groups)
                {
                    if (2 <= group.Item2 && group.Item2 <= maxGroupSize)
                    {
                        doubleFound = true;
                    }
                    if (previousValue > char.GetNumericValue(group.Item1))
                    {
                        decreaseFound = true;
                        break;
                    }
                    previousValue = (int)char.GetNumericValue(group.Item1);
                }
                if (!decreaseFound && doubleFound)
                {
                    validPins.Add(pinPossible);
                }
            }
            return validPins;
        }

        /// <summary>
        /// Helper function.
        /// Turns 111225 into {1,3}, {2,2}, {5,1}.
        /// Preserves order. Need to write a sort of class TupleNotStupid because immutables suck
        /// </summary>
        private List<Tuple<char, int>> groupDigits(string number)
        {
            List<Tuple<char, int>> adjacentGroupings = new List<Tuple<char, int>>();

            Tuple<char, int> currentCharGroup = null;
            foreach (char c in number)
            {
                if (currentCharGroup == null)
                {
                    currentCharGroup = new Tuple<char, int>(c, 1);
                }
                else
                {
                    if (currentCharGroup.Item1 == c)
                    {
                        currentCharGroup = new Tuple<char, int>(c, currentCharGroup.Item2 + 1);//ugh immutables
                    }
                    else
                    {
                        adjacentGroupings.Add(currentCharGroup);
                        currentCharGroup = new Tuple<char, int>(c, 1);
                    }
                }
            }
            adjacentGroupings.Add(currentCharGroup);
            return adjacentGroupings;
        }

        /// <summary>
        /// I kind of like the compactness and readability of this one.
        /// It also keeps the original number values intact, unlike the next implementation.
        /// Its about 2x slower as the original, less compact implementation.
        /// </summary>
        private List<int> evaluateRangeWithGroupingsCondensed(int rangeStart, int rangeEnd, int maxGroupSize = 2)
        {
            List<int> validPins = new List<int>();
            for (int pinPossible = rangeStart; pinPossible < rangeEnd; pinPossible++)
            {
                var groups = groupDigits(pinPossible.ToString());

                if (groups.Exists(group => 2 <= group.Item2 && group.Item2 <= maxGroupSize) &&
                    new string(groups.OrderBy(g => g.Item1).Select(g => g.Item1).ToArray()) == new string(groups.Select(g => g.Item1).ToArray()))
                {
                    validPins.Add(pinPossible);
                }
            }
            return validPins;
        }

        /// <summary>
        /// This is a crime. It loses the original numerical values, its hard to read, its way less efficient (about 6x slower than its originator), but its a LINQy one-liner.
        /// Just for you, Stuart.
        /// </summary>
        private int obscenityCounter(int rangeStart, int rangeEnd, int maxGroupSize = 2)
        {
            return Enumerable.Range(rangeStart, rangeEnd - rangeStart).ToList()
                    .ConvertAll(pin => groupDigits(pin.ToString()))
                    .Where(groups =>
                        groups.Exists(group => 2 <= group.Item2 && group.Item2 <= maxGroupSize) &&
                        new string(groups.OrderBy(g => g.Item1).Select(g => g.Item1).ToArray()) == new string(groups.Select(g => g.Item1)
                        .ToArray()))
                    .Count();
        }

        private List<int> parse(string fileLine)
        {
            var ends = fileLine.Split('-');
            return new List<int> {
                Int32.Parse(ends[0]),
                Int32.Parse(ends[1])
            };
        }
    }
}