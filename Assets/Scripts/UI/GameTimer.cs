using System;
using UnityEngine;

namespace MR
{
    public class GameTimer : MonoBehaviour
    {
        int _seconds = 0;

        public int Seconds
        {
            get => _seconds;
            set {
                if (_seconds != value)
                {
                    _seconds = value;          
                    UpdateSeconds();
                }
            }
        }

        TMPro.TMP_Text _text;

        float _time = 0;

        bool _isRunning = false;

        void Awake()
        {
            _text = transform.Find("Text").GetComponent<TMPro.TMP_Text>();
            UpdateSeconds();
        }

        void UpdateSeconds()
        {
            if (_text != null)
            {
                var ts = new TimeSpan(0, 0, _seconds);
                _text.text = ts.ToString("mm':'ss");
            }
        }

        public void StartTimer()
        {
            _isRunning = true;
            _time = 0;
        }

        public void StopTimer()
        {
            _isRunning = false;
        }

        void Update()
        {
            if (!_isRunning)
                return;

            _time += Time.deltaTime;
            if (_time > 1)
            {
                _time -= 1;
                Seconds++;
            }
        }
    }
}
