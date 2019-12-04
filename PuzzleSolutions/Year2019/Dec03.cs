using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec03 : Solution
    {
        public void Go(string[] fileLines)
        {
            List<Line> wires = fileLines.ToList().ConvertAll(l => parse(l));
            Coordinate origin = new Coordinate(0, 0);

            var intersections = findIntersections(wires[0], wires[1]);

            Coordinate closestManhattanDistance = intersections.Where(i => calculateManhattanDistance(i, origin) > 0).OrderBy(coord => calculateManhattanDistance(coord, origin)).ToList()[0];
            int manhattanDistance = calculateManhattanDistance(closestManhattanDistance, origin);

            Console.WriteLine($"Case 1: Closest intersection by Manhattan Distance: {closestManhattanDistance} which is a total distance of {manhattanDistance} from the origin.");

            Coordinate closestByWalkingDistance = intersections.Where(i => calculateManhattanDistance(i, origin) > 0)
                                                                .OrderBy(coord => calculateWalkingDistance(coord, wires[0]) + calculateWalkingDistance(coord, wires[1]))
                                                                .ToList()[0];
            int walkingDistance = calculateWalkingDistance(closestByWalkingDistance, wires[0]) + calculateWalkingDistance(closestByWalkingDistance, wires[1]);

            Console.WriteLine($"Case 2: Closest intersection by step Distance: {closestByWalkingDistance} which is a total distance of {walkingDistance} from the origin.");


            Console.WriteLine();
            //return calculateDistance(closest, origin);
        }

        private List<Coordinate> findIntersections(Line wireA, Line wireB)
        {
            List<Coordinate> coords = new List<Coordinate>();
            foreach (var lineSegmentA in wireA.segments)
            {
                Coordinate endPointA1 = lineSegmentA.coordinates[0];
                Coordinate endPointA2 = lineSegmentA.coordinates.Last();
                bool verticalA = (endPointA1.x - endPointA2.x) == 0;
                double? slopeA = verticalA ? null : (double?)Math.Round(((1.0 * endPointA1.y - endPointA2.y) / (1.0 * endPointA1.x - endPointA2.x)));

                foreach (var lineSegmentB in wireB.segments)
                {
                    var endPointB1 = lineSegmentB.coordinates[0];
                    var endPointB2 = lineSegmentB.coordinates.Last();

                    bool verticalB = (endPointB1.x - endPointB2.x) == 0;
                    //casting because the problem is all ints but I started writing this as if it was going to be used for real.
                    double? slopeB = verticalB ? null : (double?)Math.Round((1.0 * endPointB1.y - endPointB2.y) / (1.0 * endPointB1.x - endPointB2.x));
                    if (slopeA == slopeB)
                    {
                        // swallow this case, because I bet it never happens (meaningfully)
                    }
                    else
                    {
                        double? yAxisIntA = endPointA1.y - slopeA * endPointA1.x;
                        double? yAxisIntB = endPointB1.y - slopeB * endPointB1.x;

                        //nested ternary because I am a monster
                        //if slope is null (vertical line), go with the x coordinate of the null slope line, else use algebra
                        var xIntersect = slopeA == null ? endPointA1.x : slopeB == null ? endPointB1.x : (yAxisIntB - yAxisIntA) / (slopeA - slopeB);

                        //nested ternary because I am a monster
                        //if slope is 0 (horizontal line), go with the y coordinate of the 0 slope line, else use algebra
                        var yIntersect = slopeA == 0 ? endPointA1.y : slopeB == 0 ? endPointB1.y : slopeA * ((yAxisIntB - yAxisIntA) / (slopeA - slopeB)) + yAxisIntA;

                        var intersect = new Coordinate((int)xIntersect, (int)yIntersect);

                        if (pointIsOnLineSegment(intersect, lineSegmentA) && pointIsOnLineSegment(intersect, lineSegmentB))
                        {
                            coords.Add(new Coordinate((int)xIntersect, (int)yIntersect));
                        }
                    }
                }
            }
            return coords;
        }

        private static bool pointIsOnLineSegment(Coordinate point, Line line)
        {
            if (Math.Min(line.coordinates.First().x, line.coordinates.Last().x) <= point.x && Math.Max(line.coordinates.First().x, line.coordinates.Last().x) >= point.x &&
                Math.Min(line.coordinates.First().y, line.coordinates.Last().y) <= point.y && Math.Max(line.coordinates.First().y, line.coordinates.Last().y) >= point.y)
            {
                return true;
            }
            return false;
        }

        private Line parse(string fileLine)
        {
            var instructions = fileLine.Split(',');
            var wire = new Line();

            Coordinate lastCoord = new Coordinate(0, 0);
            wire.coordinates.Add(lastCoord);
            Line currentSegment = new Line();
            currentSegment.coordinates.Add(lastCoord);

            int lastXDelta = 0;
            int lastYDelta = 0;

            foreach (var instruction in instructions)
            {
                char direction = instruction[0];
                int distance = Int32.Parse(instruction.Substring(1));
                int xD = 0;
                int yD = 0;

                switch (direction)
                {
                    case 'U': yD += distance; break;
                    case 'R': xD += distance; break;
                    case 'D': yD -= distance; break;
                    case 'L': xD -= distance; break;
                }

                int newX = lastCoord.x + xD;
                int newY = lastCoord.y + yD;

                Coordinate newCoord = new Coordinate(newX, newY);
                wire.coordinates.Add(newCoord);

                if (lastXDelta == 0 && lastYDelta == 0)
                {
                    currentSegment.coordinates.Add(newCoord);
                }

                if ((xD == 0 && lastXDelta == 0 && lastYDelta != 0 && yD != 0) || (yD == 0 && lastYDelta == 0 && lastXDelta != 0 && xD != 0)) //continuing on a segment
                {
                    currentSegment.coordinates.Add(newCoord);
                }
               
                if ((yD != 0 && lastXDelta != 0 && xD == 0) || (xD != 0 && lastYDelta != 0 && yD == 0)) //turning onto a y Segment
                {
                    wire.segments.Add(currentSegment);
                    currentSegment = new Line();
                    currentSegment.coordinates.Add(lastCoord);
                    currentSegment.coordinates.Add(newCoord);

                }
               
                lastXDelta = xD;
                lastYDelta = yD;
                lastCoord = newCoord;
            }

            if (!wire.segments.Contains(currentSegment))
            {
                wire.segments.Add(currentSegment);
            }

            return wire;
        }

        private int calculateManhattanDistance(Coordinate a, Coordinate b)
        {
            int xDif = a.x - b.x;
            int yDif = a.y - b.y;
            return Math.Abs(xDif) + Math.Abs(yDif);
        }

        private int calculateWalkingDistance(Coordinate a, Line pathTaken)
        {
            int totalSteps = 0;
            foreach (Line segment in pathTaken.segments)
            {
                if (pointIsOnLineSegment(a, segment))
                {
                    totalSteps += calculateManhattanDistance(segment.coordinates.First(), a);
                    break;
                }
                else
                {
                    totalSteps += calculateManhattanDistance(segment.coordinates.First(), segment.coordinates.Last());
                }
            }
            return totalSteps;
        }

        private class Coordinate
        {
            public Coordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public int x;
            public int y;

            public override string ToString()
            {
                return $"({x},{y})";
            }
        }

        private class Line
        {
            public Line() { }

            public Line(List<Coordinate> coordinates)
            {
                this.coordinates = coordinates;
            }

            public List<Coordinate> coordinates = new List<Coordinate>();
            public List<Line> segments = new List<Line>();

            public override string ToString() //makes debug easier :)
            {
                if (segments != null && segments.Count > 0) return "wire";
                return $"({coordinates.First().x},{coordinates.First().y}) --- ({coordinates.Last().x},{coordinates.Last().y})";
            }
        }
    }
}
