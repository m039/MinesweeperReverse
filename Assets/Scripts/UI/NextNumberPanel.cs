using System;
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

        public event System.Action<int> onNumberSelected;

        [Inject] IObjectResolver _container;

        void Awake()
        {
            foreach (var mn in _MineNumbers)
            {
                mn.onClick += OnMinNumberClicked;
            }
        }

        void OnDestroy()
        {
            foreach (var mn in _MineNumbers)
            {
                mn.onClick -= OnMinNumberClicked;
            }
        }

        void OnMinNumberClicked(MineNumber mineNumber)
        {
            foreach (var mn in _MineNumbers)
            {
                mn.IsOutlineSelected = mn == mineNumber;
            }
            onNumberSelected?.Invoke(mineNumber.Number);
        }

        public void ShowNumbers(IReadOnlyList<int> numbers)
        {
            foreach (var mn in _MineNumbers)
            {
                mn.gameObject.SetActive(false);
                mn.IsNumberEnabled = false;
            }

            var delay = 0.1f;

            for (int i = 0; i < numbers.Count; i++)
            {
                var mineNumber = _MineNumbers[i];
                mineNumber.IsNumberEnabled = true;
                mineNumber.IsOutlineEnabled = false;
                mineNumber.gameObject.SetActive(true);
                mineNumber.Number = numbers[i];
                mineNumber.SlideIn(_SlideInLocation.position, delay, () => mineNumber.IsOutlineEnabled = true);
                delay += 0.3f;
            }
        }

        public void HideSelectedNumber()
        {
            foreach (var mn in _MineNumbers)
            {
                if (mn.IsOutlineSelected)
                {
                    mn.IsOutlineSelected = false;
                    mn.IsNumberEnabled = false;
                }
            }
        }

        public bool SelectNextNumber()
        {
            foreach (var mn in _MineNumbers)
            {
                if (mn.IsNumberEnabled)
                {
                    mn.IsOutlineSelected = true;
                    onNumberSelected?.Invoke(mn.Number);
                    return true;
                }
            }

            return false;
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
