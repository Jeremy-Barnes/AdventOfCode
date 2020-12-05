using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuzzleSolutions.Year2020
{
    public class Dec05 : Solution
    {
        private List<string> RequiredFields = new List<string> {
        "byr",
        "iyr",
        "eyr",
        "hgt",
        "hcl",
        "ecl",
        "pid",
        };

        private List<string> OptionalFields = new List<string> {
            "cid"
        };

        public void Go(string[] fileLines)
        {
            Console.Write("Part 1: "); 
            EvaluateLines(fileLines);
        }


        public void EvaluateLines(string[] fileLines )
        {
            int[,] seatGrid = new int[128,8];

            var maxSeatId = 0;
            foreach (var line in fileLines)
            {
                var minY = 0;
                var maxY = 127;

                var minX = 0;
                var maxX = 7;
                for (int i = 0; i < 7; i++)
                {
                    var direction = line[i];
                    if(direction == 'F')
                    {
                        maxY = ((maxY-minY) / 2) + minY;
                    } else if(direction == 'B')
                    {
                        minY = maxY - ((maxY - minY) / 2);
                    }
                }

                for (int i = 7; i < 10; i++)
                {
                    var direction = line[i];
                    if (direction == 'L')
                    {
                        maxX = ((maxX - minX) / 2) + minX;
                    }
                    else if (direction == 'R')
                    {
                        minX = maxX - ((maxX - minX) / 2);
                    }
                }
                var currentSeatId = maxX + (maxY*8);
                seatGrid[maxY, maxX] = 1;
                maxSeatId = currentSeatId > maxSeatId ? currentSeatId : maxSeatId;
            }

            Console.WriteLine($"Max Seat ID {maxSeatId}");
        }


    }
}
