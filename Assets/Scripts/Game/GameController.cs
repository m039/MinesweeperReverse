using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public class GameController : IStartable, System.IDisposable
    {
        const string ShowHelpAtFirstStartKey = "show_help_at_first_start_key";

        [Inject] NextNumberPanel _nextNumberPanel;

        [Inject] MineField _mineField;

        [Inject] HelpScreen _helpScreen;

        [Inject] SettingsScreen _settingsScreen;

        [Inject] LoseScreen _loseScreen;

        [Inject] WinScreen _winScreen;

        [Inject] MainControls _mainControls;

        [Inject] ConfettiController _confettiController;

        [Inject] ProgressService _progressService;

        [Inject] SceneData _sceneData;

        [Inject] AudioController _audioController;

        List<int> _nextNumbers;

        BaseScreen[] _screens;

        bool _isNumberSelected = false;

        System.Action _onShowHelpAtFirstStart;

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

            _mainControls.QuestionButton.onClick.AddListener(OnQuestionClicked);
            _mainControls.MenuButton.onClick.AddListener(OnMenuClicked);

            if (!PlayerPrefs.HasKey(ShowHelpAtFirstStartKey))
            {
                _onShowHelpAtFirstStart = () =>
                {
                    PlayerPrefs.SetInt(ShowHelpAtFirstStartKey, 1);
                    _mainControls.GameTimer.StartTimer();
                };
                _helpScreen.Show(true);
            } else
            {
                _mainControls.GameTimer.StartTimer();
            }

            YandexMetrikaManager.Instance.ReachGoal("start_game_" + (_sceneData.IsEasyLevel ? "easy" : "hard"));
            YandexGamesManager.Instance.ShowFullscreenAdv();
        }

        void System.IDisposable.Dispose()
        {
            _mineField.onPlaceNumber -= OnPlaceNumer;

            _nextNumberPanel.onNumberSelected -= OnNumberSelected;

            _mainControls.QuestionButton.onClick.RemoveListener(OnQuestionClicked);
            _mainControls.MenuButton.onClick.RemoveListener(OnMenuClicked);

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

                _audioController.Play(SoundId.CellClickGood);

                _nextNumberPanel.HideSelectedNumber();
                if (!_nextNumberPanel.SelectNextNumber())
                {
                    ShowNextNumbers();
                    if (!_nextNumberPanel.SelectNextNumber())
                    {
                        _confettiController.Play();
                        _mainControls.GameTimer.StopTimer();
                        DOVirtual.DelayedCall(0.3f, () =>
                        {
                            _audioController.Play(SoundId.WinScreenOpen);
                            if (_sceneData.IsEasyLevel)
                            {
                                _progressService.SetBestTimeEasy(_mainControls.GameTimer.Seconds);
                            } else
                            {
                                _progressService.SetBestTimeHard(_mainControls.GameTimer.Seconds);
                            }
                            _winScreen.GameTimer.Seconds = _mainControls.GameTimer.Seconds;
                            _winScreen.Show();

                            YandexMetrikaManager.Instance.ReachGoal("game_completed_" + (_sceneData.IsEasyLevel ? "easy" : "hard"));

                            YandexGamesManager.Instance.SetPlayerData(JsonUtility.ToJson(new YandexGamesData
                            {
                                bestTimeEasy = _progressService.GetBestTimeInSecondsEasy(),
                                bestTimeHard = _progressService.GetBestTimeInSecondsHard()
                            }));

                            YandexGamesManager.Instance.SetLeaderboardScore(
                                _sceneData.IsEasyLevel ? "leaderboardeasy2" : "leaderboardhard2",
                                _mainControls.GameTimer.Seconds * 1000
                                );
                        });
                    }
                }
            } else
            {
                var healthCounter = _mainControls.HealthCounter;
                _audioController.Play(SoundId.CellClickBad);

                if (healthCounter.HeartCount > 0)
                {
                    if (healthCounter.HeartCount == 1)
                    {
                        _mainControls.GameTimer.StopTimer();
                        _mineField.IsHoverEnabled = false;
                    }

                    mineCell.Shake();
                    mineCell.Blink();

                    foreach (var bomb in _mineField.GetNearbyBombs(mineCell))
                    {
                        bomb.Blink();
                    }

                    healthCounter.RemoveHeart(0.6f, () => {
                        if (healthCounter.HeartCount <= 0)
                        {
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                _audioController.Play(SoundId.LoseScreenOpen);
                                _loseScreen.Show();
                            });
                        }
                    });
                }
            }
        }

        void OnNumberSelected(int number)
        {
            _mineField.IsHoverEnabled = true;
            _mineField.SelectedNumber = number;
            _isNumberSelected = true;
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
            _mineField.IsHoverEnabled = false && _isNumberSelected;
        }

        void OnHideScreen()
        {
            _mineField.IsHoverEnabled = true && _isNumberSelected;

            _onShowHelpAtFirstStart?.Invoke();
            _onShowHelpAtFirstStart = null;
        }

        void OnContinueClicked()
        {
            void oneShot(bool wasShown)
            {
                _loseScreen.Hide();
                _mainControls.HealthCounter.AddHeart(0.6f, null);
                YandexGamesManager.Instance.onShowRewardedVideoClose -= oneShot;
                _mainControls.GameTimer.StartTimer();
            };

            YandexGamesManager.Instance.onShowRewardedVideoClose += oneShot;
            YandexGamesManager.Instance.ShowRewardedVideo();
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
