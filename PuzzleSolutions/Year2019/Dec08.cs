using PuzzleSolutions.Year2019.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PuzzleSolutions.Year2019
{
    public class Dec08 : Solution
    {



        public void Go(string[] fileLines)
        {
            solve(fileLines[0], 25, 6);
        }

        private void solve(string fileLine, int w, int h)
        {
            int currentPixelIndex = 0;
            List<ImageLayer> layers = new List<ImageLayer>();
            int? minimumZeroCount = null;
            int? minimumLayerIndex = 0;

            while (currentPixelIndex < fileLine.Length)
            {
                var layer = fileLine.Substring(currentPixelIndex, w * h);
                currentPixelIndex += w * h;
                var ct = layer.Count(x => x == '0');
                if(minimumZeroCount == null || ct < minimumZeroCount)
                {
                    minimumZeroCount = ct;
                    minimumLayerIndex = (currentPixelIndex / (w * h) )-1;
                }
                layers.Add(new ImageLayer(w, h, layer));
            }

            string minimum0Layer = fileLine.Substring(minimumLayerIndex.Value * w * h, w*h);
            Console.WriteLine($"Part 1 Solution {minimum0Layer.Count(x => x == '1') * minimum0Layer.Count(x => x == '2')}");

            int[,] finalImage = new int[w, h];
            layers.Reverse();
            foreach (var imgLayer in layers)
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int pix = imgLayer.GetPixel(x, y);
                        if (pix < 2) //transparent = ignore
                        {
                            finalImage[x, y] = pix;
                        }
                    }
                }
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int color = finalImage[x, y];
                    if (color == 1)
                        Console.ForegroundColor = ConsoleColor.White;
                    else if (color == 0)
                        Console.ForegroundColor = ConsoleColor.Blue;

                    Console.Write("O");
                   
                }
                Console.WriteLine();
            }
        }
    }

    public class ImageLayer
    {
        int width = 0;
        int height = 0;
        int[,] layer;
        public ImageLayer(int width, int height, string layer)
        {
            this.width = width;
            this.height = height;
            this.layer = new int[width, height];
            
            for (int y = 0; y < height; y++)
            {
                var line = layer.Substring(y * width, width);
                for (int x = 0; x < width; x++)
                {
                    this.layer[x, y] = (int)char.GetNumericValue(line[x]);
                }
            }
        }

        public int GetPixel(int x, int y)
        {
            return layer[x, y];
        }
    }
}