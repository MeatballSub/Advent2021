using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day4
{
    public class Board
    {
        public int [,] spaces;
        public bool [,] marked;

        public Board()
        {
            spaces = new int[5, 5];
            marked = new bool[5, 5];
        }

        private bool addMark(int value)
        {
            bool added_mark = false;

            for (int row = 0; row < 5; ++row)
            {
                for (int col = 0; col < 5; ++col)
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
            bool winner = false;

            int[] row_counts = new int[5];
            int[] col_counts = new int[5];

            for (int row = 0; !winner && row < 5; ++row)
            {
                for (int col = 0; !winner && col < 5; ++col)
                {
                    if(marked[row,col])
                    {
                        ++row_counts[row];
                        ++col_counts[col];
                        if((row_counts[row] > 4) || (col_counts[col] > 4))
                        {
                            winner = true;
                        }
                    }
                }
            }

            return winner;
        }

        public bool mark(int value)
        {
            return addMark(value) && isWinner();
        }

        public Board(TextReader reader):this()
        {
            string line;
            for(int row=0;row<5; ++row)
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

            for(int row = 0; row < 5; ++row)
            {
                for(int col = 0; col < 5; ++col)
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

            Board winner = new Board();
            int final_draw = -1;
            foreach(int draw in draws)
            {
                foreach(Board board in boards)
                {
                    if(board.mark(draw))
                    {
                        final_draw = draw;
                        winner = board;
                        break;
                    }
                }
                if(final_draw != -1)
                {
                    break;
                }
            }
            
            return winner.score(final_draw);
        }

        public static int part2(string file_name)
        {
            (List<int> draws, List<Board> boards) = readInput(file_name);

            int final_draw = -1;
            Board loser = new Board();
            foreach (int draw in draws)
            {
                List<Board> winners = new List<Board>();

                foreach (Board board in boards)
                {
                    if (board.mark(draw))
                    {
                        winners.Add(board);
                    }
                }

                foreach(Board board in winners)
                {
                    boards.Remove(board);
                }

                if (boards.Count < 1)
                {
                    final_draw = draw;
                    break;
                }
                else if (boards.Count < 2)
                {
                    loser = boards[0];
                }
            }

            return loser.score(final_draw);
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
