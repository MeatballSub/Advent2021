using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day11
{
    public class Program
    {
        static HashSet<Point> flash_set;
        const char FLASH_POINT = (char)('9' + 1);

        static List<char[]> readInput(string file_name)
        {
            return File.ReadAllLines(file_name).Select(l => l.ToCharArray()).ToList();
        }

        static List<char[]> increment(List<char[]> octopi)
        {
            return octopi.Select(arr => arr.Select(i => (char)(i + 1)).ToArray()).ToList();
        }

        static bool boundsCheck(List<char[]> octopi, Point index)
        {
            return (index.Y >= 0 && index.Y < octopi.Count && index.X >= 0 && index.X < octopi[index.Y].Length);
        }

        static List<char[]> flash(List<char[]> octopi, int x, int y)
        {
            Point octopus = new Point(x, y);
            if(flash_set.Add(octopus))
            {
                List<Point> neighbors = new List<Point>() { new Point(-1, -1), new Point(-1, 0), new Point(-1, 1), new Point(0, -1), new Point(0, 1), new Point(1, -1), new Point(1, 0), new Point(1, 1) };
                foreach (Point neighbor in neighbors)
                {
                    neighbor.Offset(octopus);
                    if(boundsCheck(octopi, neighbor))
                    {
                        if(++octopi[neighbor.Y][neighbor.X] == FLASH_POINT)
                        {
                            flash(octopi, neighbor.X, neighbor.Y);
                        }
                    }
                }
            }
            return octopi;
        }

        static List<char[]> flash(List<char[]> octopi)
        {
            for(int y = 0; y < octopi.Count; ++y)
            {
                for (int x = 0; x < octopi[y].Length; ++x)
                {
                    if (octopi[y][x] >= FLASH_POINT)
                    {
                        octopi = flash(octopi, x, y);
                    }
                }
            }
            return octopi;
        }

        static List<char[]> reset(List<char[]> octopi)
        {
            foreach(Point octopus in flash_set)
            {
                octopi[octopus.Y][octopus.X] = '0';
            }
            return octopi;
        }

        static List<char[]> step(List<char[]> octopi)
        {
            flash_set.Clear();
            octopi = increment(octopi);
            octopi = flash(octopi);
            octopi = reset(octopi);
            return octopi;
        }

        public static long part1(string file_name)
        {
            long count = 0;
            flash_set = new HashSet<Point>();

            var octopi = readInput(file_name);            
            for (int i = 0; i < 100; ++i)
            {
                octopi = step(octopi);
                count += flash_set.Count;
            }

            return count;
        }

        public static long part2(string file_name)
        {
            flash_set = new HashSet<Point>();

            var octopi = readInput(file_name);
            int num_octopi = octopi.Sum(s => s.Length);
            int i = 0;
            for (;  flash_set.Count != num_octopi; ++i)
            {
                octopi = step(octopi);
            }

            return i;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(part1("sample_input.txt"));
            Console.WriteLine(part1("input.txt"));
            Console.WriteLine(part2("sample_input.txt"));
            Console.WriteLine(part2("input.txt"));
        }
    }
}
