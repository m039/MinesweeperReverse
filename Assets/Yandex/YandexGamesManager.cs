using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MR
{
    public class YandexGamesManager : MonoBehaviour
    {
        [System.Serializable]
        public class YandexGameData
        {
            public int bestTimeEasy = -1;
            public int bestTimeHard = -1;
        }

        static public YandexGamesManager Instance;

        public System.Action<YandexGameData> onDownloadGameData;

        public System.Action onRewardedVideoRewarded;

        public System.Action<bool> onRewardedVideoClosed;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
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
        private static extern void ShowFullscreenAdvInternal();

        [DllImport("__Internal")]
        private static extern void ShowRewardedVideoInternal();

        [DllImport("__Internal")]
        private static extern void UploadGameDataInternal(string data);

        [DllImport("__Internal")]
        private static extern void SetLeaderboardScoreInternal(string leaderboard, int number);

        [DllImport("__Internal")]
        private static extern void DownloadGameDataInternal();

        [DllImport("__Internal")]
        private static extern string GetLangInternal();
#endif

        public void ShowFullscreenAdv()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            ShowFullscreenAdvInternal();
#endif
        }

        public void ShowRewardedVideo()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            ShowRewardedVideoInternal();
#endif
        }

        public string GetLangCode()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return GetLangInternal();
#else
            return null;
#endif
        }

        public void SetLeaderboardScore(string leaderboard, int seconds)
        {
            var number = seconds * 1000;
#if !UNITY_EDITOR && UNITY_WEBGL
            SetLeaderboardScoreInternal(leaderboard, number);
#endif
        }

        public void OnFullscreenAdvClosed(string wasShown)
        {
        }

        public void OnRewardedVideoClosed(string wasShown)
        {
            onRewardedVideoClosed?.Invoke(bool.Parse(wasShown));
        }

        public void OnRewardedVideoRewarded()
        {
            onRewardedVideoRewarded?.Invoke();
        }

        public void UploadGameData(YandexGameData gameData)
        {
            if (gameData == null)
                return;

#if !UNITY_EDITOR && UNITY_WEBGL
            var json = JsonUtility.ToJson(gameData);
            UploadGameDataInternal(json);
#endif
        }

        public void DownloadGameData()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            DownloadGameDataInternal();
#endif
        }

        public void OnDownloadGameData(string json)
        {
            onDownloadGameData?.Invoke(JsonUtility.FromJson<YandexGameData>(json));
        }
    }
}
