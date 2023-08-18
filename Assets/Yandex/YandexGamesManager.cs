#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

using UnityEngine;
using UnityEngine.Scripting;

namespace MR
{
    public class YandexGamesManager : MonoBehaviour
    {
        static YandexGamesManager s_Instance;

        static public YandexGamesManager Instance
        {
            get
            {
                if (ReferenceEquals(s_Instance, null))
                {
                    return FindObjectOfType<YandexGamesManager>();
                }

                return s_Instance;
            }
        }

        public event System.Action<string> onGetPlayerData;

        public event System.Action onShowRewardedVideoRewarded;

        public event System.Action<bool> onShowRewardedVideoClose;

        public event System.Action<bool> onShowFullscreenAdvClose;

        private void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = this;
                gameObject.transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void YG_showFullscreenAdv();

        [DllImport("__Internal")]
        private static extern void YG_showRewardedVideo();

        [DllImport("__Internal")]
        private static extern void YG_getPlayerData();

        [DllImport("__Internal")]
        private static extern void YG_setPlayerData(string data);

        [DllImport("__Internal")]
        private static extern void YG_setLeaderboardScore(string leaderboard, int number);

        [DllImport("__Internal")]
        private static extern string YG_getLang();
#endif

        public void ShowFullscreenAdv()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_showFullscreenAdv();
#endif
        }

        public void ShowRewardedVideo()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_showRewardedVideo();
#endif
        }

        public string GetLang()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return YG_getLang();
#else
            return null;
#endif
        }

        public void SetLeaderboardScore(string leaderboard, int number)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_setLeaderboardScore(leaderboard, number);
#endif
        }

        [Preserve]
        void OnShowFullscreenAdvClose(string wasShown)
        {
            onShowFullscreenAdvClose?.Invoke(bool.Parse(wasShown));
        }

        [Preserve]
        void OnShowRewardedVideoClose(string wasShown)
        {
            onShowRewardedVideoClose?.Invoke(bool.Parse(wasShown));
        }

        [Preserve]
        void OnShowRewardedVideoRewarded()
        {
            onShowRewardedVideoRewarded?.Invoke();
        }

        public void SetPlayerData(string gameData)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_setPlayerData(gameData);
#endif
        }

        public void GetPlayerData()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_getPlayerData();
#endif
        }

        [Preserve]
        void OnGetPlayerData(string gameData)
        {
            onGetPlayerData?.Invoke(gameData);
        }
    }
}
