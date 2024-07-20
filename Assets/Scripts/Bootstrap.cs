using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR
{
    public class Bootstrap : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return new WaitUntil(YandexGamesManager.Instance.IsInitialized);
            SceneManager.LoadScene(Consts.MainMenuScene);
        }
    }
}
