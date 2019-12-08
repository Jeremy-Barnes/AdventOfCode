using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019.Utils
{
    public enum OpCode
    {
        add = 1,
        multiply = 2,
        save = 3,
        output = 4,
        jumpTrue = 5,
        jumpFalse = 6,
        lessThan = 7,
        equals = 8,
        exit = 99
    }

    public class IntcodeComputer
    {
        List<int> input;
        int index = 0;

        public IntcodeComputer(List<int> input)
        {
            this.input = input;
        }

        public IntcodeComputer(int input)
        {
            this.input = new List<int> { input };
        }

        public void AddInput(int input)
        {
            this.input.Add(input);
        }

        public int SetUpAndRunIntCode(string inputLine)
        {
            try
            {
                return ExecuteIntCodeOperationToCompletion(parse(inputLine));
            }
            catch (Exception ex)
            {
                return Int32.MinValue;
            }
            throw new Exception("How did you even get here!");
        }

        public int ExecuteIntCodeOperationToCompletion(List<string> opCodes)
        {
            int output = 0;
            do
            {
                var operatorCode = new Instruction(opCodes[index], opCodes, index);
                output = operatorCode.operate(opCodes, () => { int result = input[0]; input.RemoveAt(0); return result; }, ref index) ?? output;
            } while (index < opCodes.Count);

            return output;
        }

        public int? ExecuteIntCodeOperationToHalting(List<string> opCodes)
        {
            int? output = null;
            do
            {
                var operatorCode = new Instruction(opCodes[index], opCodes, index);
                output = operatorCode.operate(opCodes, () => { int result = input[0]; input.RemoveAt(0); return result; }, ref index);
            } while (output == null);

            return output;
        }

        public bool IsComplete(List<string> opCodes)
        {
            return index >= opCodes.Count;
        }

        public List<string> parse(string line)
        {
            return line.Split(',').ToList();
        }

    }

    public class Instruction
    {
        public OpCode opCode;
        public List<bool> positionModePerOperand = new List<bool>();
        public List<int> operands = new List<int>();

        public Instruction(string opCodeValue, List<string> intCodes, int instructionIndex)
        {
            var opCodeStr = string.Concat((opCodeValue.Length > 1 ? opCodeValue[opCodeValue.Length - 2] : '0' ), opCodeValue[opCodeValue.Length - 1]);
            opCode = (OpCode)Int32.Parse(opCodeStr);

            if (opCode == (OpCode)99) return;

            string operandModeIndicators = opCodeValue.Length > 1 ? opCodeValue.Substring(0, opCodeValue.Length - 2) : "000";
            foreach (char integer in operandModeIndicators.Reverse())
            {
                if (integer == '0')
                {
                    positionModePerOperand.Add(true);
                }
                else
                {
                    positionModePerOperand.Add(false);
                }
            }
            foreach (int i in Enumerable.Range(0, 3 - operandModeIndicators.Length)) //
                positionModePerOperand.Add(true);

            operands.Add(Int32.Parse(intCodes[instructionIndex + 1]));

            if (opCode == (OpCode)3 || opCode == (OpCode)4) return;

            operands.Add(Int32.Parse(intCodes[instructionIndex + 2]));

            if (opCode == (OpCode)5 || opCode == (OpCode)6) return;

            operands.Add(Int32.Parse(intCodes[instructionIndex + 3]));
        }

        public int? operate(List<string> codes, Func<int> input, ref int instructionPointer)
        {
            int? output = null;
            if (opCode == OpCode.add)
            {
                codes[operands[2]] = (getOperandValue(operands[0], positionModePerOperand[0], codes) + getOperandValue(operands[1], positionModePerOperand[1], codes)).ToString();
            }
            if (opCode == OpCode.multiply)
            {
                codes[operands[2]] = (getOperandValue(operands[0], positionModePerOperand[0], codes) * getOperandValue(operands[1], positionModePerOperand[1], codes)).ToString();

            }
            if (opCode == OpCode.save)
            {
                codes[operands[0]] = input.Invoke().ToString();
            }
            if (opCode == OpCode.output)
            {
                codes[0] = getOperandValue(operands[0], positionModePerOperand[0], codes).ToString();
                output = Int32.Parse(codes[0]);
            }
            if (opCode == OpCode.jumpTrue)
            {
                if (getOperandValue(operands[0], positionModePerOperand[0], codes) != 0)
                {
                    instructionPointer = getOperandValue(operands[1], positionModePerOperand[1], codes);
                    return output;
                }

            }
            if (opCode == OpCode.jumpFalse)
            {
                if (getOperandValue(operands[0], positionModePerOperand[0], codes) == 0)
                {
                    instructionPointer = getOperandValue(operands[1], positionModePerOperand[1], codes);
                    return output;
                }
            }
            if (opCode == OpCode.lessThan)
            {
                if (getOperandValue(operands[0], positionModePerOperand[0], codes) < getOperandValue(operands[1], positionModePerOperand[1], codes))
                {
                    codes[operands[2]] = "1";
                }
                else
                {
                    codes[operands[2]] = "0";
                }
            }
            if (opCode == OpCode.equals)
            {
                codes[operands[2]] = getOperandValue(operands[0], positionModePerOperand[0], codes) == getOperandValue(operands[1], positionModePerOperand[1], codes) ? "1" : "0";
            }
            if (opCode == OpCode.exit)
            {
                instructionPointer = Int32.MaxValue;
                return Int32.Parse(codes[0]);
            }
            instructionPointer += operands.Count + 1;
            return output;
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
