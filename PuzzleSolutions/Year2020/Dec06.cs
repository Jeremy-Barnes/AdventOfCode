using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuzzleSolutions.Year2020
{
    public class Dec06 : Solution
    {
        public void Go(string[] fileLines)
        {
            EvaluateLines(fileLines);
        }


        public void EvaluateLines(string[] fileLines )
        {

            var yesses = 0;
            var universalYesses = 0;
            List<string> currentGroup = new List<string>();

            foreach (var line in fileLines)
            {
                if (line.Length > 0)
                {
                    currentGroup.Add(line);
                } 
                else 
                {
                    var evaluation = Evaluate(currentGroup);
                    yesses += evaluation.Item1;
                    universalYesses += evaluation.Item2;
                    currentGroup.Clear();
                }
            }
            var evaluation2 = Evaluate(currentGroup);
            yesses += evaluation2.Item1;
            universalYesses += evaluation2.Item2;


            Console.WriteLine($"Yes answers {yesses}");
            Console.WriteLine($"Universal Yesses {universalYesses}");

        }

        private (int, int) Evaluate(List<string> groupAnswers)
        {
            int yesses = 0;
            int universalYesses = 0;
            Dictionary<char, int> answerCountPerGroup = new Dictionary<char, int>();

            foreach (var personAnswers in groupAnswers)
            {
                foreach (char answer in personAnswers)
                {
                    if (!answerCountPerGroup.ContainsKey(answer))
                    {
                        yesses++;
                        answerCountPerGroup.Add(answer, 1);
                    } else
                    {
                        answerCountPerGroup[answer] = answerCountPerGroup[answer] + 1;

                    }
                }
            }

            foreach(var kvp in answerCountPerGroup)
            {
                if(kvp.Value == groupAnswers.Count)
                {
                    universalYesses++;
                }
            }

            return (yesses, universalYesses);
        }

    }
}
