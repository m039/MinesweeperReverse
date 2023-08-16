using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
    public class ProgressService
    {
        const string BestTimeEasyKey = "best_time_easy_key_seconds";

        const string BestTimeHardKey = "best_time_hard_key_seconds";

        public void SetBestTimeEasy(int seconds)
        {
            var oldSeconds = GetBestTimeInSecondsEasy();
            if (seconds > oldSeconds && oldSeconds > 0)
                return;

            PlayerPrefs.SetInt(BestTimeEasyKey, seconds);
        }

        public void SetBestTimeHard(int seconds)
        {
            var oldSeconds = GetBestTimeInSecondsHard();
            if (seconds > oldSeconds && oldSeconds > 0)
                return;

            PlayerPrefs.SetInt(BestTimeHardKey, seconds);
        }

        public int GetBestTimeInSecondsEasy()
        {
            return PlayerPrefs.GetInt(BestTimeEasyKey, -1);
        }

        public int GetBestTimeInSecondsHard()
        {
            return PlayerPrefs.GetInt(BestTimeHardKey, -1);
        }
    }
}
