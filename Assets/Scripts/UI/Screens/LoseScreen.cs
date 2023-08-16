using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MR
{
    public class LoseScreen : BaseScreen
    {
        #region Inspector

        [SerializeField] Button _ContinueButton;

        #endregion

        public Button ContinueButton => _ContinueButton;

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
