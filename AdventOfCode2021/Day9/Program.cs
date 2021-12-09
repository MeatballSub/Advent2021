using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day9
{
    public class Program
    {
        static readonly public Point[] offsets = new Point[] { new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0) };

        static List<string> readInput(string file_name)
        {
            List<string> input = new List<string>();
            using (TextReader reader = File.OpenText(file_name))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    input.Add(line);
                }
            }
            return input;
        }

        static List<Point> getNeighbors(int x, int y)
        {
            return offsets.Select(p => p + new Size(x, y)).ToList();
        }

        static bool boundsCheck(Point neighbor, List<string> array)
        {
            return (neighbor.Y >= 0 && neighbor.Y < array.Count && neighbor.X >= 0 && neighbor.X < array[neighbor.Y].Length);
        }

        static List<Point> findLowPoints(List<string> height_map)
        {
            List<Point> low_points = new List<Point>();

            for (int y = 0; y < height_map.Count; ++y)
            {
                for (int x = 0; x < height_map[y].Length; ++x)
                {
                    bool is_low_point = true;
                    foreach (Point neighbor in getNeighbors(x, y))
                    {
                        if (boundsCheck(neighbor, height_map) && height_map[neighbor.Y][neighbor.X] <= height_map[y][x])
                        {
                            is_low_point = false;
                            break;
                        }
                    }
                    if (is_low_point)
                    {
                        low_points.Add(new Point(x, y));
                    }
                }
            }

            return low_points;
        }

        static HashSet<Point> getBasin(List<string> height_map, Point low_point)
        {            
            HashSet<Point> basin = new HashSet<Point>();
            Queue<Point> frontier = new Queue<Point>();
            frontier.Enqueue(low_point);

            while(frontier.Count > 0)
            {
                Point point = frontier.Dequeue();
                basin.Add(point);
                foreach(Point offset in offsets)
                {
                    Point neighbor = point;
                    neighbor.Offset(offset);
                    if(boundsCheck(neighbor, height_map) && height_map[neighbor.Y][neighbor.X] != '9' && !basin.Contains(neighbor))
                    {
                        frontier.Enqueue(neighbor);
                    }
                }
            }

            return basin;
        }

        static List<HashSet<Point>> getBasins(List<string> height_map, List<Point> low_points)
        {
            return low_points.Select(p => getBasin(height_map, p)).ToList();
        }

        public static long part1(string file_name)
        {
            var height_map = readInput(file_name);
            var low_points = findLowPoints(height_map);

            return low_points.Select(p => 1 + height_map[p.Y][p.X] - '0').Sum();
        }

        public static long part2(string file_name)
        {
            var height_map = readInput(file_name);
            var low_points = findLowPoints(height_map);
            var basins = getBasins(height_map, low_points);

            basins.Sort((a, b) => b.Count.CompareTo(a.Count));

            return basins.Take(3).Aggregate(1, (total, next) => total * next.Count);
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
