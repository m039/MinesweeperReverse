using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public class NextNumberPanel : MonoBehaviour, IInitializable
    {
        #region Inspector

        [SerializeField] MineNumber[] _MineNumbers;

        [SerializeField] RectTransform _SlideInLocation;

        #endregion

        [Inject] IObjectResolver _container;

        IEnumerator Start()
        {
            foreach (var mn in _MineNumbers)
            {
                mn.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);

            ShowNumbers(new int[] { 1, 2, 3, 4 });
        }

        [ContextMenu("Show Numbers")]
        public void ShowNumbers()
        {
            ShowNumbers(new int[] { 1, 2, 3, 4 });
        }

        public void ShowNumbers(int[] numbers)
        {
            foreach (var mn in _MineNumbers)
            {
                mn.gameObject.SetActive(false);
            }

            var delay = 0.1f;

            for (int i = 0; i < numbers.Length; i++)
            {
                _MineNumbers[i].gameObject.SetActive(true);
                _MineNumbers[i].Number = numbers[i];
                _MineNumbers[i].SlideIn(_SlideInLocation.position, delay);
                delay += 0.5f;
            }
        }

        void IInitializable.Initialize()
        {
            foreach (var mn in _MineNumbers)
            {
                _container.Inject(mn);
            }
        }
    }
}
