using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PuzzleSolutions.Year2020
{
    public class Dec04 : Solution
    {
        private List<string> RequiredFields = new List<string> {
        "byr",
        "iyr",
        "eyr",
        "hgt",
        "hcl",
        "ecl",
        "pid",
        };

        private List<string> OptionalFields = new List<string> {
            "cid"
        };

        public void Go(string[] fileLines)
        {
            Console.Write("Part 1: "); 
            EvaluateLines(fileLines, false);

            Console.Write("Part 2: ");
            EvaluateLines(fileLines, true);

        }


        public void EvaluateLines(string[] fileLines, bool advancedValidation)
        {
            int passports = 0;
            var kvpDict = new Dictionary<string, string>();
            
            foreach (var line in fileLines)
            {
                bool endPassport = line.Length == 0;
                
                if(!endPassport)
                {
                    var kvps = line.Split(' ');

                    foreach (var kvpString in kvps)
                    {
                        var kvp = kvpString.Split(':');
                        if (!kvpDict.Keys.Contains(kvp[0]))
                            kvpDict.Add(kvp[0], kvp[1]);
                    }

                }
                else
                {
                    if (ValidateRules(kvpDict, advancedValidation))
                    {
                        passports++;
                    }
                    kvpDict = new Dictionary<string, string>();

                }

            }
            if (ValidateRules(kvpDict, advancedValidation))
            {
                passports++;
            }

            Console.WriteLine($"Valid Passports {passports}");
        }

        bool ValidateRules(Dictionary<string, string> passportValues, bool advancedValidation)
        {
            if (!RequiredFields.All(r => passportValues.Keys.Contains(r))) return false;
            if (!advancedValidation) return true;

            if (!int.TryParse(passportValues["byr"], out int byr) || byr < 1920 || byr > 2002)
                return false;
            if (!int.TryParse(passportValues["iyr"], out int iyr) || iyr < 2010 || iyr > 2020)
                return false;
            if (!int.TryParse(passportValues["eyr"], out int eyr) || eyr < 2020 || eyr > 2030)
                return false;

            var height = passportValues["hgt"];
            var heightUnits = height.Substring(height.Length - 2);
            bool heightUnitsValid = heightUnits == "in" || heightUnits == "cm";
            bool heightInFreedomUnits = heightUnits == "in";
            var heightIsNum = int.TryParse(height.Remove(height.Length - 2), out int heightVal);
            if (!heightIsNum || ! heightUnitsValid || (heightInFreedomUnits && (heightVal < 59 || heightVal > 76)) || (!heightInFreedomUnits && (heightVal < 150 || heightVal > 193)))
                return false;

            if (passportValues["hcl"].Length != 7 || !Regex.IsMatch(passportValues["hcl"], "^#(?:[0-9a-fA-F]{3}){1,2}$"))
                return false;

            var eyeColors = new List<string>()
            {
                "amb",
                "blu",
                "brn",
                "gry",
                "grn",
                "hzl",
                "oth"
            };
            if (!eyeColors.Contains(passportValues["ecl"]))
                return false;

            if (passportValues["pid"].Length != 9 || !long.TryParse(passportValues["pid"], out long pid))
                return false;

            return true;
        }
    }
}
