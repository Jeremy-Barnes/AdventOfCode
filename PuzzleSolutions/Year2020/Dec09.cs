using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuzzleSolutions.Year2020
{
    public class Dec09 : Solution
    {
        public void Go(string[] fileLines)
        {
            Console.WriteLine("Part1: " + Eval(fileLines));

            Console.WriteLine("Part2: " + findBack(Eval(fileLines), fileLines.Select(i => long.Parse(i)).ToList()));
        }


        public long Eval(string[] fileLines)
        {
            Dictionary<long, long> instructionPolongerAndAccVal = new Dictionary<long, long>();
            var fileLinesL = fileLines.Select(i => long.Parse(i)).ToList();
            for (int i = 25; i < fileLines.Length; i++)
            {
                var currentLine = fileLines[i];
                if (isValid(long.Parse(currentLine), fileLinesL.GetRange(i-25, 25)) == (-1, -1)) return long.Parse(currentLine);
            }
            return -1;
        }

        public (long,long) isValid(long number, List<long> preamble)
        {
            for(int outer = 0; outer < preamble.Count; outer++)
            {
                var outNum = preamble[outer];
                for (int inner = outer+1; inner < preamble.Count; inner++)
                {
                    var inNum = preamble[inner];
                    if(outNum + inNum== number)
                    {
                        return (outer, inner);
                    }
                }
            }
            return (-1, -1);
        }

        public long? findBack(long invalidNum, List<long> fileLines)
        {
            try
            {
                var seriesStart = fileLines.Count-1;
                while (true)
                {
                    List<long> series = new List<long>();

                    for (int i = seriesStart; i >= 0; i--)
                    {
                        series.Add(fileLines[i]);
                        if (series.Count < 2) continue;
                        if (series.Sum() == invalidNum)
                        {
                            return series.Max() + series.Min();
                        }
                        if (series.Sum() > invalidNum)
                        {
                            seriesStart--;
                            break;
                        }
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine();
            }
            return null;
        }
    }
}
