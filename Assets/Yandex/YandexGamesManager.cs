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

        public const string CODE_LEADERBOARD_PLAYER_NOT_PRESENT = "LEADERBOARD_PLAYER_NOT_PRESENT";

        [System.Serializable]
        public class LeaderboardPlayerEntryResponse
        {
            public int score;
            public string extraData;
        }

        [System.Serializable]
        public class ErrorResponse
        {
            public string code;
            public int httpStatus;
            public string message;
            public string name;
            public string stack;
        }

        public event System.Action<string> onGetPlayerData;

        public event System.Action onShowRewardedVideoRewarded;

        public event System.Action<bool> onShowRewardedVideoClose;

        public event System.Action<bool> onShowFullscreenAdvClose;

        public event System.Action<LeaderboardPlayerEntryResponse> onGetLeaderboardPlayerEntry;

        public event System.Action<ErrorResponse> onGetLeaderboardPlayerEntryError;

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
        private static extern void YG_getLeaderboardPlayerEntry(string leaderboard);

        [DllImport("__Internal")]
        private static extern string YG_getLang();

        [DllImport("__Internal")]
        private static extern void YG_gameReady();
#endif

        public void ShowFullscreenAdv()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_showFullscreenAdv();
#else
            onShowFullscreenAdvClose?.Invoke(false);
#endif
        }

        public void ShowRewardedVideo()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_showRewardedVideo();
#else
            onShowRewardedVideoClose?.Invoke(false);
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

        public void GetLeaderboardPlayerEntry(string leaderboard)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_getLeaderboardPlayerEntry(leaderboard);
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

        [Preserve]
        void OnGetLeaderboardPlayerEntry(string response)
        {
            onGetLeaderboardPlayerEntry?.Invoke(JsonUtility.FromJson<LeaderboardPlayerEntryResponse>(response));
        }

        [Preserve]
        void OnGetLeaderboardPlayerEntryError(string response)
        {
            onGetLeaderboardPlayerEntryError?.Invoke(JsonUtility.FromJson<ErrorResponse>(response));
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

        public void GameReady()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            YG_gameReady();
#endif
        }
    }
}
