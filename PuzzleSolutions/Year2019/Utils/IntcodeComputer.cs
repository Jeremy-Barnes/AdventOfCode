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
        adjustRelativeBase = 9,
        exit = 99
    }

    public class IntcodeComputer
    {
        List<int> input;
        int index = 0;
        long relativeBase = 0;

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

        public long SetUpAndRunIntCode(string inputLine)
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

        public long ExecuteIntCodeOperationToCompletion(List<string> opCodes)
        {
            long output = 0;
            do
            {
                var operatorCode = new Instruction(opCodes[index], opCodes, index);
                output = operatorCode.operate(opCodes, () => { int result = input[0]; input.RemoveAt(0); return result; }, ref index, ref relativeBase) ?? output;
            } while (index < opCodes.Count);

            return output;
        }

        public long? ExecuteIntCodeOperationToHalting(List<string> opCodes)
        {
            long? output = null;
            do
            {
                var operatorCode = new Instruction(opCodes[index], opCodes, index);
                output = operatorCode.operate(opCodes, () => { int result = input[0]; input.RemoveAt(0); return result; }, ref index, ref relativeBase);
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
        public List<int> modePerOperand = new List<int>();
        public List<long> operands = new List<long>();

        public Instruction(string opCodeValue, List<string> intCodes, int instructionIndex)
        {
            var opCodeStr = string.Concat((opCodeValue.Length > 1 ? opCodeValue[opCodeValue.Length - 2] : '0'), opCodeValue[opCodeValue.Length - 1]);
            opCode = (OpCode)Int64.Parse(opCodeStr);

            if (opCode == (OpCode)99) return;

            string operandModeIndicators = opCodeValue.Length > 1 ? opCodeValue.Substring(0, opCodeValue.Length - 2) : "000";
            foreach (char integer in operandModeIndicators.Reverse())
            {
                modePerOperand.Add((int)char.GetNumericValue(integer));
            }
            foreach (int i in Enumerable.Range(0, 3 - operandModeIndicators.Length)) //fill in case its implicit
                modePerOperand.Add(0);

            operands.Add(Int64.Parse(intCodes[instructionIndex + 1]));

            if (opCode == (OpCode)3 || opCode == (OpCode)4 || opCode == (OpCode)9) return;

            operands.Add(Int64.Parse(intCodes[instructionIndex + 2]));

            if (opCode == (OpCode)5 || opCode == (OpCode)6) return;

            operands.Add(Int64.Parse(intCodes[instructionIndex + 3]));
        }

        public long? operate(List<string> codes, Func<int> input, ref int instructionPointer, ref long relativeBase)
        {
            long? output = null;
            if (opCode == OpCode.add)
            {
                var saveIndex = (int)getSaveValue((int)operands[2], modePerOperand[2], codes, relativeBase);
                var op1 = getOperandValue(operands[0], modePerOperand[0], codes, relativeBase);
                var op2 = getOperandValue(operands[1], modePerOperand[1], codes, relativeBase);
                codes[saveIndex] = (op1+op2).ToString();
            }
            if (opCode == OpCode.multiply)
            {

                codes[(int)getSaveValue((int)operands[2], modePerOperand[2], codes, relativeBase)] = (getOperandValue(operands[0], modePerOperand[0], codes, relativeBase) * getOperandValue(operands[1], modePerOperand[1], codes, relativeBase)).ToString();

            }
            if (opCode == OpCode.save)
            {
                codes[(int)getSaveValue((int)operands[0], modePerOperand[0], codes, relativeBase)] = input.Invoke().ToString();
            }
            if (opCode == OpCode.output)
            {
                //codes[0] = getOperandValue(operands[0], modePerOperand[0], codes, relativeBase).ToString();
                output = getOperandValue(operands[0], modePerOperand[0], codes, relativeBase);
               // Console.WriteLine(output);
            }
            if (opCode == OpCode.jumpTrue)
            {
                if (getOperandValue(operands[0], modePerOperand[0], codes, relativeBase) != 0)
                {
                    instructionPointer = (int)getOperandValue(operands[1], modePerOperand[1], codes, relativeBase);
                    return output;
                }

            }
            if (opCode == OpCode.jumpFalse)
            {
                if (getOperandValue(operands[0], modePerOperand[0], codes, relativeBase) == 0)
                {
                    instructionPointer = (int)getOperandValue(operands[1], modePerOperand[1], codes, relativeBase);
                    return output;
                }
            }
            if (opCode == OpCode.lessThan)
            {
                if (getOperandValue(operands[0], modePerOperand[0], codes, relativeBase) < getOperandValue(operands[1], modePerOperand[1], codes, relativeBase))
                {
                    codes[(int)getSaveValue((int)operands[2], modePerOperand[2], codes, relativeBase)] = "1";
                }
                else
                {
                    codes[(int)getSaveValue((int)operands[2], modePerOperand[2], codes, relativeBase)] = "0";
                }
            }
            if (opCode == OpCode.equals)
            {
                codes[(int)getSaveValue((int)operands[2], modePerOperand[2], codes, relativeBase)] = getOperandValue(operands[0], modePerOperand[0], codes, relativeBase) == getOperandValue(operands[1], modePerOperand[1], codes, relativeBase) ? "1" : "0";
            }
              if(opCode == OpCode.adjustRelativeBase)
            {
                relativeBase += getOperandValue(operands[0], modePerOperand[0], codes, relativeBase);
            }
            if (opCode == OpCode.exit)
            {
                instructionPointer = Int32.MaxValue;
                return Int32.Parse(codes[0]);
            }
            instructionPointer += operands.Count + 1;
            return output;
        }

        private long getOperandValue(long operand, int parameterMode, List<string> codes, long relativeBase)
        {
            if (parameterMode == 0)
            {
                var code = codes.ElementAtOrDefault((int)operand) ?? "0";
                return Int64.Parse(code);
            }
            if (parameterMode == 1)
            {
                return operand;
            }
            if (parameterMode == 2)
            {
                var code = codes.ElementAtOrDefault((int)(relativeBase + operand)) ?? "0";
                return Int64.Parse(code);
            }
            throw new Exception("Bad parameter given");
        }

        private long getSaveValue(long operand, int parameterMode, List<string> codes, long relativeBase)
        {
            if (parameterMode == 2)
            {
                return (relativeBase + operand);
            }
            return operand;

            //throw new Exception("Bad parameter given");
        }
    }
}
