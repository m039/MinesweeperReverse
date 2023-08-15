using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
    public class HealthCounter : MonoBehaviour
    {
        #region Inspector

        [SerializeField] int _Count = 2;

        #endregion

        RectTransform _template;

        readonly List<RectTransform> _hearts = new();

        public int HeartCount => _hearts.Count;

        void Awake()
        {
            _template = transform.Find("Template").GetComponent<RectTransform>();
            _template.gameObject.SetActive(false);
        }

        void Start()
        {
            CreateHearts();
        }

        void CreateHearts()
        {
            for (int i = 0; i < _Count; i++)
            {
                var heart = Instantiate(_template, transform);
                heart.gameObject.SetActive(true);
                _hearts.Add(heart);
            }
        }

        public void RemoveHeart(float delay, System.Action callback)
        {
            if (_hearts.Count <= 0)
                throw new System.Exception("Can't remove heart.");

            var heart = _hearts[_hearts.Count - 1];
            _hearts.RemoveAt(_hearts.Count - 1);
            heart.DOScale(0, 0.5f)
                .SetDelay(delay)
                .SetEase(Ease.InBack)
                .OnComplete(() => {
                    Destroy(heart.gameObject);
                    callback?.Invoke();
                });
        }

        public void AddHeart(float delay, System.Action callback)
        {
            var heart = Instantiate(_template, transform);
            heart.gameObject.SetActive(true);
            _hearts.Add(heart);

            heart.localScale = Vector3.zero;
            heart.DOScale(1, 0.5f)
                .SetDelay(delay)
                .SetEase(Ease.OutBack)
                .OnComplete(() => callback?.Invoke());
        }
    }
}
