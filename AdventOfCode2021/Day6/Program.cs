using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    public class Program
    {
        const int MAX_DAY = 9;
        const int RESET_DAY = 7;

        static long[] readInput(string file_name)
        {
            long[] lantern_fish = new long[MAX_DAY + 1];

            using (TextReader reader = File.OpenText(file_name))
            {
                string line = reader.ReadLine();
                var lantern_fish_strings = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach(string lantern_fish_string in lantern_fish_strings)
                {
                    ++lantern_fish[int.Parse(lantern_fish_string)];
                }
            }
            return lantern_fish;
        }

        static long[] spawnNewFish(long[] lantern_fish)
        {
            long new_fish = lantern_fish[0];
            lantern_fish[0] -= new_fish;
            lantern_fish[RESET_DAY] += new_fish;
            lantern_fish[MAX_DAY] = new_fish;

            return lantern_fish;
        }

        static long[] decrementFish(long[] lantern_fish)
        {
            for (int i = 0; i < MAX_DAY; ++i)
            {
                lantern_fish[i] = lantern_fish[i + 1];
            }

            lantern_fish[MAX_DAY] = 0;

            return lantern_fish;
        }

        static long[] nextDay(long[] lantern_fish)
        {
            return decrementFish(spawnNewFish(lantern_fish));
        }

        static long simulate(string file_name, int num_days)
        {
            long[] lantern_fish = readInput(file_name);

            for (int day = 0; day < num_days; ++day)
            {
                lantern_fish = nextDay(lantern_fish);
            }

            return lantern_fish.Sum();
        }

        public static long part1(string file_name)
        {
            return simulate(file_name, 80);
        }

        public static long part2(string file_name)
        {
            return simulate(file_name, 256);
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
