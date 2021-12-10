using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day10
{
    public class Program
    {
        static readonly Dictionary<char, char> matching_pairs = new Dictionary<char, char>() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' } };

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

        static bool isChunkOpener(char c)
        {
            return matching_pairs.ContainsKey(c);
        }

        static bool isMatchingChunkCloser(char opener, char closer)
        {
            return (closer == matching_pairs[opener]);
        }

        static (char, Stack<char>) isCorruptOrIncomplete(string line)
        {
            char corrupting_char = char.MinValue;

            Stack<char> chunks_open = new Stack<char>();
            foreach (char c in line)
            {
                if (isChunkOpener(c)) chunks_open.Push(c);
                else if (isMatchingChunkCloser(chunks_open.Peek(), c)) chunks_open.Pop();
                else
                {
                    corrupting_char = c;
                    chunks_open.Clear();
                    break;
                }
            }

            return (corrupting_char, chunks_open);
        }

        static char getCorruptChar(string line)
        {
            return isCorruptOrIncomplete(line).Item1;
        }

        static Stack<char> getIncompleteStack(string line)
        {
            return isCorruptOrIncomplete(line).Item2;
        }

        public static long part1(string file_name)
        {
            Dictionary<char, long> char_to_points = new Dictionary<char, long> { { char.MinValue, 0 }, { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            var lines = readInput(file_name);
            return lines.Select(l => getCorruptChar(l)).Sum(c => char_to_points[c]);
        }

        public static long part2(string file_name)
        {
            Dictionary<char, long> char_to_points = new Dictionary<char, long> { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 } };

            Func<Stack<char>, long> getScore = s => s.Aggregate(0L, (total, next) => total * 5 + char_to_points[matching_pairs[next]]);

            var lines = readInput(file_name);
            var scores = lines.Select(line => getScore(getIncompleteStack(line))).Where(s => s != 0).ToList();
            scores.Sort();
            return scores[(scores.Count - 1) / 2];
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
