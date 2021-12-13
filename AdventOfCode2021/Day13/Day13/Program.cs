using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day13
{
    public class Line
    {
        public char axis;
        public int value;

        public Line(string s)
        {
            Match match = Regex.Match(s, @"^fold along (?<axis>.)=(?<value>\d+)$");
            axis = match.Groups["axis"].Value[0];
            value = int.Parse(match.Groups["value"].Value);
        }
    }
    public class Program
    {
        static (HashSet<Point>, List<Line>) readInput(string file_name)
        {
            HashSet<Point> points = new HashSet<Point>();
            List<Line> lines = new List<Line>();

            using (TextReader reader = File.OpenText(file_name))
            {
                bool process_lines = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if(line == string.Empty)
                    {
                        process_lines = true;
                    }
                    else if(process_lines)
                    {
                        lines.Add(new Line(line));
                    }
                    else
                    {
                        var coords = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        points.Add(new Point(int.Parse(coords[0]), int.Parse(coords[1])));
                    }
                }
            }
            return (points, lines);
        }

        static int translate(int coord, int line_value)
        {
            if (coord > line_value) coord = 2 * line_value - coord;
            return coord;
        }

        static HashSet<Point> fold(HashSet<Point> points, Line line)
        {
            if(line.axis == 'x')
            {
                return points.Select(p => new Point(translate(p.X, line.value), p.Y)).ToHashSet();
            }
            else
            {
                return points.Select(p => new Point(p.X, translate(p.Y, line.value))).ToHashSet();
            }
        }

        public static long part1(string file_name)
        {
            (var points, var lines) = readInput(file_name);
            var line = lines[0];

            return fold(points, line).Count;
        }

        public static long part2(string file_name)
        {
            (var points, var lines) = readInput(file_name);
            
            foreach(var line in lines)
            {
                points = fold(points, line);
            }

            int max_x = points.Select(p => p.X).Max();
            int max_y = points.Select(p => p.Y).Max();

            for (int y = 0; y <= max_y; ++y)
            {
                for (int x = 0; x <= max_x; ++x)
                {
                    Console.Write(points.Contains(new Point(x, y)) ? '#' : ' ');
                }
                Console.WriteLine();
            }

            return 0;
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
