using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public class GameController : IStartable, System.IDisposable
    {
        [Inject] NextNumberPanel _nextNumberPanel;

        [Inject] MineField _mineField;

        List<int> _nextNumbers;

        void IStartable.Start()
        {
            _mineField.ConstructField();
            _mineField.IsHoverEnabled = false;
            _mineField.onPlaceNumber += OnPlaceNumer;

            _nextNumberPanel.onNumberSelected += OnNumberSelected;

            _nextNumbers = _mineField.GetNextNumbers();
            _nextNumbers.Shuffle();
            ShowNextNumbers();
        }

        void System.IDisposable.Dispose()
        {
            _mineField.onPlaceNumber -= OnPlaceNumer;

            _nextNumberPanel.onNumberSelected -= OnNumberSelected;
        }

        void OnPlaceNumer(MineCell mineCell)
        {
            if (_mineField.SelectedNumber == mineCell.BombCount)
            {
                mineCell.IsPressed = true;
                mineCell.Number = mineCell.BombCount;

                _nextNumberPanel.HideSelectedNumber();
                if (!_nextNumberPanel.SelectNextNumber())
                {
                    ShowNextNumbers();
                    _nextNumberPanel.SelectNextNumber();
                }
            } else
            {
                mineCell.ShakeAndBlink();
            }
        }

        void OnNumberSelected(int number)
        {
            _mineField.IsHoverEnabled = true;
            _mineField.SelectedNumber = number;
        }

        const int NextNumbersCount = 4;

        void ShowNextNumbers()
        {
            var count = Mathf.Min(_nextNumbers.Count, NextNumbersCount);
            var numbers = _nextNumbers.GetRange(0, count);
            _nextNumbers.RemoveRange(0, count);
            _nextNumberPanel.ShowNumbers(numbers);
        }
    }
}
