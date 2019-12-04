using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec01 : Solution
    {
        public void Go(string[] fileLines)
        {
            int moduleFuel = 0;
            int moduleFuelInclusive = 0;
            foreach (int i in parse(fileLines))
            {
                moduleFuel += findFuelForSimpleMass(i);
                moduleFuelInclusive += findFuelForModuleAndFuelMasses(i);
            }
            Console.WriteLine("Part 1 Solution, necessary fuel: " + moduleFuel);
            Console.WriteLine("Part 2 Solution, necessary fuel, accounting for the weight of fuel and the power of wishes: " + moduleFuelInclusive);
        }

        private List<int> parse(string[] fileLines)
        {
            return fileLines.ToList().ConvertAll(line => Int32.Parse(line)); //input validation is for suckers
        }

        private int findFuelForSimpleMass(int mass)
        {
            return mass / 3 - 2;
        }

        private int findFuelForModuleAndFuelMasses(int mass)
        {
            int baseFuel = findFuelForSimpleMass(mass); //just the module, simple
            int fuelMass = 0;
            int differentialFuelMass = findFuelForSimpleMass(baseFuel);
            while(differentialFuelMass > 0)
            {
                fuelMass += differentialFuelMass;
                differentialFuelMass = findFuelForSimpleMass(differentialFuelMass);
            }
            return fuelMass + baseFuel;
        }
    }
}
