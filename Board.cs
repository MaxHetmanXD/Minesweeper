using Minesweeper;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Minesweeper
{
    public class Board
    {
        public int Rows { get; }
        public int Columns { get; }
        public int MinesCount { get; }

        public ObservableCollection<Cell> Cells { get; }

        public bool IsGameOver { get; private set; }
        public bool IsGameWon { get; private set; } // Флаг победы
        private int _revealedCount; // Счетчик открытых клеток

        public Board(int rows, int columns, int minesCount)
        {
            Rows = rows;
            Columns = columns;
            MinesCount = minesCount;
            Cells = new ObservableCollection<Cell>();

            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Cells.Add(new Cell { Row = r, Column = c });
                }
            }

            PlaceMines();
            CalculateNeighbors();
        }

        private void PlaceMines()
        {
            Random rand = new Random();
            int minesPlaced = 0;

            while (minesPlaced < MinesCount)
            {
                int index = rand.Next(Cells.Count);
                if (!Cells[index].IsMine)
                {
                    Cells[index].IsMine = true;
                    minesPlaced++;
                }
            }
        }

        private void CalculateNeighbors()
        {
            foreach (var cell in Cells)
            {
                if (cell.IsMine) continue;

                var neighbors = GetNeighbors(cell);
                cell.NeighborMines = neighbors.Count(n => n.IsMine);
            }
        }

        public void RevealCell(Cell cell)
        {
            // Если игра окончена, клетка уже открыта или стоит флажок - ничего не делаем
            if (IsGameOver || cell.IsRevealed || cell.IsFlagged) return;

            cell.IsRevealed = true;
            _revealedCount++; // Увеличиваем счетчик открытых клеток

            if (cell.IsMine)
            {
                IsGameOver = true;
                RevealAllMines();
                return;
            }

            // Рекурсивное открытие пустых клеток
            if (cell.NeighborMines == 0)
            {
                var neighbors = GetNeighbors(cell);
                foreach (var neighbor in neighbors)
                {
                    RevealCell(neighbor);
                }
            }

            // ПРОВЕРКА ПОБЕДЫ: если количество открытых клеток равно общему числу безопасных клеток
            if (_revealedCount == (Rows * Columns) - MinesCount)
            {
                IsGameOver = true;
                IsGameWon = true;

                // Для красоты автоматически ставим флажки на все мины при победе
                foreach (var mineCell in Cells.Where(c => c.IsMine))
                {
                    mineCell.IsFlagged = true;
                }
            }
        }

        public void ToggleFlag(Cell cell)
        {
            if (IsGameOver || cell.IsRevealed) return;
            cell.IsFlagged = !cell.IsFlagged;
        }

        private void RevealAllMines()
        {
            foreach (var cell in Cells.Where(c => c.IsMine))
            {
                cell.IsRevealed = true;
            }
        }

        private System.Collections.Generic.IEnumerable<Cell> GetNeighbors(Cell cell)
        {
            var neighbors = new System.Collections.Generic.List<Cell>();

            for (int r = -1; r <= 1; r++)
            {
                for (int c = -1; c <= 1; c++)
                {
                    if (r == 0 && c == 0) continue;

                    int newRow = cell.Row + r;
                    int newCol = cell.Column + c;

                    if (newRow >= 0 && newRow < Rows && newCol >= 0 && newCol < Columns)
                    {
                        neighbors.Add(Cells.First(x => x.Row == newRow && x.Column == newCol));
                    }
                }
            }
            return neighbors;
        }
    }
}