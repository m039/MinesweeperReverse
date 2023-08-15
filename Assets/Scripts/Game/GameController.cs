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

        [Inject] SettingsScreen _settingsScreen;

        [Inject] GameTopPanel _topPanel;

        [Inject] HealthCounter _healthCounter;

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
            _helpScreen.onShow += OnShowScreen;
            _helpScreen.onHide += OnHideScreen;

            _settingsScreen.Hide(true);
            _settingsScreen.onShow += OnShowScreen;
            _settingsScreen.onHide += OnHideScreen;

            _topPanel.QuestionButton.onClick.AddListener(OnQuestionClicked);
            _topPanel.MenuButton.onClick.AddListener(OnMenuClicked);
        }

        void System.IDisposable.Dispose()
        {
            _mineField.onPlaceNumber -= OnPlaceNumer;

            _nextNumberPanel.onNumberSelected -= OnNumberSelected;

            _topPanel.QuestionButton.onClick.RemoveListener(OnQuestionClicked);
            _topPanel.MenuButton.onClick.RemoveListener(OnMenuClicked);

            _helpScreen.onShow -= OnShowScreen;
            _helpScreen.onHide -= OnHideScreen;

            _settingsScreen.onShow -= OnShowScreen;
            _settingsScreen.onHide -= OnHideScreen;
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
                if (_healthCounter.HeartCount > 0)
                {
                    _healthCounter.RemoveHeart(0.6f, () => {
                        // TODO
                    });
                }
            }
        }

        void OnNumberSelected(int number)
        {
            _mineField.IsHoverEnabled = true;
            _mineField.SelectedNumber = number;
        }

        void OnQuestionClicked()
        {
            _helpScreen.Show();
        }

        void OnMenuClicked()
        {
            _settingsScreen.Show();
        }

        void OnShowScreen()
        {
            _mineField.IsHoverEnabled = false;
        }

        void OnHideScreen()
        {
            _mineField.IsHoverEnabled = true;
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
