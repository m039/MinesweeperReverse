using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public class GameController : IStartable, System.IDisposable
    {
        const int MaxNumberOfTries = 2;

        [Inject] NextNumberPanel _nextNumberPanel;

        [Inject] MineField _mineField;

        [Inject] HelpScreen _helpScreen;

        [Inject] SettingsScreen _settingsScreen;

        [Inject] LoseScreen _loseScreen;

        [Inject] WinScreen _winScreen;

        [Inject] GameTopPanel _topPanel;

        [Inject] HealthCounter _healthCounter;

        List<int> _nextNumbers;

        BaseScreen[] _screens;

        int _numberOfTries = MaxNumberOfTries;

        void IStartable.Start()
        {
            _mineField.ConstructField();
            _mineField.IsHoverEnabled = false;
            _mineField.onPlaceNumber += OnPlaceNumer;

            _nextNumberPanel.onNumberSelected += OnNumberSelected;

            _nextNumbers = _mineField.GetNextNumbers();
            _nextNumbers.Shuffle();
            ShowNextNumbers();

            _screens = new BaseScreen[]
            {
                _helpScreen,
                _settingsScreen,
                _loseScreen,
                _winScreen
            };

            foreach (var screen in _screens)
            {
                screen.Hide(true);
                screen.onShow += OnShowScreen;
                screen.onHide += OnHideScreen;
            }

            _loseScreen.ContinueButton.onClick.AddListener(OnContinueClicked);

            _topPanel.QuestionButton.onClick.AddListener(OnQuestionClicked);
            _topPanel.MenuButton.onClick.AddListener(OnMenuClicked);
        }

        void System.IDisposable.Dispose()
        {
            _mineField.onPlaceNumber -= OnPlaceNumer;

            _nextNumberPanel.onNumberSelected -= OnNumberSelected;

            _topPanel.QuestionButton.onClick.RemoveListener(OnQuestionClicked);
            _topPanel.MenuButton.onClick.RemoveListener(OnMenuClicked);

            foreach (var screen in _screens)
            {
                screen.onShow -= OnShowScreen;
                screen.onHide -= OnHideScreen;
            }

            _loseScreen.ContinueButton.onClick.RemoveListener(OnContinueClicked);
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
                    if (!_nextNumberPanel.SelectNextNumber())
                    {
                        _winScreen.Show();
                    }
                }
            } else
            {
                if (_healthCounter.HeartCount > 0)
                {
                    if (_healthCounter.HeartCount == 1)
                    {
                        _mineField.IsHoverEnabled = false;
                    }

                    mineCell.ShakeAndBlink();
                    _healthCounter.RemoveHeart(0.6f, () => {
                        if (_healthCounter.HeartCount <= 0)
                        {
                            DOVirtual.DelayedCall(0.5f, () => _loseScreen.Show());
                        }
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

        void OnContinueClicked()
        {
            _loseScreen.Hide();
            _healthCounter.AddHeart(0.6f, null);
            _numberOfTries--;

            if (_numberOfTries == 0)
            {
                _loseScreen.ContinueButton.gameObject.SetActive(false);
            }
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
