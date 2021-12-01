using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Day1
{
    public class UnitTest1
    {
        private List<int> readDepths(string file_name)
        {
            List<int> depths = new List<int>();
            using(TextReader reader = File.OpenText(file_name))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    depths.Add(int.Parse(line));
                }
            }
            return depths;
        }

        private int countIncreases(string file_name, int distance)
        {
            List<int> depths = readDepths(file_name);
            return depths.Where((item, index) => (index >= distance) && (item > depths[index - distance])).Count();
        }

        [Theory]
        [MemberData(nameof(Day01Part1sample))]
        [MemberData(nameof(Day01Part1))]
        public void Part1(string file_name, int answer)
        {
            int result = countIncreases(file_name, 1);
            Assert.Equal(answer, result);
        }

        [Theory]
        [MemberData(nameof(Day01Part2sample))]
        [MemberData(nameof(Day01Part2))]
        public void Part2(string file_name, int answer)
        {
            int result = countIncreases(file_name, 3);
            Assert.Equal(answer, result);
        }

        public static IEnumerable<object[]> TestSetup(string file_name, int answer)
        {
            return new[]
            {
                new object[] {file_name, answer}
            };
        }

        public static IEnumerable<object[]> Day01Part1sample => TestSetup("sample_input.txt", 7);

        public static IEnumerable<object[]> Day01Part1 => TestSetup("input.txt", 1665);

        public static IEnumerable<object[]> Day01Part2sample => TestSetup("sample_input.txt", 5);

        public static IEnumerable<object[]> Day01Part2 => TestSetup("input.txt", 1702);
    }
}
