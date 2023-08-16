using m039.BasicLocalization;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public class MainMenuController : IStartable, IDisposable
    {
        [Inject] MainMenuView _mainMenuView;

        [Inject] ProgressService _progresService;

        void IStartable.Start()
        {
            _mainMenuView.NextLanguageButton.onClick.AddListener(OnSelectNextLanguageClicked);
            _mainMenuView.PreviousLanguageButton.onClick.AddListener(OnSelectPreviousLanguageClicked);
            _mainMenuView.PlayEasyButton.onClick.AddListener(OnPlayEasyClicked);
            BasicLocalization.OnLanguageChanged += OnLanguageChanged;

            UpdateBestTimes();
        }

        void IDisposable.Dispose()
        {
            _mainMenuView.NextLanguageButton.onClick.RemoveListener(OnSelectNextLanguageClicked);
            _mainMenuView.PreviousLanguageButton.onClick.RemoveListener(OnSelectPreviousLanguageClicked);
            _mainMenuView.PlayEasyButton.onClick.RemoveListener(OnPlayEasyClicked);
            BasicLocalization.OnLanguageChanged -= OnLanguageChanged;
        }

        void OnPlayEasyClicked()
        {
            SceneManager.LoadScene(Consts.LevelEasyScene);
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
