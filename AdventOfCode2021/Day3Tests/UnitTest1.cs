using System;
using System.Collections.Generic;
using Xunit;

namespace Day3Tests
{
    public class UnitTest1
    {
        [Theory]
        [MemberData(nameof(Part1sample))]
        [MemberData(nameof(Part1))]
        public void Part1Test(string file_name, int answer)
        {
            int result = Day3.Program.part1(file_name);
            Assert.Equal(answer, result);
        }

        [Theory]
        [MemberData(nameof(Part2sample))]
        [MemberData(nameof(Part2))]
        public void Part2Test(string file_name, int answer)
        {
            int result = Day3.Program.part2(file_name);
            Assert.Equal(answer, result);
        }

        public static IEnumerable<object[]> TestSetup(string file_name, int answer)
        {
            return new[]
            {
                new object[] {file_name, answer}
            };
        }

        public static IEnumerable<object[]> Part1sample => TestSetup("sample_input.txt", 198);

        public static IEnumerable<object[]> Part1 => TestSetup("input.txt", 3242606);

        public static IEnumerable<object[]> Part2sample => TestSetup("sample_input.txt", 230);

        public static IEnumerable<object[]> Part2 => TestSetup("input.txt", 4856080);
    }
}
