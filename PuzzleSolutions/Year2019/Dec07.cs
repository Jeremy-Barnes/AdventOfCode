using PuzzleSolutions.Year2019.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec07 : Solution
    {
        List<int> phaseSequenceItems = new List<int> { 0, 1, 2, 3, 4 };
        List<int> phaseSequenceItemsSecondPart = new List<int> { 5, 6, 7, 8, 9 };

        public void Go(string[] fileLines)
        {
            var permutations = scramble(phaseSequenceItemsSecondPart);
            int maxOut = 0;

            var identifiers = new List<char> { 'a', 'b', 'c', 'd', 'e' };
            foreach (List<int> phaseSequence in permutations)
            {
                int output = 0;
                List<Amplifier> amps = new List<Amplifier>();

                //int dex = 0;
                foreach (int phase in phaseSequence)
                {
                    Amplifier amp = new Amplifier(phase, output, fileLines[0]);
                    output = amp.Amplify();
                    amps.Add(amp);
                }

                while(!amps.Last().IsComplete())
                foreach (var amp in amps)
                {
                    if (!amp.IsComplete())
                    {
                        amp.AddInput(output);
                        output = amp.Amplify();
                    }
                }
                if (maxOut < output) maxOut = output;

            }
            Console.WriteLine($"Solution to part 2 {maxOut}");

            maxOut = 0;
            permutations = scramble(phaseSequenceItems);
            foreach (List<int> phaseSequence in permutations)
            {
               int output = 0;
                foreach (int phase in phaseSequence)
                {
                    output = (int)new IntcodeComputer(new List<int> { phase, output }).SetUpAndRunIntCode(fileLines[0]);
                    if (maxOut < output) maxOut = output;
                }
            }
            Console.WriteLine($"Solution to part 1 {maxOut}");
        }

        private List<List<int>> scramble(List<int> items)
        {
            if (items.Count == 1)
                return new List<List<int>> { items };
            List<List<int>> scrambled = new List<List<int>>();
            foreach (var item in items)
            {
                var eggs = scramble(items.Except(new List<int> { item }).ToList());//her?
                eggs.ForEach(sublist => sublist.Insert(0, item));
                scrambled.AddRange(eggs);
            }
            return scrambled;
        }


        private class Amplifier
        {
            private IntcodeComputer intputer;
            private List<string> opCodes = new List<string>();
            public Amplifier(int phaseSetting, int input, string opCodes)
            {
                intputer = new IntcodeComputer(new List<int> { phaseSetting, input });
                this.opCodes = intputer.parse(opCodes);
            }

            public int Amplify()
            {
                return (int)intputer.ExecuteIntCodeOperationToHalting(opCodes).Value;
            }

            public void AddInput(int input)
            {
                intputer.AddInput(input);
            }

            public bool IsComplete()
            {
                return intputer.IsComplete(opCodes);
            }
        }
    }
}