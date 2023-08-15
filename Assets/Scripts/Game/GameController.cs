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

        [Inject] HelpScreen _helpScreen;

        [Inject] GameTopPanel _topPanel;

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

            _helpScreen.Hide(true);
            _helpScreen.BackButton.onClick.AddListener(OnHelpBackClicked);

            _topPanel.QuestionButton.onClick.AddListener(OnQuestionClicked);
        }

        void System.IDisposable.Dispose()
        {
            _mineField.onPlaceNumber -= OnPlaceNumer;

            _nextNumberPanel.onNumberSelected -= OnNumberSelected;

            _topPanel.QuestionButton.onClick.RemoveListener(OnQuestionClicked);
            _helpScreen.BackButton.onClick.RemoveListener(OnHelpBackClicked);
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

        void OnQuestionClicked()
        {
            _mineField.IsHoverEnabled = false;
            _helpScreen.Show();
        }

        void OnHelpBackClicked()
        {
            _mineField.IsHoverEnabled = true;
            _helpScreen.Hide();
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
