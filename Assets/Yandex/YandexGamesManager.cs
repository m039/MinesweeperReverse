#if !UNITY_EDITOR && UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

using UnityEngine;

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

        public System.Action<string> onGetData;

        public System.Action onShowRewardedVideoRewarded;

        public System.Action<bool> onShowRewardedVideoClose;

        public System.Action<bool> onShowFullscreenAdvClose;

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
        private static extern void YG_setData(string data);

        [DllImport("__Internal")]
        private static extern void YG_setLeaderboardScore(string leaderboard, int number);

        [DllImport("__Internal")]
        private static extern void YG_getData();

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

        void OnShowFullscreenAdvClose(string wasShown)
        {
            onShowFullscreenAdvClose?.Invoke(bool.Parse(wasShown));
        }

        void OnShowRewardedVideoClose(string wasShown)
        {
            onShowRewardedVideoClose?.Invoke(bool.Parse(wasShown));
        }

        void OnShowRewardedVideoRewarded()
        {
            onShowRewardedVideoRewarded?.Invoke();
        }

        public void SetData(string gameData)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_setData(gameData);
#endif
        }

        public void GetData()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_getData();
#endif
        }

        void OnGetData(string gameData)
        {
            onGetData?.Invoke(gameData);
        }
    }
}
