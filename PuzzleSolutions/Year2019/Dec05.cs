using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec05 : Solution
    {
        int input = 5;
        public void Go(string[] fileLines)
        {
            var outputCaseDefault = setUpAndRunIntCode(fileLines, null);
            Console.WriteLine($"Here is the final output code of the intcode program: {outputCaseDefault[0]}.");
        }

        private List<string> setUpAndRunIntCode(string[] inputLines, int? seekingOutput = null, int? startingNoun = null, int? startingVerb = null)
        {
            try
            {
                foreach (string line in inputLines)
                {
                    return executeIntCodeOperation(parse(line));
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            throw new Exception("How did you even get here!");
        }


        private List<string> executeIntCodeOperation(List<string> opCodes)
        {
            int index = 0;
            do
            {
                var operatorCode = new Instruction(opCodes[index], opCodes, index);
                index += operatorCode.operate(opCodes, input, index);
            } while (index < opCodes.Count);

            return opCodes;
        }


        private List<string> parse(string line)
        {
            return line.Split(',').ToList();
        }


        private class Instruction
        {
            public Instruction(string opCodeValue, List<string> intCodes, int instructionIndex)
            {
                string theRest = "";
                if(opCodeValue.Length > 2)
                {
                    var backwards = opCodeValue.Reverse().ToList();
                    opCode = Int32.Parse(string.Concat(backwards[1], backwards[0]));
                    theRest = opCodeValue.Substring(0, opCodeValue.Length - 2);
                    foreach(char integer in theRest.Reverse())
                    {
                        if(integer == '0')
                        {
                            positionModeNotImmediateMode.Add(true);
                        }
                        else
                        {
                            positionModeNotImmediateMode.Add(false);
                        }
                    }
                   
                }
                else
                {
                    opCode = Int32.Parse(opCodeValue);
                }
                foreach (int i in Enumerable.Range(0, 3 - theRest.Length))
                    positionModeNotImmediateMode.Add(true);

                if (opCode == 1 || opCode == 2)
                {
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 1]));
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 2]));
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 3]));
                }
                if (opCode == 3)
                {
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 1]));
                }
                if (opCode == 4)
                {
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 1]));
                }
                if (opCode == 5 || opCode == 6)
                {
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 1]));
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 2]));
                }
                if (opCode == 7)
                {
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 1]));
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 2]));
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 3]));
                }
                if (opCode == 8)
                {
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 1]));
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 2]));
                    operands.Add(Int32.Parse(intCodes[instructionIndex + 3]));
                }
                if (opCode == 99)
                {
                    //psh
                }
            }

            public int opCode;
            public List<bool> positionModeNotImmediateMode = new List<bool>();
            public List<int> operands = new List<int>();

            public int operate(List<string> codes, int input, int instructionPointer)
            {
                if (opCode == 1)
                {
                    codes[operands[2]] = (getOperandValue(operands[0], positionModeNotImmediateMode[0], codes) + getOperandValue(operands[1], positionModeNotImmediateMode[1], codes)).ToString();
                }
                if (opCode == 2)
                {
                    codes[operands[2]] = (getOperandValue(operands[0], positionModeNotImmediateMode[0], codes) * getOperandValue(operands[1], positionModeNotImmediateMode[1], codes)).ToString();

                }
                if (opCode == 3)
                {
                    codes[operands[0]] = input.ToString();
                }
                if (opCode == 4)
                {
                    codes[0] = getOperandValue(operands[0], positionModeNotImmediateMode[0], codes).ToString();
                }
                if (opCode == 5)
                {
                    if(getOperandValue(operands[0], positionModeNotImmediateMode[0], codes) != 0) 
                    {
                        return ((-1) * instructionPointer) + getOperandValue(operands[1], positionModeNotImmediateMode[1], codes);
                    }

                }
                if (opCode == 6)
                {
                    if (getOperandValue(operands[0], positionModeNotImmediateMode[0], codes) == 0)
                    {
                        return ((-1) * instructionPointer) + getOperandValue(operands[1], positionModeNotImmediateMode[1], codes);
                    }
                }
                if (opCode == 7)
                {
                    if (getOperandValue(operands[0], positionModeNotImmediateMode[0], codes) < getOperandValue(operands[1], positionModeNotImmediateMode[1], codes))
                    {
                        codes[operands[2]] = "1";
                    } else
                    {
                        codes[operands[2]] = "0";
                    }
                }
                if (opCode == 8)
                {
                    if (getOperandValue(operands[0], positionModeNotImmediateMode[0], codes) == getOperandValue(operands[1], positionModeNotImmediateMode[1], codes))
                    {
                        codes[operands[2]] = "1";
                    }
                    else
                    {
                        codes[operands[2]] = "0";
                    }
                }
                if (opCode == 99)
                {
                    return int.MaxValue/2;
                }
                return operands.Count + 1;                
            }

            private int getOperandValue(int operand, bool positionModeNotImmediateMode, List<string> codes)
            {
                if (positionModeNotImmediateMode)
                {
                    return Int32.Parse(codes[operand]);
                }
                else
                {
                    return operand;
                }
            }
        }
    }
}