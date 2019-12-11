using PuzzleSolutions.Year2019.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec09 : Solution
    {
        IntcodeComputer compy = new IntcodeComputer(2);


        public void Go(string[] fileLines)
        {
            var codes = parse(fileLines[0]);
            long? outp;
            do
            {
                outp = compy.ExecuteIntCodeOperationToHalting(codes);
            } while (!compy.IsComplete(codes));
            Console.WriteLine($"Here is the final output code of the intcode program: {outp}.");
        }

        private List<string> parse(string line)
        {
            
            var codes = line.Split(',').ToList();
            for(int i = 0; i < 10000; i++)
            {
                codes.Add("0");
            }
            return codes;
        }
    }
}