using System.Collections.Generic;
using Xunit;

namespace Day12Test
{
    public class UnitTest1
    {
        //[Theory]
        [MemberData(nameof(Part1sample))]
        [MemberData(nameof(Part1sample2))]
        [MemberData(nameof(Part1sample3))]
        [MemberData(nameof(Part1))]
        public void Part1Test(string file_name, int answer)
        {
            long result = Day12.Program.part1(file_name);
            Assert.Equal(answer, result);
        }

        //[Theory]
        [MemberData(nameof(Part2sample))]
        [MemberData(nameof(Part2sample2))]
        [MemberData(nameof(Part2sample3))]
        [MemberData(nameof(Part2))]
        public void Part2Test(string file_name, long answer)
        {
            long result = Day12.Program.part2(file_name);
            Assert.Equal(answer, result);
        }

        public static IEnumerable<object[]> TestSetup(string file_name, long answer)
        {
            return new[]
            {
                new object[] {file_name, answer}
            };
        }

        public static IEnumerable<object[]> Part1sample => TestSetup("sample_input.txt", 226);

        public static IEnumerable<object[]> Part1sample2 => TestSetup("sample_input2.txt", 19);

        public static IEnumerable<object[]> Part1sample3 => TestSetup("sample_input3.txt", 10);

        public static IEnumerable<object[]> Part1 => TestSetup("input.txt", 4413);

        public static IEnumerable<object[]> Part2sample => TestSetup("sample_input.txt", 3509);

        public static IEnumerable<object[]> Part2sample2 => TestSetup("sample_input2.txt", 103);

        public static IEnumerable<object[]> Part2sample3 => TestSetup("sample_input3.txt", 36);

        public static IEnumerable<object[]> Part2 => TestSetup("input.txt", 118803);
    }
}