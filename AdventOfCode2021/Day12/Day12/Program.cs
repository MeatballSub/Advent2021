using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    public class Program
    {
        static Dictionary<string, List<string>> readInput(string file_name)
        {
            Dictionary<string, List<string>> input = new Dictionary<string, List<string>>();

            var edges = File.ReadAllLines(file_name).Select(l => l.Split('-', StringSplitOptions.RemoveEmptyEntries));
            foreach(var edge in edges)
            {
                var node1 = edge[0];
                var node2 = edge[1];
                if (!input.ContainsKey(node1)) input[node1] = new List<string>();
                if (!input.ContainsKey(node2)) input[node2] = new List<string>();
                input[node1].Add(node2);
                input[node2].Add(node1);
            }

            return input;
        }

        static long findPaths(string file_name, Func<string, Stack<string>, bool> allowable)
        {
            long count = 0;
            var caves_edges = readInput(file_name);

            Stack<Stack<string>> routes = new Stack<Stack<string>>();
            routes.Push(new Stack<string>());
            routes.Peek().Push("start");

            while (routes.Count > 0)
            {
                Stack<string> curr = routes.Pop();
                if (curr.Peek() == "end")
                {
                    ++count;
                }
                else
                {
                    var next = caves_edges[curr.Peek()].Where(c => allowable(c, curr));
                    foreach (var cave in next)
                    {
                        Stack<string> next_cave = new Stack<string>(curr);
                        next_cave.Push(cave);
                        routes.Push(next_cave);
                    }
                }
            }

            return count;
        }

        public static long part1(string file_name)
        {
            bool allowable(string cave, Stack<string> curr) => (cave != cave.ToLower() || !curr.Contains(cave));
            return findPaths(file_name, allowable);
        }

        public static long part2(string file_name)
        {
            bool allowable(string cave, Stack<string> curr) => (cave != "start" && (cave != cave.ToLower() || !curr.Contains(cave) || noDoubleSmallCave(curr)));
            return findPaths(file_name, allowable);
        }

        static bool noDoubleSmallCave(Stack<string> curr)
        {
            var doubled_small_caves = curr.Where(c => c == c.ToLower()).GroupBy(c => c).Where(g => g.Count() > 1);
            return (doubled_small_caves.Count() == 0);
        }

        static void Main(string[] args)
        {
            Console.WriteLine(part1("sample_input.txt"));
            Console.WriteLine(part1("sample_input2.txt"));
            Console.WriteLine(part1("sample_input3.txt"));
            Console.WriteLine(part1("input.txt"));
            Console.WriteLine(part2("sample_input.txt"));
            Console.WriteLine(part2("sample_input2.txt"));
            Console.WriteLine(part2("sample_input3.txt"));
            Console.WriteLine(part2("input.txt"));
        }
    }
}
