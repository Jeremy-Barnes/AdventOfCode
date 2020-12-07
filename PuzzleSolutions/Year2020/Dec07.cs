using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuzzleSolutions.Year2020
{
    public class Dec07 : Solution
    {
        public void Go(string[] fileLines)
        {
            EvaluateLines(fileLines);
        }

        public void EvaluateLines(string[] fileLines)
        {

            Dictionary<string, Dictionary<string, int>> bagColorAndRules = new Dictionary<string, Dictionary<string, int>>();
            foreach (var line in fileLines)
            {
                var bagAndContents = line.Split(new string[] { "contain" }, StringSplitOptions.None);
                bagColorAndRules.Add(cleanBagRule(bagAndContents[0]), countBagContents(bagAndContents[1]));
            }

            var searchBag = "shiny gold bag";
            Console.WriteLine($"Part 1: {searchBags(bagColorAndRules, searchBag, new List<string>())}");
            Console.WriteLine($"Part 2: {searchBagsPt2(bagColorAndRules, searchBag)}");
        }

        private int searchBags(Dictionary<string, Dictionary<string, int>> bagColorAndRules, string searchBag, List<string> alreadySearched)
        {
            int bags = 0;
            foreach(var bagRule in bagColorAndRules)
            {
                if (alreadySearched.Contains(bagRule.Key)) continue;
                if (bagRule.Value.ContainsKey(searchBag)){
                    alreadySearched.Add(bagRule.Key);
                    bags++;
                    bags += searchBags(bagColorAndRules, bagRule.Key, alreadySearched);
                }
            }
            return bags;
        }

        private int searchBagsPt2(Dictionary<string, Dictionary<string, int>> bagColorAndRules, string outerBag)
        {
            int bags = 0;
            foreach (var bagRule in bagColorAndRules)
            {
                if (bagRule.Key.Contains(outerBag))
                {
                    foreach(var interiorBags in bagRule.Value)
                    {
                        bags += interiorBags.Value * searchBagsPt2(bagColorAndRules, interiorBags.Key) + interiorBags.Value;
                    }
                }
            }
            return bags;
        }

        private Dictionary<string, int> countBagContents(string bagContentRule)
        {
            Dictionary<string, int> contentsOfBagAndCount = new Dictionary<string, int>();
            var bagRules = bagContentRule.Split(',');
            foreach (var rule in bagRules)
            {
                try
                {
                    var ruleCount = int.Parse(new string(rule.SkipWhile(c => !char.IsDigit(c))
                             .TakeWhile(c => char.IsDigit(c))
                             .ToArray())); //blows up when the bag is empty but who wants an empty bag?

                    var ruleBag = cleanBagRule(rule.Replace(ruleCount.ToString(), ""));
                    contentsOfBagAndCount.Add(ruleBag, ruleCount);
                }
                catch (Exception ex) { //fuck off empty bag
                }
            }
            return contentsOfBagAndCount;
        }
        
        private string cleanBagRule(string bagRule)
        {
            return bagRule.Trim().Replace(".", "").Replace("bags", "bag");
        }

    }
}
