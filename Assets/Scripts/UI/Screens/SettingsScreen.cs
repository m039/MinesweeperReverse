using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace MR
{
    public class SettingsScreen : BaseScreen
    {
        #region Inspector

        [SerializeField] RectTransform _MusicDisable;

        [SerializeField] RectTransform _SoundDisable;

        #endregion

        [Inject] AudioController _audioController;

        protected override void Awake()
        {
            base.Awake();

            _MusicDisable.gameObject.SetActive(false);
            UpdateSoundDisable();
        }

        public void OnGoToMainMenuClick()
        {
            SceneManager.LoadScene(Consts.MainMenuScene);
        }

        public void OnMusicClick()
        {
            _MusicDisable.gameObject.SetActive(!_MusicDisable.gameObject.activeSelf);
        }

        public void OnSoundClick()
        {
            _audioController.SetSoundEnabled(!_audioController.IsSoundEnabled());
            UpdateSoundDisable();
        }

        void UpdateSoundDisable()
        {
            _SoundDisable.gameObject.SetActive(!_audioController.IsSoundEnabled());
        }
    }
}
