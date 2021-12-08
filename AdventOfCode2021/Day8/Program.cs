using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day8
{
    public class Program
    {
        static List<string> readInput(string file_name)
        {
            List<string> input = new List<string>();
            using(TextReader reader = File.OpenText(file_name))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    input.Add(line);
                }
            }
            return input;
        }

        static Dictionary<string, int> getSegmentToNumber()
        {
            Dictionary<string, int> segment_to_number = new Dictionary<string, int>();
            segment_to_number["abcefg"] = 0;
            segment_to_number["cf"] = 1;
            segment_to_number["acdeg"] = 2;
            segment_to_number["acdfg"] = 3;
            segment_to_number["bcdf"] = 4;
            segment_to_number["abdfg"] = 5;
            segment_to_number["abdefg"] = 6;
            segment_to_number["acf"] = 7;
            segment_to_number["abcdefg"] = 8;
            segment_to_number["abcdfg"] = 9;

            return segment_to_number;
        }

        static Dictionary<int, string> getEasyNumberMapping(string numbers)
        {
            Dictionary<int, string> mapping = new Dictionary<int, string>();
            var digits = numbers.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string digit in digits)
            {
                if (digit.Length == 2) mapping.Add(1, digit);
                else if (digit.Length == 3) mapping.Add(7, digit);
                else if (digit.Length == 4) mapping.Add(4, digit);
                else if (digit.Length == 7) mapping.Add(8, digit);
            }

            return mapping;
        }

        static Dictionary<char, int> getSegmentCounts(string numbers)
        {
            Dictionary<char, int> segment_count = new Dictionary<char, int>();

            foreach (char segment in "abcdefg")
            {
                segment_count[segment] = numbers.ToCharArray().Count(c => c == segment);
            }

            return segment_count;
        }

        static Dictionary<char, char> getTranslations(Dictionary<int, string> number_strs, Dictionary<char, int> segment_count)
        {
            Dictionary<char, char> translation = new Dictionary<char, char>();

            foreach ((char c, int count) in segment_count)
            {
                if (count == 4) translation['e'] = c;
                else if (count == 6) translation['b'] = c;
                else if (count == 9) translation['f'] = c;
            }

            translation['c'] = segmentMinus(1, "f", number_strs, translation);
            translation['a'] = segmentMinus(7, "cf", number_strs, translation);
            translation['d'] = segmentMinus(4, "bcf", number_strs, translation);
            translation['g'] = segmentMinus(8, "abcdef", number_strs, translation);

            return translation.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        static Dictionary<char, char> getDecoderRing(string numbers)
        {
            Dictionary<int, string> number_strs = getEasyNumberMapping(numbers);
            Dictionary<char, int> segment_counts = getSegmentCounts(numbers);
            return getTranslations(number_strs, segment_counts);
        }

        static int decode(string value, Dictionary<char, char> translation)
        {
            Dictionary<string, int> segment_to_number = getSegmentToNumber();
            
            var decoded_value = value.Select(c => translation[c]).ToArray();
            Array.Sort(decoded_value);

            return segment_to_number[new string(decoded_value)];
        }

        static char segmentMinus(int value, string subtract, Dictionary<int, string> number_strs, Dictionary<char, char> translation)
        {
            var subtract_translated = subtract.Select(c => translation[c]);
            return number_strs[value].Except(subtract_translated).First();
        }

        static int getDisplayValue(string[] output_values, Dictionary<char, char> translation)
        {
            return output_values.Select(v => decode(v, translation)).Aggregate(0, (total, next) => total * 10 + next);
        }

        public static long part1(string file_name)
        {
            long count = 0;
            var input = readInput(file_name);
            foreach(string str in input)
            {
                Match match = Regex.Match(str, @"^.*?\|(?<output_values>.*?)$");
                var numbers = match.Groups["output_values"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int [] segment_counts = { 0, 0, 1, 1, 1, 0, 0, 1 };
                count += numbers.Sum(s => segment_counts[s.Length]);
            }
            return count;
        }

        public static long part2(string file_name)
        {
            long sum = 0;
            var input = readInput(file_name);
            foreach (string str in input)
            {
                Match match = Regex.Match(str, @"(?<numbers>^.*?)\|(?<output_values>.*?)$");
                Dictionary<char, char> translation = getDecoderRing(match.Groups["numbers"].Value);
                var output_values = match.Groups["output_values"].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                sum += getDisplayValue(output_values, translation);
            }
            return sum;
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
