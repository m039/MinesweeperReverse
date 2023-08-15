using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR
{
    public class SettingsScreen : BaseScreen
    {
        #region Inspector

        [SerializeField] RectTransform _MusicDisable;

        [SerializeField] RectTransform _SoundDisable;

        #endregion

        protected override void Awake()
        {
            base.Awake();

            _MusicDisable.gameObject.SetActive(false);
            _SoundDisable.gameObject.SetActive(false);
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
            _SoundDisable.gameObject.SetActive(!_SoundDisable.gameObject.activeSelf);
        }
    }
}
