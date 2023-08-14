using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VContainer;
using UnityEngine.EventSystems;

namespace MR
{
    public class MineNumber : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        public int Number
        {
            get => int.Parse(_text.text);
            set => SetNumber(value);
        }

        public bool IsNumberEnabled
        {
            get => _text.gameObject.activeSelf;
            set => _text.gameObject.SetActive(value);
        }

        public bool IsOutlineEnabled
        {
            get => _isOutlineEnabled;
            set
            {
                _isOutlineEnabled = value;
                UpdateOutline();
            }
        }

        public bool IsOutlineSelected
        {
            get => _isOutlineSelected;
            set
            {
                _isOutlineSelected = value;
                UpdateOutline();
            }
        }

        public event System.Action<MineNumber> onClick;

        TMPro.TMP_Text _text;
        RectTransform _outline;
        bool _isInside;
        bool _isOutlineEnabled;
        bool _isOutlineSelected;

        [Inject] GameConfig _gameConfig;

        void Awake()
        {
            _text = transform.Find("Text").GetComponent<TMPro.TMP_Text>();
            _outline = transform.Find("Outline").GetComponent<RectTransform>();

            _isOutlineEnabled = false;
            _isInside = false;
            UpdateOutline();
        }

        public void SlideIn(Vector3 startPosition, float delay, System.Action onComplete)
        {
            _text.transform.position = startPosition;
            _text.transform.DOLocalMove(Vector3.zero, 2.0f)
                .SetEase(Ease.OutQuint)
                .SetDelay(delay)
                .OnComplete(() => onComplete?.Invoke());
        }

        void SetNumber(int number)
        {
            _text.text = number.ToString();
            _text.color = _gameConfig.FindColorForNumber(number);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _isInside = true;
            UpdateOutline();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _isInside = false;
            UpdateOutline();
        }

        void UpdateOutline()
        {
            if (!_isOutlineEnabled)
            {
                _outline.gameObject.SetActive(false);
            } else
            {
                _outline.gameObject.SetActive(_isInside || _isOutlineSelected);
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(this);
        }
    }
}
