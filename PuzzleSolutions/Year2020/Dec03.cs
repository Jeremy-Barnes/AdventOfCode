using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PuzzleSolutions.Year2020
{
    public class Dec03 : Solution
    {
        public void Go(string[] fileLines)
        {
            Console.WriteLine("Part 1 Trees Encountered: ");
            SkiFree(fileLines, 1, 3);

            long treesEncounteredMultiplied = 1;

            Console.WriteLine("Part 2 Trees Encountered Per Slope: ");
            treesEncounteredMultiplied *= SkiFree(fileLines, 1, 1);
            treesEncounteredMultiplied *= SkiFree(fileLines, 1, 3);
            treesEncounteredMultiplied *= SkiFree(fileLines, 1, 5);
            treesEncounteredMultiplied *= SkiFree(fileLines, 1, 7);
            treesEncounteredMultiplied *= SkiFree(fileLines, 2, 1);
            Console.WriteLine($"Trees multiplied: {treesEncounteredMultiplied}");
        }


        public int SkiFree(string[] fileLines, int downDelta, int rightDelta)
        {
            int yPos = 0;
            var trees = 0;
            int xPos = 0;
            int lineY = 0;
            foreach (var line in fileLines)
            {
                if(lineY < yPos)
                {
                    lineY++;
                    continue;
                }
                var linePattern = line.ToCharArray().ToList();
                while (xPos >= linePattern.Count)
                {
                    linePattern = linePattern.Concat(linePattern).ToList();
                }

                var cell = linePattern[xPos];
                if (cell == '#')
                {
                    trees++;
                }
                xPos = xPos + rightDelta;
                yPos += downDelta;
                lineY++;
            }

            Console.WriteLine(trees);
            return trees;
        }
    }
}
