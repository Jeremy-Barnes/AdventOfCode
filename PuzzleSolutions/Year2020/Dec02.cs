using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PuzzleSolutions.Year2020
{
    public class Dec02 : Solution
    {
        public void Go(string[] fileLines)
        {
            Part1(fileLines);

            Part2(fileLines);
        }

        public void Part2(string[] fileLines)
        {
            List<string> validPasses = new List<string>();
            foreach (var line in fileLines)
            {
                var segs = line.Split(':');
                var rules = segs[0].Split(' ');
                var lims = rules[0].Split('-');
                var firstAllowablePosition = int.Parse(lims[0]) - 1;
                var alternateAllowablePosition = int.Parse(lims[1]) - 1;
                var charRule = char.Parse(rules[1]);

                var passwordMaybe = segs[1].Trim();
                var charTimes = passwordMaybe.Count(c => c == charRule);
                if ((passwordMaybe[firstAllowablePosition] == charRule && passwordMaybe[alternateAllowablePosition] != charRule) ||
                    (passwordMaybe[firstAllowablePosition] !=  charRule && passwordMaybe[alternateAllowablePosition] == charRule))
                {
                    validPasses.Add(passwordMaybe);
                }
            }
            Console.WriteLine(validPasses.Count);
        }



        public void Part1(string[] fileLines)
        {
            List<string> validPasses = new List<string>();
            foreach (var line in fileLines)
            {
                var segs = line.Split(':');
                var rules = segs[0].Split(' ');
                var lims = rules[0].Split('-');
                var lowerLim = int.Parse(lims[0]);
                var upperLim = int.Parse(lims[1]);

                var charRule = char.Parse(rules[1]);

                var passwordMaybe = segs[1].Trim();
                var charTimes = passwordMaybe.Count(c => c == charRule);
                if (charTimes >= lowerLim && charTimes <= upperLim)
                {
                    validPasses.Add(passwordMaybe);
                }
            }

            Console.WriteLine(validPasses.Count);
        }
    }
}
