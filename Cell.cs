using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Minesweeper
{
    public class Cell : INotifyPropertyChanged
    {
        private bool _isRevealed;
        private bool _isFlagged;

        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsMine { get; set; }
        public int NeighborMines { get; set; }

        // Открыта ли клетка
        public bool IsRevealed
        {
            get => _isRevealed;
            set
            {
                if (_isRevealed != value)
                {
                    _isRevealed = value;
                    OnPropertyChanged();
                }
            }
        }

        // Установлен ли флажок
        public bool IsFlagged
        {
            get => _isFlagged;
            set
            {
                if (_isFlagged != value)
                {
                    _isFlagged = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}