using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec02 : Solution
    {
        public void Go(string[] lines)
        {

            var outputCaseDefault = setUpAndRunIntCode(lines, null); //just for debugging the problem, will never actually be a real problem answer

            Console.WriteLine($"Execution with no desired output and without altering the intcode program (debug mode) Output: {outputCaseDefault[0]}. \n");
            Console.WriteLine($"Final state of the intcode program: {string.Join(',', outputCaseDefault)}.");
            Console.WriteLine("\n\n\n");

            var outputCase1 = setUpAndRunIntCode(lines, null, 12, 2);

            Console.WriteLine($"Execution with no desired output and with Noun = 12, Verb = 2 (Case 1) Output: {outputCase1[0]}. \n");
            Console.WriteLine($"Final state of the intcode program: {string.Join(',', outputCase1)}.");
            Console.WriteLine("\n\n\n");

            int theCorrectOut = 19690720;
            var outputCase2 = setUpAndRunIntCode(lines, theCorrectOut, 0, 0);

            Console.WriteLine($"Execution with desired output {theCorrectOut} and with uncertain Noun And Verb (Case 2) Output: {outputCase2[0]}, Noun {outputCase2[1]}, Verb {outputCase2[2]}.\n");
            Console.WriteLine($"Final state of the intcode program: {string.Join(',', outputCase2)}.");
            Console.WriteLine("\n\n\n");

        }

        private List<int> setUpAndRunIntCode(string[] inputLines, int? seekingOutput = null, int? startingNoun = null, int? startingVerb = null)
        {
            try
            {
                foreach (string line in inputLines)
                {
                    var opCodes = parse(line);

                    if (!startingNoun.HasValue)
                        startingNoun = opCodes[1];
                    if (!startingVerb.HasValue)
                        startingVerb = opCodes[2];

                    //freshman CS nested for loops BAYBEEE
                    for (int noun = startingNoun.Value; noun < 99; noun++)
                    {
                        for (int verb = startingVerb.Value; verb < 99; verb++)
                        {
                            opCodes = parse(line); //memclear
                            opCodes[1] = noun;
                            opCodes[2] = verb;
                            var outputs = executeIntCodeOperation(opCodes);

                            if (seekingOutput.HasValue && outputs[0] == seekingOutput || !seekingOutput.HasValue)
                            {
                                return outputs;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            throw new Exception("How did you even get here!");
        }


        private List<int> executeIntCodeOperation(List<int> opCodes)
        {
            int index = 0;
            do
            {
                var operatorCode = opCodes[index];
                if (operatorCode == 99)
                {
                    break;
                }


                if (operatorCode == 1 || operatorCode == 2)
                {
                    int operand1 = opCodes[opCodes[index + 1]];
                    int operand2 = opCodes[opCodes[index + 2]];
                    int answerStorageIndex = opCodes[index + 3];

                    if (operatorCode == 1)
                    {
                        opCodes[answerStorageIndex] = operand1 + operand2;
                    }
                    else
                    {
                        opCodes[answerStorageIndex] = operand1 * operand2;
                    }
                }
                index += 4;

            } while (index < opCodes.Count);

            return opCodes;
        }

        private List<int> parse(string line)
        {
            return line.Split(',').ToList().ConvertAll(s => Int32.Parse(s)); ;
        }
    }
}
