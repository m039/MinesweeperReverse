using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VContainer;

namespace MR
{
    public class MineNumber : MonoBehaviour
    {
        public int Number
        {
            get => int.Parse(_text.text);
            set => SetNumber(value);
        }

        TMPro.TMP_Text _text;

        [Inject] GameConfig _gameConfig;

        void Awake()
        {
            _text = transform.Find("Text").GetComponent<TMPro.TMP_Text>();
        }

        public void SlideIn(Vector3 startPosition, float delay)
        {
            _text.transform.position = startPosition;
            _text.transform.DOLocalMove(Vector3.zero, 2.0f)
                .SetEase(Ease.OutQuint)
                .SetDelay(delay);
        }

        void SetNumber(int number)
        {
            _text.text = number.ToString();
            _text.color = _gameConfig.FindColorForNumber(number);
        }
    }
}
