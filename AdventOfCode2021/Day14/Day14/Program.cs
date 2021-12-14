using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day14
{
    public class Program
    {
        static Dictionary<(string, int), Dictionary<char, long>> expansions = new Dictionary<(string, int), Dictionary<char, long>>();
        static Dictionary<string, char> rules = new Dictionary<string, char>();

        static void Main(string[] args)
        {
            Console.WriteLine(part1("sample_input.txt"));
            Console.WriteLine(part1("input.txt"));
            Console.WriteLine(part2("sample_input.txt"));
            Console.WriteLine(part2("input.txt"));
        }

        public static long part1(string file_name)
        {
            return processSteps(file_name, 10);
        }

        public static long part2(string file_name)
        {
            return processSteps(file_name, 40);
        }

        static long processSteps(string file_name, int steps)
        {
            expansions.Clear();

            string start = null;
            (start, rules) = readInput(file_name);

            var pair_char_counts = start.Skip(1).Select((c, i) => getCounts(start.Substring(i, 2), steps)).ToList();

            // NNCB
            // (NN) (NC) (CB)
            //    \ /  \ /
            //     2    2 
            // each of the letters in the middle(not first or last) will be counted an extra time, create element to subtract those out
            var double_counted_chars = start.Skip(1).SkipLast(1).GroupBy(c => c).ToDictionary(g => g.Key, g => (long)-g.Count());

            pair_char_counts.Add(double_counted_chars);
            var total_char_counts = merge(pair_char_counts);

            return getMaxMinDiff(total_char_counts);
        }

        static (string, Dictionary<string, char>) readInput(string file_name)
        {
            var lines = File.ReadAllLines(file_name);
            return (lines[0], lines.Skip(2).Select(l => l.Split(" -> ", StringSplitOptions.RemoveEmptyEntries)).ToDictionary(e => e[0], e => e[1][0]));
        }

        static Dictionary<char, long> merge(Dictionary<char, long> first, Dictionary<char, long> second)
        {
            Dictionary<char, long>[] array = new Dictionary<char, long>[] { first, second };
            return merge(array);
        }

        static Dictionary<char, long> merge(IEnumerable<Dictionary<char, long>> array)
        {
            return array.SelectMany(dict => dict).ToLookup(kvp => kvp.Key, kvp => kvp.Value).ToDictionary(g => g.Key, g => g.Sum());
        }

        static long getMaxMinDiff(Dictionary<char, long> char_counts)
        {
            var counts = char_counts.Select(kvp => kvp.Value);
            return counts.Max() - counts.Min();
        }

        static Dictionary<char, long> getCounts(string s, int depth)
        {
            Dictionary<char, long> counts = null;

            if (expansions.ContainsKey((s, depth)))
            {
                counts = expansions[(s, depth)];
            }
            else
            {
                if (depth == 0)
                {
                    counts = s.GroupBy(c => c).ToDictionary(g => g.Key, g => (long)g.Count());
                }
                else
                {
                    var left_counts = getCounts(leftString(s), depth - 1);
                    var right_counts = getCounts(rightString(s), depth - 1);
                    counts = merge(left_counts, right_counts);

                    // rules[s] double counted in both left and right
                    // NN -> C
                    //   NN
                    //  /  \
                    // NC  CN
                    --counts[rules[s]];
                }
                expansions.Add((s, depth), counts);
            }

            return counts;
        }

        static string leftString(string s)
        {
            return s[0].ToString() + rules[s];
        }

        static string rightString(string s)
        {
            return rules[s].ToString() + s[1];
        }
    }
}
