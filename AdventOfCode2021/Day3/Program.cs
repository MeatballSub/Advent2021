using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            int zero_count = values.Where(s => s[index] == '0').Count();
            int one_count = values.Count - zero_count;
            return (zero_count, one_count);
        }

        public static int part1(string file_name)
        {
            List<string> input = readInput(file_name);
            int length = input[0].Length;

            int gamma_rate = 0;
            int epsilon_rate = 0;
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

            return (gamma_rate * epsilon_rate);
        }

        private static char getFilterChar(List<string> input, string type, int index)
        {
            char filter_char = '0';

            (int zero_count, int one_count) = getCounts(input, index);
            if (zero_count > one_count)
            {
                filter_char = (type == "oxygen") ? '0' : '1';
            }
            else
            {
                filter_char = (type == "oxygen") ? '1' : '0';
            }

            return filter_char;
        }

        private static int getRating(List<string> input, string type)
        {
            List<string> possibilities = new List<string>(input);
            for (int i = 0; i < input[0].Length && possibilities.Count > 1; ++i)
            {
                char filter_char = getFilterChar(possibilities, type, i);
                possibilities = possibilities.Where(s => s[i] == filter_char).ToList();
            }
            return Convert.ToInt32(possibilities[0], 2);
        }

        public static int part2(string file_name)
        {
            int result = 0;
            List<string> input = readInput(file_name);
            int oxygen_rating = getRating(input, "oxygen");
            int co2_rating = getRating(input, "co2");

            Console.WriteLine("Oxygen: " + oxygen_rating);
            Console.WriteLine("CO2: " + co2_rating);

            return (oxygen_rating * co2_rating);
        }

        static void Main(string[] args)
        {
            part1("sample_input.txt");
            part1("input.txt");
            part2("sample_input.txt");
            part2("input.txt");
        }
    }
}
