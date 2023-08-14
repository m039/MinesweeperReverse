using UnityEngine;
using VContainer;

namespace MR
{
    public class MineCell : MonoBehaviour
    {
        
        #region Inspector

        [SerializeField] BoxCollider2D _BodyCollider;

        [SerializeField] float _IconYOffset;

        #endregion

        public Vector3 Dimensions => _BodyCollider.size;

        public MineCellIndicatorType IndicatorType
        {
            get => _indicatorType;
            set => SetIndicatorType(value);
        }

        public bool IsPressed
        {
            get => _isPressed;
            set => SetPressed(value);
        }

        public int Number
        {
            get => int.Parse(_number.text);
            set => SetNumber(value);
        }

        public int BombCount { get; set; }

        MineCellIndicatorType _indicatorType = MineCellIndicatorType.Empty;

        bool _isPressed;

        Transform _emptyCell;

        Transform _pressedCell;

        Transform _bomb;

        Transform _icon;

        TMPro.TMP_Text _number;

        [Inject] GameConfig _gameConfig;

        void Awake()
        {
            _emptyCell = transform.Find("CellEmpty");
            _pressedCell = transform.Find("CellPressed");
            _bomb = transform.Find("Icon/Bomb");
            _number = transform.Find("Icon/Number").GetComponent<TMPro.TMP_Text>();
            _icon = transform.Find("Icon");

            IsPressed = false;
            IndicatorType = MineCellIndicatorType.Empty;
        }

        void Start()
        {
            Number = Number;
        }

        void SetIndicatorType(MineCellIndicatorType indicatorType)
        {
            _indicatorType = indicatorType;

            switch (_indicatorType)
            {
                case MineCellIndicatorType.Empty:
                    _bomb.gameObject.SetActive(false);
                    _number.gameObject.SetActive(false);
                    break;

                case MineCellIndicatorType.Number:
                    _bomb.gameObject.SetActive(false);
                    _number.gameObject.SetActive(true);
                    break;

                case MineCellIndicatorType.Bomb:
                    _bomb.gameObject.SetActive(true);
                    _number.gameObject.SetActive(false);
                    break;
            }
        }

        void SetPressed(bool isPressed)
        {
            _isPressed = isPressed;

            _emptyCell.gameObject.SetActive(!_isPressed);
            _pressedCell.gameObject.SetActive(_isPressed);

            if (_isPressed)
            {
                _icon.transform.localPosition = Vector3.zero;
            } else
            {
                _icon.transform.localPosition = Vector3.up * _IconYOffset;
            }
        }

        void SetNumber(int number)
        {
            _number.text = number.ToString();
            _number.color = _gameConfig.FindColorForNumber(number);
        }
    }
}
