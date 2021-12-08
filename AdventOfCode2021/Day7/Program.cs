using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    public class Program
    {
        static List<int> readInput(string file_name)
        {
            List<int> crabs = new List<int>();
            using(TextReader reader = File.OpenText(file_name))
            {
                crabs = reader.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(_ => int.Parse(_)).ToList();
            }
            return crabs;
        }

        static long cost2(long crab, long align)
        {
            long n = Math.Abs(crab - align);
            return (n * (n + 1) / 2);
        }

        public static long part1(string file_name)
        {
            List<int> crabs = readInput(file_name);
            crabs.Sort();
            int align = crabs[(crabs.Count + 1) / 2];

            return crabs.Sum(c => Math.Abs(c - align));
        }

        public static long part2(string file_name)
        {
            List<int> crabs = readInput(file_name);
            int round_down = (int)Math.Floor(crabs.Average());
            int round_up = (int)Math.Ceiling(crabs.Average());

            long round_down_cost = (long)crabs.Sum(c => cost2(c, round_down));
            long round_up_cost = (long)crabs.Sum(c => cost2(c, round_up));

            return Math.Min(round_down_cost, round_up_cost);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(part2("sample_input.txt"));
            Console.WriteLine(part2("input.txt"));
        }
    }
}
