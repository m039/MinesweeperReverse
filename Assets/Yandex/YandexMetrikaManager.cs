#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

using UnityEngine;

namespace MR
{
    public class YandexMetrikaManager : MonoBehaviour
    {
        static YandexMetrikaManager s_Instance;

        static public YandexMetrikaManager Instance
        {
            get
            {
                if (ReferenceEquals(s_Instance, null))
                {
                    return FindObjectOfType<YandexMetrikaManager>();
                }

                return s_Instance;
            }
        }

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
            if (s_Instance == null)
            {
                s_Instance = this;
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
