using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day15
{
    public static class DictionaryExtension
    {
        public static TValue valueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue default_value)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : default_value;
        }
    }

    public class Program
    {
        static readonly List<Size> neighbors = new List<Size> { new Size(0, -1), new Size(-1, 0), new Size(1, 0), new Size(0, 1) };
        static int MAP_SIZE;

        public static long part1(string file_name)
        {
            var map = readInput(file_name);
            return dijkstraLength(map);
        }

        public static long part2(string file_name)
        {
            var map = readInput(file_name);
            map = expandMap(map);

            return dijkstraLength(map);
        }

        static long dijkstraLength(char[,] map)
        {
            var risk = dijkstra(map);

            Point exit = new Point(MAP_SIZE - 1, MAP_SIZE - 1);

            return risk[exit];
        }

        static Dictionary<Point, long> dijkstra(char[,] map)
        {
            Dictionary<Point, long> risk = new Dictionary<Point, long>();
            PriorityQueue<Point, long> frontier = new PriorityQueue<Point, long>();

            risk.Add(new Point(0, 0), 0);
            frontier.Enqueue(new Point(0, 0), 0);

            Point front;
            long priority;

            while (frontier.TryDequeue(out front, out priority))
            {
                if (priority <= risk.valueOrDefault(front, long.MaxValue))
                {
                    exploreNeighbors(front, risk, frontier, map);
                }
            }

            return risk;
        }

        static void exploreNeighbors(Point front, Dictionary<Point, long> risk, PriorityQueue<Point, long> frontier, char[,] map)
        {
            foreach (var neighbor in getNeighbors(front))
            {
                expandNeighbor(front, neighbor, risk, frontier, map);
            }
        }

        static IEnumerable<Point> getNeighbors(Point p)
        {
            bool boundsCheck(Point p) => p.X >= 0 && p.X < MAP_SIZE && p.Y >= 0 && p.Y < MAP_SIZE;
            return neighbors.Select(n => Point.Add(p, n)).Where(n => boundsCheck(n));
        }

        static void expandNeighbor(Point front, Point neighbor, Dictionary<Point, long> risk, PriorityQueue<Point, long> frontier, char[,] map)
        {
            long alt = risk[front] + map[neighbor.Y, neighbor.X] - '0';
            if (alt < risk.valueOrDefault(neighbor, long.MaxValue))
            {
                frontier.Enqueue(neighbor, alt);
                risk[neighbor] = alt;
            }
        }

        static char[,] expandMap(char [,] map)
        {
            char WRAP_CHAR = (char)('9' + 1);
            int old_map_size = MAP_SIZE;
            MAP_SIZE *= 5;
            char[,] expanded_map = new char[MAP_SIZE, MAP_SIZE];

            for(int y = 0; y < MAP_SIZE; ++y)
            {
                for(int x = 0; x < MAP_SIZE; ++x)
                {
                    if(x < old_map_size && y < old_map_size)
                    {
                        expanded_map[y, x] = map[y, x];
                    }
                    else
                    {
                        char value = (x < old_map_size) ? expanded_map[y - old_map_size, x] : expanded_map[y, x - old_map_size];
                        ++value;
                        expanded_map[y, x] = (value == WRAP_CHAR) ? '1' : value;
                    }
                }
            }

            return expanded_map;
        }

        static char[,] readInput(string file_name)
        {
            var lines = File.ReadAllLines(file_name);
            MAP_SIZE = lines[0].Length;
            var map = new char[MAP_SIZE, MAP_SIZE];
            for (int y = 0; y < MAP_SIZE; ++y)
            {
                for (int x = 0; x < MAP_SIZE; ++x)
                {
                    map[y, x] = lines[y][x];
                }
            }
            return map;
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
