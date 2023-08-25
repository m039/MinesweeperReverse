using m039.BasicLocalization;
using SimpleJSON;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public class MainMenuController : IStartable, IDisposable
    {
        static bool s_IsGameReady;

        [Inject] MainMenuView _mainMenuView;

        [Inject] ProgressService _progresService;

        [Inject] YandexGamesManager _yandexGames;

        void IStartable.Start()
        {
            _mainMenuView.NextLanguageButton.onClick.AddListener(OnSelectNextLanguageClicked);
            _mainMenuView.PreviousLanguageButton.onClick.AddListener(OnSelectPreviousLanguageClicked);
            _mainMenuView.PlayEasyButton.onClick.AddListener(OnPlayEasyClicked);
            _mainMenuView.PlayHardButton.onClick.AddListener(OnPlayHardClicked);
            _yandexGames.onGetPlayerData += OnDownloadGameData;
            BasicLocalization.OnLanguageChanged += OnLanguageChanged;

            UpdateBestTimes();

            _yandexGames.GetPlayerData();

            if (!s_IsGameReady)
            {
                _yandexGames.GameReady();
                s_IsGameReady = true;
            }
        }

        void IDisposable.Dispose()
        {
            _mainMenuView.NextLanguageButton.onClick.RemoveListener(OnSelectNextLanguageClicked);
            _mainMenuView.PreviousLanguageButton.onClick.RemoveListener(OnSelectPreviousLanguageClicked);
            _mainMenuView.PlayEasyButton.onClick.RemoveListener(OnPlayEasyClicked);
            _mainMenuView.PlayHardButton.onClick.RemoveListener(OnPlayHardClicked);
            _yandexGames.onGetPlayerData -= OnDownloadGameData;
            BasicLocalization.OnLanguageChanged -= OnLanguageChanged;
        }

        void OnPlayEasyClicked()
        {
            SceneManager.LoadScene(Consts.LevelEasyScene);
        }

        void OnPlayHardClicked()
        {
            SceneManager.LoadScene(Consts.LevelHardScene);
        }

        void OnSelectNextLanguageClicked()
        {
            BasicLocalization.SelectNextLanguage();
        }

        void OnSelectPreviousLanguageClicked()
        {
            BasicLocalization.SelectPreviousLanguage();
        }

        void OnLanguageChanged(BasicLocalizationLanguage language)
        {
            UpdateBestTimes();
        }

        void OnDownloadGameData(string str)
        {
            if (string.IsNullOrEmpty(str))
                return;

            var gameData = JsonUtility.FromJson<YandexGamesData>(str);

            if (gameData.bestTimeEasy > 0)
            {
                _progresService.SetBestTimeEasy(gameData.bestTimeEasy);
            }

            if (gameData.bestTimeHard > 0)
            {
                _progresService.SetBestTimeHard(gameData.bestTimeHard);
            }

            UpdateBestTimes();
        }

        void UpdateBestTimes()
        {
            void displayBestTime(RectTransform group, TMPro.TMP_Text text, int bestTime)
            {
                if (bestTime <= 0)
                {
                    group.gameObject.SetActive(false);
                }
                else
                {
                    group.gameObject.SetActive(true);
                    var bestTimeTs = new TimeSpan(0, 0, bestTime);
                    text.text = string.Format(BasicLocalization.GetTranslation("BestTimeFmt"), bestTimeTs.ToString("mm':'ss"));
                }
            }

            displayBestTime(
                _mainMenuView.BestTimeEasyGroup,
                _mainMenuView.BestTimeEasyText,
                _progresService.GetBestTimeInSecondsEasy()
                );

            displayBestTime(
                _mainMenuView.BestTimeHardGroup,
                _mainMenuView.BestTimeHardText,
                _progresService.GetBestTimeInSecondsHard()
                );
        }
    }
}
