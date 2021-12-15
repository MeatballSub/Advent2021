using System.Collections.Generic;
using Xunit;

namespace Day14Test
{
    public class UnitTest1
    {
        //[Theory]
        [MemberData(nameof(Part1sample))]
        [MemberData(nameof(Part1))]
        public void Part1Test(string file_name, int answer)
        {
            long result = Day14.Program.part1(file_name);
            Assert.Equal(answer, result);
        }

        //[Theory]
        [MemberData(nameof(Part2sample))]
        [MemberData(nameof(Part2))]
        public void Part2Test(string file_name, long answer)
        {
            long result = Day14.Program.part2(file_name);
            Assert.Equal(answer, result);
        }

        public static IEnumerable<object[]> TestSetup(string file_name, long answer)
        {
            return new[]
            {
                new object[] {file_name, answer}
            };
        }

        public static IEnumerable<object[]> Part1sample => TestSetup("sample_input.txt", 1588);

        public static IEnumerable<object[]> Part1 => TestSetup("input.txt", 3213);

        public static IEnumerable<object[]> Part2sample => TestSetup("sample_input.txt", 2188189693529);

        public static IEnumerable<object[]> Part2 => TestSetup("input.txt", 3711743744429);
    }
}