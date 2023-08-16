using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MR
{
    public class YandexMetrikaManager : MonoBehaviour
    {
        public static YandexMetrikaManager Instance;

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void YM_Hit(string str);

        [DllImport("__Internal")]
        private static extern void YM_ReachGoal(string target);
#endif

        public void Hit(string url)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            YM_Hit(url);
#endif
        }

        public void ReachGoal(string target)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            YM_ReachGoal(target);
#endif
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
