using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec06 : Solution
    {
        public void Go(string[] fileLines)
        {
            var orbitals = parse(fileLines);
            List<string> orbitalChain = new List<string>();
            int degreesToCenterOfMass = 0;
            foreach(var key in orbitals.Keys)
            {
                degreesToCenterOfMass += findOrbitals(key, orbitals, "COM", ref orbitalChain);
            }
            Console.WriteLine($"There are this many orbits, total: {degreesToCenterOfMass}");

            orbitalChain = new List<string>();
            List<string> orbitalChainSan = new List<string>();

            findOrbitals("YOU", orbitals, "COM", ref orbitalChain);
            findOrbitals("SAN", orbitals, "COM", ref orbitalChainSan);

            var overlap = orbitalChainSan.Intersect(orbitalChain);
            int degreesToCenterOfSan = orbitalChain.IndexOf(overlap.First()) + orbitalChainSan.IndexOf(overlap.First()) ;
            Console.WriteLine($"There are this many orbital transfers to be close to SAN: {degreesToCenterOfSan}");
        }

        private int findOrbitals(string heavenlyBody, Dictionary<string, string> orbitals, string target, ref List<string> orbitalChain, int degreesOfSeparation = 1)
        {
            var orbited = orbitals[heavenlyBody];
            orbitalChain.Add(orbited);
            if(orbited == target)
            {
                return degreesOfSeparation;
            }
            degreesOfSeparation++;
            return findOrbitals(orbited, orbitals, target, ref orbitalChain, degreesOfSeparation);
        }

        private Dictionary<string, string> parse(string[] fileLines)
        {
           var orbitz = new Dictionary<string, string>();
           foreach(var fileLine in fileLines)
            {
                var items = fileLine.Split(")");
                orbitz.Add(items[1], items[0]);
            }
            return orbitz;
        }
    }
}