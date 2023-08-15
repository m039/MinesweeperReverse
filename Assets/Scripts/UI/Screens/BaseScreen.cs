using DG.Tweening;
using UnityEngine;

namespace MR
{
    public class BaseScreen : MonoBehaviour
    {
        protected CanvasGroup Shadow;

        protected CanvasGroup Container;

        public bool IsAnimating { get; private set; }

        public event System.Action onShow;

        public event System.Action onHide;

        protected virtual void Awake()
        {
            Shadow = transform.Find("Shadow").GetComponent<CanvasGroup>();
            Container = transform.Find("Container").GetComponent<CanvasGroup>();
        }

        protected virtual void OnDestroy()
        {
        }

        public void Show(bool immedate = false)
        {
            onShow?.Invoke();

            Container.gameObject.SetActive(true);
            Shadow.gameObject.SetActive(true);

            if (immedate)
            {
                return;
            }

            IsAnimating = true;

            Shadow.alpha = 0f;
            Shadow.DOFade(1, 0.6f);

            Container.alpha = 0f;
            Container.DOFade(1, 0.6f);

            Container.transform.localPosition = new Vector3(0, -400, 0);
            Container.transform.DOLocalMoveY(0, 0.6f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => IsAnimating = false);
        }

        public void Hide(bool immedate = false)
        {
            if (immedate)
            {
                Container.gameObject.SetActive(false);
                Shadow.gameObject.SetActive(false);
                onHide?.Invoke();
                return;
            }

            IsAnimating = true;

            Shadow.DOFade(0, 0.6f);

            Container.DOFade(0, 0.6f);

            Container.transform.DOLocalMoveY(-400, 0.66f)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    Container.gameObject.SetActive(false);
                    Shadow.gameObject.SetActive(false);
                    IsAnimating = false;
                    onHide?.Invoke();
                });
        }
    }
}
