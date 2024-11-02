using System;

namespace SudokuSolver
{
    class Program
    {
        static int[,] grid = new int[9, 9];
        static Random random = new Random();

        static void Main(string[] args)
        {
            // Generate a complete Sudoku board
            GenerateSudoku();

            // Remove some numbers to create an incomplete puzzle
            CreatePuzzle(40); // You can adjust the number of removed cells

            // Display the incomplete puzzle
            Console.WriteLine("Generated Sudoku Puzzle:");
            DisplayGrid(grid);

            // Solve the Sudoku puzzle
            if (SolveSudoku())
            {
                Console.WriteLine("Solved Sudoku:");
                DisplayGrid(grid);
            }
            else
            {
                Console.WriteLine("The puzzle cannot be solved.");
            }

            Console.ReadKey();
        }

        static void GenerateSudoku()
        {
            FillGrid();
            // You might want to implement a method to shuffle rows/columns to get different puzzles.
        }

        static bool FillGrid()
        {
            // A simple backtracking algorithm to fill the grid
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsValidMove(row, col, num))
                            {
                                grid[row, col] = num;
                                if (FillGrid()) // Recursively fill the grid
                                {
                                    return true;
                                }
                                grid[row, col] = 0; // Reset if no number fits
                            }
                        }
                        return false; // Backtrack
                    }
                }
            }
            return true; // Grid is filled
        }

        static void CreatePuzzle(int emptyCells)
        {
            while (emptyCells > 0)
            {
                int row = random.Next(9);
                int col = random.Next(9);
                if (grid[row, col] != 0) // Only remove filled cells
                {
                    grid[row, col] = 0; // Remove the cell
                    emptyCells--;
                }
            }
        }

        static void DisplayGrid(int[,] grid)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(grid[i, j] + " ");
                    if (j % 3 == 2 && j < 8) Console.Write("| "); // Vertical separator
                }
                Console.WriteLine();
                if (i % 3 == 2 && i < 8) Console.WriteLine("------+-------+------"); // Horizontal separator
            }
        }

        static bool SolveSudoku()
        {
            // Find the next empty cell
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        // Try all possible numbers for the empty cell
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsValidMove(row, col, num))
                            {
                                grid[row, col] = num; // Assign the number

                                if (SolveSudoku()) // Recursively solve the rest of the puzzle
                                {
                                    return true;
                                }

                                grid[row, col] = 0; // Reset the cell
                            }
                        }
                        return false; // No valid number found
                    }
                }
            }
            return true; // If there are no empty cells, the puzzle is solved
        }

        static bool IsValidMove(int row, int col, int num)
        {
            // Check row and column
            for (int i = 0; i < 9; i++)
            {
                if (grid[row, i] == num || grid[i, col] == num)
                {
                    return false;
                }
            }

            // Check 3x3 box
            int boxRow = row / 3 * 3;
            int boxCol = col / 3 * 3;
            for (int i = boxRow; i < boxRow + 3; i++)
            {
                for (int j = boxCol; j < boxCol + 3; j++)
                {
                    if (grid[i, j] == num)
                    {
                        return false;
                    }
                }
            }

            return true; // The number is valid
        }
    }
}
