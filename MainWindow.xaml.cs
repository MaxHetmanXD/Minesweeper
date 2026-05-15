using Minesweeper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        private Board _gameBoard;

        public MainWindow()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void StartNewGame()
        {
            _gameBoard = new Board(10, 10, 10);
            this.DataContext = _gameBoard;
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Cell clickedCell)
            {
                _gameBoard.RevealCell(clickedCell);

                if (_gameBoard.IsGameOver)
                {
                    if (_gameBoard.IsGameWon)
                    {
                        MessageBox.Show("Поздравляем! Вы разминировали всё поле!", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Вы попали на мину! Попробуйте еще раз.", "Поражение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    StartNewGame();
                }
            }
        }

        private void Cell_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Cell rightClickedCell)
            {
                _gameBoard.ToggleFlag(rightClickedCell);
                e.Handled = true;
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }
    }
}