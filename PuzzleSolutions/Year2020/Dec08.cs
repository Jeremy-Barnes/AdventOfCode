using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuzzleSolutions.Year2020
{
    public class Dec08 : Solution
    {
        public void Go(string[] fileLines)
        {
            Console.WriteLine("Part1: " + FindLoop(fileLines));

            Console.WriteLine("Part2: " + FindBadOp(fileLines));
        }

        public int? FindLoop(string[] fileLines)
        {
            Dictionary<int, int> instructionPointerAndAccVal = new Dictionary<int, int>();

            for (int i = 0; i < fileLines.Length; i++)
            {
                string line = fileLines[i];
                var cmd = line.Split(' ');
                if (instructionPointerAndAccVal.ContainsKey(i))
                {
                    Console.WriteLine($"IP: {i}, AV: {accVal}");
                    return null;
                }
                var currentInstruction = i;
                switch (cmd[0])
                {
                    case "acc":
                        {
                            acc(int.Parse(cmd[1]));
                            break;
                        }
                    case "jmp":
                        {
                            i += int.Parse(cmd[1]) - 1;
                            break;
                        }
                    case "nop":
                        {
                            break;
                        }
                }

                instructionPointerAndAccVal.Add(currentInstruction, accVal);
            }
            return accVal;
        }

        int accVal = 0;
        public void acc(int byAmt)
        {
            accVal += byAmt;
        }

        public int? FindBadOp(string[] fileLines)
        {
            Dictionary<int, List<string>> changedPrograms = new Dictionary<int, List<string>>();
            var fileList = fileLines.ToList();
            for (int i = 0; i < fileLines.Length; i++)
            {
                var cmd = fileLines[i].Split(' ');
                string oldOp = null;
                string newOp = null;
                if (cmd[0].Contains("nop"))
                {
                    oldOp = "nop";
                    newOp = "jmp";
                }
                else if (cmd[0].Contains("jmp"))
                {
                    oldOp = "jmp";
                    newOp = "nop";
                }

                if (oldOp != null)
                {
                    List<string> newList = new List<string>(fileList);
                    newList[i] = newList[i].Replace(oldOp, newOp);
                    changedPrograms.Add(i, newList);
                }
            }

            foreach(var kvp in changedPrograms)
            {
                accVal = 0;
                var acc = FindLoop(kvp.Value.ToArray());
                if(acc != null)
                {
                    Console.WriteLine($"Winner: {kvp.Key} Acc {acc}");
                    return acc;
                }
            }
            return null;
        }
    }
}
