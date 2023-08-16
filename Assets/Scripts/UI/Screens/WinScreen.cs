using UnityEngine.SceneManagement;

namespace MR
{
    public class WinScreen : BaseScreen
    {
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
