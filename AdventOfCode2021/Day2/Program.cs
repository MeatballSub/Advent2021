using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Day2
{
    public class Instruction
    {
        public string type;
        public int amount;

        public Instruction(string line)
        {
            string [] components = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            type = components[0];
            amount = int.Parse(components[1]);
        }
    }
    public class Program
    {
        private static List<Instruction> readInput(string file_name)
        {
            List<Instruction> instructions = new List<Instruction>();
            using (TextReader reader = File.OpenText(file_name))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    instructions.Add(new Instruction(line));
                }
            }
            return instructions;
        }

        private static Point execute(Point p, Instruction instruction)
        {
            Point new_point = p;
            switch(instruction.type)
            {
                case "forward":
                    new_point.X += instruction.amount;
                    break;
                case "down":
                    new_point.Y += instruction.amount;
                    break;
                case "up":
                    new_point.Y -= instruction.amount;
                    break;
            }
            return new_point;
        }

        private static (Point, int) execute(Point p, int aim, Instruction instruction)
        {
            Point new_point = p;
            switch (instruction.type)
            {
                case "forward":
                    new_point.X += instruction.amount;
                    new_point.Y += aim * instruction.amount;
                    break;
                case "down":
                    aim += instruction.amount;
                    break;
                case "up":
                    aim -= instruction.amount;
                    break;
            }
            return (new_point, aim);
        }

        public static int part1(string file_name)
        {
            Point position = new Point(0, 0);
            int result = 0;
            List<Instruction> instructions = readInput(file_name);
            foreach(Instruction instruction in instructions)
            {
                position = execute(position, instruction);
            }
            Console.WriteLine("Position: " + position);
            result = position.X * position.Y;
            Console.WriteLine("Result: " + result);
            return result;
        }

        public static int part2(string file_name)
        {
            Point position = new Point(0, 0);
            int aim = 0;
            int result = 0;
            List<Instruction> instructions = readInput(file_name);
            foreach (Instruction instruction in instructions)
            {
                (position, aim) = execute(position, aim, instruction);
            }
            Console.WriteLine("Position: {0}, Aim: {1}", position, aim);
            result = position.X * position.Y;
            Console.WriteLine("Result: " + result);
            return result;
        }

        private static void Main(string[] args)
        {
            part1("sample_input.txt");
        }
    }
}
