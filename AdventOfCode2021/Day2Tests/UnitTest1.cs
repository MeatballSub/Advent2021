using System.Collections.Generic;
using Xunit;

namespace Day2Tests
{
    public class UnitTest1
    {
        //[Theory]
        [MemberData(nameof(Day02Part1sample))]
        [MemberData(nameof(Day02Part1))]
        public void Part1(string file_name, int answer)
        {
            int result = Day2.Program.part1(file_name);
            Assert.Equal(answer, result);
        }

        //[Theory]
        [MemberData(nameof(Day02Part2sample))]
        [MemberData(nameof(Day02Part2))]
        public void Part2(string file_name, int answer)
        {
            int result = Day2.Program.part2(file_name);
            Assert.Equal(answer, result);
        }

        public static IEnumerable<object[]> TestSetup(string file_name, int answer)
        {
            return new[]
            {
                new object[] {file_name, answer}
            };
        }

        public static IEnumerable<object[]> Day02Part1sample => TestSetup("sample_input.txt", 150);

        public static IEnumerable<object[]> Day02Part1 => TestSetup("input.txt", 2215080);

        public static IEnumerable<object[]> Day02Part2sample => TestSetup("sample_input.txt", 900);

        public static IEnumerable<object[]> Day02Part2 => TestSetup("input.txt", 1864715580);
    }
}
