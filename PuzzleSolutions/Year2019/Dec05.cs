using PuzzleSolutions.Year2019.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec05 : Solution
    {
        IntcodeComputer compy = new IntcodeComputer(5);

        public void Go(string[] fileLines)
        {
            var codes = parse(fileLines[0]);
            int? outp;
            do
            {
                outp = compy.ExecuteIntCodeOperationToHalting(codes);
            } while (!compy.IsComplete(codes));
            Console.WriteLine($"Here is the final output code of the intcode program: {outp}.");
        }

        private List<string> parse(string line)
        {
            return line.Split(',').ToList();
        }
    }
}