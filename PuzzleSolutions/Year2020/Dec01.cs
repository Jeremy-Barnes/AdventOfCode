using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleSolutions.Year2020
{
    public class Dec01 : Solution
    {
        public void Go(string[] fileLines)
        {
            //var stackDay1 = FindSum(0, 0, fileLines, 2, 0);
            var stackDay1 = IndeterminateLoopNester(fileLines, 1);

            int multiplied = 1;
            foreach(var kvp in stackDay1)
            {
                multiplied *= kvp.Value;
                Console.WriteLine($"Index - {kvp.Index}, Value - {kvp.Value} ");
            }
            Console.WriteLine($"All Multiplied: {multiplied}");

            var stackDay2 = IndeterminateLoopNester(fileLines, 2);

            multiplied = 1;
            foreach (var kvp in stackDay2)
            {
                multiplied *= kvp.Value;
                Console.WriteLine($"Index - {kvp.Index}, Value - {kvp.Value} ");
            }
            Console.WriteLine($"All Multiplied: {multiplied}");
        }

        public List<(int Index, int Value)> IndeterminateLoopNester(string[] fileLines, int requiredDepth)
        {
            return FindSum(0, -1, fileLines, requiredDepth, 0);
        }

        private List<(int, int)> FindSum(int leftOperand, int leftOperandIndex, string[] fileLines, int requiredDepth = 1, int currentDepth = 0)
        {
            for (int rightOperandIndex = leftOperandIndex + 1; rightOperandIndex < fileLines.Length; rightOperandIndex++)
            {
                int rightOperand = int.Parse(fileLines[rightOperandIndex]);

                if (requiredDepth == currentDepth && leftOperand + rightOperand == 2020)
                {
                    return new List<(int, int)>() { (rightOperandIndex, rightOperand) };
                }
                else if (requiredDepth != currentDepth)
                {
                    var stack = new List<(int, int)>() { (rightOperandIndex, rightOperand) };
                    var subStack = FindSum(leftOperand + rightOperand, rightOperandIndex, fileLines, requiredDepth, currentDepth + 1);
                    if (subStack != null)
                    {

                        return stack.Concat(subStack).ToList();
                    }
                } //else keep looping
            }
            return null;
        }

        private void FastSolution(string[] fileLines, bool part2 = false)
        {
            for (int outerIndex = 0; outerIndex < fileLines.Length; outerIndex++)
            {
                for (int innerIndex = outerIndex; innerIndex < fileLines.Length; innerIndex++)
                {

                    int outer = int.Parse(fileLines[outerIndex]);
                    int inner = int.Parse(fileLines[innerIndex]);

                    int multiplied = outer * inner;
                    int added = outer + inner;

                    if (part2) 
                    {
                        for (int innererIndex = innerIndex; innererIndex < fileLines.Length; innererIndex++)
                        {
                            int mostInner = int.Parse(fileLines[innererIndex]);

                            if (outer + inner + mostInner == 2020)
                            {
                                Console.WriteLine($"Multiplied: {(outer * inner * mostInner)} Indexes: {outerIndex} and {innerIndex} and {innererIndex} Values: {outer} amd {inner} and {mostInner}");
                            }
                        }
                    } else if(outer + inner == 2020)
                    {
                        if (outer + inner == 2020)
                        {
                            Console.WriteLine($"Multiplied: {(outer * inner)} Indexes: {outerIndex} and {innerIndex} Values: {outer} amd {inner}");
                        }
                    }
                }
            }
        }
    }
}
