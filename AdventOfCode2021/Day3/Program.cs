using System;
using System.Collections.Generic;
using System.IO;

namespace Day3
{
    public class Program
    {
        private static List<string> readInput(string file_name)
        {
            List<string> values = new List<string>();
            using (TextReader reader = File.OpenText(file_name))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    values.Add(line);
                }
            }
            return values;
        }

        private static (int, int) getCounts(List<string> values, int index)
        {
            int zero_count = 0;
            int one_count = 0;
            foreach (string s in values)
            {
                if (s[index] == '0')
                {
                    ++zero_count;
                }
                else
                {
                    ++one_count;
                }
            }
            return (zero_count, one_count);
        }

        public static int part1(string file_name)
        {
            int result = 0;
            List<string> input = readInput(file_name);
            int length = input[0].Length;

            uint gamma_rate = 0;
            uint epsilon_rate = 0;
            for(int i = 0; i < length; ++i)
            {                
                gamma_rate <<= 1;
                epsilon_rate <<= 1;
                (int zero_count, int one_count) = getCounts(input, i);
                if (one_count > zero_count)
                {
                    gamma_rate += 1;
                }
                else
                {
                    epsilon_rate += 1;
                }
            }

            Console.WriteLine("Gamma: " + gamma_rate);
            Console.WriteLine("Epsilon: " + epsilon_rate);

            return (int)(gamma_rate * epsilon_rate);
        }

        private static List<string> removeNonMatch(List<string> possibilities, int i, char required)
        {
            List<string> filtered = new List<string>(possibilities);
            foreach (string s in possibilities)
            {
                if (s[i] != required)
                {
                    filtered.Remove(s);
                }
            }
            return filtered;
        }

        private static int getRating(List<string> input, string type)
        {
            List<string> possibilities = new List<string>(input);
            for (int i = 0; i < input[0].Length; ++i)
            {
                if (possibilities.Count > 1)
                {
                    (int zero_count, int one_count) = getCounts(possibilities, i);
                    if ((one_count > zero_count) || (zero_count == one_count))
                    {
                        possibilities = removeNonMatch(possibilities, i, (type == "oxygen") ? '1' : '0');
                    }
                    else // (zero_count > one_count)
                    {
                        possibilities = removeNonMatch(possibilities, i, (type == "oxygen") ? '0' : '1');
                    }
                }
            }
            return Convert.ToInt32(possibilities[0], 2);
        }

        private static int getOxygen(List<string> input)
        {
            return getRating(input, "oxygen");
        }

        private static int getCO2(List<string> input)
        {
            return getRating(input, "co2");
        }

        public static int part2(string file_name)
        {
            int result = 0;
            List<string> input = readInput(file_name);
            int oxygen_rating = getOxygen(input);
            int co2_rating = getCO2(input);

            Console.WriteLine("Oxygen: " + oxygen_rating);
            Console.WriteLine("CO2: " + co2_rating);

            return (oxygen_rating * co2_rating);
        }

        static void Main(string[] args)
        {
            part2("sample_input.txt");
        }
    }
}
