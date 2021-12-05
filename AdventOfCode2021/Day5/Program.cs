using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day5
{
    public class LineSegment
    {
        public Point point1;
        public Point point2;

        public LineSegment(string line)
        {
            Match match = Regex.Match(line, @"^(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)$");
            point1 = new Point(int.Parse(match.Groups["x1"].Value), int.Parse(match.Groups["y1"].Value));
            point2 = new Point(int.Parse(match.Groups["x2"].Value), int.Parse(match.Groups["y2"].Value));
        }

        private int offsetDirection(int value1, int value2)
        {
            int offset = 0;
            if (value1 > value2)
            {
                offset = -1;
            }
            else if (value1 < value2)
            {
                offset = 1;
            }
            return offset;
        }

        private Point getOffset()
        {
            return new Point(offsetDirection(point1.X, point2.X), offsetDirection(point1.Y, point2.Y));
        }

        public bool isHorizontalOrVertical()
        {
            return ((point1.X == point2.X) || (point1.Y == point2.Y));
        }

        public List<Point> PointsCovered()
        {
            List<Point> points_covered = new List<Point>();
            Point offset = getOffset();

            for(Point point = point1; point != point2; point.Offset(offset))
            {
                points_covered.Add(point);
            }
            points_covered.Add(point2);

            return points_covered;
        }
    }
    public class Program
    {
        static List<LineSegment> readInput(string file_name)
        {
            List<LineSegment> line_segments = new List<LineSegment>();
            using(TextReader reader = File.OpenText(file_name))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    line_segments.Add(new LineSegment(line));
                }
            }
            return line_segments;
        }

        static Dictionary<Point, int> markPoints(Dictionary<Point, int> covered_points, LineSegment line_segment)
        {
            List<Point> points = line_segment.PointsCovered();
            foreach(Point point in points)
            {
                if (!covered_points.ContainsKey(point))
                {
                    covered_points[point] = 0;
                }
                ++covered_points[point];
            }
            return covered_points;
        }

        static Dictionary<Point, int> getCoveredPoints(List<LineSegment> line_segments)
        {
            Dictionary<Point, int> covered_points = new Dictionary<Point, int>();

            foreach(LineSegment line_segment in line_segments)
            {
                covered_points = markPoints(covered_points, line_segment);
            }

            return covered_points;
        }

        public static int part1(string file_name)
        {
            List<LineSegment> line_segments = readInput(file_name).Where(ls => ls.isHorizontalOrVertical()).ToList();
            Dictionary<Point, int> covered_points = getCoveredPoints(line_segments);
            return covered_points.Where(kvp => kvp.Value > 1).Count();
        }

        public static int part2(string file_name)
        {
            List<LineSegment> line_segments = readInput(file_name);
            Dictionary<Point, int> covered_points = getCoveredPoints(line_segments);
            return covered_points.Where(kvp => kvp.Value > 1).Count();
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
