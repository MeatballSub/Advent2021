using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day4
{
    public class Board
    {
        const int BOARD_SIZE = 5;
        public int [,] spaces;
        public bool [,] marked;
        public bool won;

        public Board()
        {
            spaces = new int[BOARD_SIZE, BOARD_SIZE];
            marked = new bool[BOARD_SIZE, BOARD_SIZE];
            won = false;
        }

        private bool addMark(int value)
        {
            bool added_mark = false;

            for (int row = 0; row < BOARD_SIZE; ++row)
            {
                for (int col = 0; col < BOARD_SIZE; ++col)
                {
                    if (spaces[row, col] == value)
                    {
                        marked[row, col] = true;
                        added_mark = true;
                    }
                }
            }

            return added_mark;
        }

        private bool isWinner()
        {
            int[] row_counts = new int[BOARD_SIZE];
            int[] col_counts = new int[BOARD_SIZE];

            for (int row = 0; !won && row < BOARD_SIZE; ++row)
            {
                for (int col = 0; !won && col < BOARD_SIZE; ++col)
                {
                    if(marked[row,col])
                    {
                        ++row_counts[row];
                        ++col_counts[col];
                        if((row_counts[row] >= BOARD_SIZE) || (col_counts[col] >= BOARD_SIZE))
                        {
                            won = true;
                        }
                    }
                }
            }

            return won;
        }

        public bool mark(int value)
        {
            return addMark(value) && isWinner();
        }

        public Board(TextReader reader):this()
        {
            string line;
            for(int row=0;row< BOARD_SIZE; ++row)
            {
                line = reader.ReadLine();
                var row_numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int col = 0;
                foreach(string number in row_numbers)
                {
                    spaces[row, col] = int.Parse(number);
                    marked[row, col] = false;
                    ++col;
                }
            }
        }

        public int score(int final_draw)
        {
            int sum = 0;

            for(int row = 0; row < BOARD_SIZE; ++row)
            {
                for(int col = 0; col < BOARD_SIZE; ++col)
                {
                    if(!marked[row,col])
                    {
                        sum += spaces[row, col];
                    }
                }
            }

            return (sum * final_draw);
        }
    }
    public class Program
    {
        private static (List<int>, List<Board>) readInput(string file_name)
        {
            List<int> draws = new List<int>();
            List<Board> boards = new List<Board>();

            using (TextReader reader = File.OpenText(file_name))
            {
                string line;
                line = reader.ReadLine();
                var draws_strings = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach(string draw_string in draws_strings)
                {
                    draws.Add(int.Parse(draw_string));
                }

                while ((line = reader.ReadLine()) != null)
                {
                    boards.Add(new Board(reader));
                }
            }
            return (draws, boards);
        }

        public static int part1(string file_name)
        {
            (List<int> draws, List<Board> boards) = readInput(file_name);

            Board winner = null;

            int draw_index = 0;
            for (; (winner == null) && (draw_index < draws.Count); ++draw_index)
            {
                foreach (Board board in boards)
                {
                    if(board.mark(draws[draw_index]))
                    {
                        winner = board;
                        break;
                    }
                }
            }
            
            return winner.score(draws[draw_index - 1]);
        }

        public static int part2(string file_name)
        {
            (List<int> draws, List<Board> boards) = readInput(file_name);

            Board loser = null;

            int draw_index = 0;
            for (; (boards.Count > 0) && (draw_index < draws.Count); ++draw_index)
            {
                foreach (Board board in boards)
                {
                    board.mark(draws[draw_index]);
                }

                boards = boards.Where(b => !b.won).ToList();

                if (boards.Count == 1)
                {
                    loser = boards[0];
                }
            }

            return loser.score(draws[draw_index - 1]);
        }
        static void Main(string[] args)
        {
            //Console.WriteLine(part1("sample_input.txt"));
            //Console.WriteLine(part1("input.txt"));
            Console.WriteLine(part2("sample_input.txt"));
            //Console.WriteLine(part2("input.txt"));
        }
    }
}
