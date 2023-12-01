using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR
{
    public class Bootstrap : MonoBehaviour
    {
        float _timer;
        bool _isReady;

        void Start()
        {
            _timer = 0;
            StartGameIfReady();
        }

        void Update()
        {
            if (_isReady)
            {
                return;
            }

            _timer += Time.unscaledDeltaTime;
            if (_timer > 0.01)
            {
                _timer = 0;
                StartGameIfReady();
            }
        }

        void StartGameIfReady()
        {
            if (YandexGamesManager.Instance.IsInitialized())
            {
                _isReady = true;
                SceneManager.LoadScene(Consts.MainMenuScene);
            }
        }
    }
}
