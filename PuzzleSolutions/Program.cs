using PuzzleSolutions.Year2019;
using System;
using System.Collections.Generic;
using System.IO;

namespace PuzzleSolutions
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(Console.LargestWindowWidth - (int)(Console.LargestWindowWidth * .20), Console.LargestWindowHeight - (int)(Console.LargestWindowHeight * .20));
            string userIn = "";
            while (userIn.ToUpper() != "EXIT")
            {
                printTree();
                int puzzleDay = getPuzzleDay();
                int puzzleYear = getPuzzleYear();
                string solutionIdString = $"Dec{puzzleDay.ToString().PadLeft(2, '0')}";

                try
                {
                    // ACTUAL OPERATIONAL LOGIC THAT ISN'T JUST CUTE CONSOLE FUN TO AMUSE MYSELF
                    Solution solution = (Solution)Activator.CreateInstance(Type.GetType($"PuzzleSolutions.Year{puzzleYear}.{solutionIdString}"));
                    var inputLines = getInput(puzzleYear, solutionIdString);
                    solution.Go(inputLines);
                    // BACK TO CONSOLE FUN :)
                }
                catch (Exception ex)
                {

                    if (Type.GetType($"PuzzleSolutions.Year{puzzleYear}.{solutionIdString}") == null)
                    {
                        Console.WriteLine($"Oh no, I haven't implemented PuzzleSolutions.Year{puzzleYear}.{solutionIdString} yet!");
                    }
                    else
                    {
                        Console.WriteLine("You got some bad input or something, chump - " + ex.ToString());
                    }

                }
                // BACK TO CONSOLE FUN :)

                Console.WriteLine("Done! Hit Enter to run again, type exit to quit.");
                userIn = Console.ReadLine();
            }
        }

        static string[] getInput(int puzzleYear, string solutionIdString)
        {
            Console.WriteLine();
            Console.WriteLine("If you'd like to paste input rather than reading from file, paste the lines here and then hit Enter, otherwise just hit Enter to skip.");
            Console.Write("> ");
            var pastedLines = new List<string>();
            string lastLine;
            do {
                lastLine = Console.ReadLine();
                if (lastLine?.Length > 0)
                {
                    pastedLines.Add(lastLine);
                }
            }
            while (lastLine != null && lastLine.Length > 0);
            if(pastedLines?.Count > 0)
            {
                return pastedLines.ToArray();
            }
            return File.ReadAllLines($"Year{puzzleYear}/inputs/{solutionIdString}-input.txt"); //standard case, but during problem solving I'm pasting in all the little samples.
        }

        /// <summary>
        /// Stupid thing I wrote to amuse myself. Ignore until you've run the code.
        /// </summary>
        static void printTree()
        {
            string message;

            if (DateTime.Now.Month != 12 || DateTime.Now.Day > 25)
            {
                message = "Why are you running this when its not Christmastime?";
            }
            else
            {
                message = DateTime.Now.Day == 25 ? "MAXIMUM CHRISTMAS" : $"Merry December {DateTime.Now.Day}!";
                Random rand = new Random();

                int treeHeight = DateTime.Now.Day == 25 ? 40 : Math.Max((int)(DateTime.Now.Day * rand.NextDouble()), 5);
                int bottomWidth = treeHeight * 2;

                for (int level = 0; level < treeHeight; level++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                    int levelWidth = (2 * level) + 1;
                    int needlesAndBaubles = levelWidth - 2;

                    Console.Write(new string(' ', (bottomWidth - levelWidth) / 2));

                    if (levelWidth == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("*");
                        continue;
                    }

                    Console.Write("/");

                    for (int nB = 0; nB < needlesAndBaubles; nB++)
                    {
                        int random = rand.Next(12);
                        switch (random)
                        {
                            case 1: Console.ForegroundColor = ConsoleColor.Green; break;
                            case 2: Console.ForegroundColor = ConsoleColor.Red; break;
                            case 3: Console.ForegroundColor = ConsoleColor.Magenta; break;
                            case 4: Console.ForegroundColor = ConsoleColor.Blue; break;
                            case 5: Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                            case 6: Console.ForegroundColor = ConsoleColor.Yellow; break;
                            case 7: Console.ForegroundColor = ConsoleColor.White; break;
                            default: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                        }
                        Console.Write('*');
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.WriteLine("\\");
            }
            }
            Console.WriteLine(message);
        }

        /// <summary>
        /// Requests user input so that all my solutions can run from one program.
        /// </summary>
        static int getPuzzleDay()
        {
            Console.WriteLine($"Any specific day? Type a number (1-25) to choose a solution, or just hit Enter to play {(DateTime.Now.Day > 25 ? "a random puzzle" : "today's puzzle")}");
            Console.Write("> ");
            string userIn = Console.ReadLine();
            int puzzleDay = -1;
            bool isNumber = Int32.TryParse(userIn, out puzzleDay);
            if (!isNumber || puzzleDay > 25 || puzzleDay < 1)
            {
                puzzleDay = new Random().Next(1, 26);
                if (userIn.Length > 0) Console.Write($"Jerk. My favorite day is December {userIn}th. ");
                else puzzleDay = DateTime.Now.Day;
            }
            Console.WriteLine($"Playing solution to December {puzzleDay}.");
            return puzzleDay;
        }

        /// <summary>
        /// In future, will request user input so I can use the same repo for all future AoC's
        /// </summary>
        /// <returns></returns>
        static int getPuzzleYear()
        {
            return DateTime.Now.Year;
        }
    }

    public interface Solution
    {
        void Go(string[] fileLines);
    }
}
