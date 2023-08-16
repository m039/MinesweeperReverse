using UnityEngine.SceneManagement;
using UnityEngine;

namespace MR
{
    public class WinScreen : BaseScreen
    {
        #region Inspector

        [SerializeField] GameTimer _GameTimer;

        #endregion

        public GameTimer GameTimer => _GameTimer;

        public void OnRestartClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnGoToMainMenuClicked()
        {
            SceneManager.LoadScene(Consts.MainMenuScene);
        }
    }
}
