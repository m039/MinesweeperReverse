using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MR
{
    public class MineField : MonoBehaviour
    {
        #region Inspector

        [SerializeField] MineCell _MineCellPrefab;

        [SerializeField] int _Rows = 10;

        [SerializeField] int _Columns = 10;

        [Range(0f, 1f)]
        [SerializeField] float _BombsPercent = 0.3f;

        [SerializeField] float _CellGap = 0.1f;

        #endregion

        public int SelectedNumber { get; set; } = 4;

        public bool IsHoverEnabled
        {
            get => _isHoverEnabled;
            set
            {
                if (_selectedCell != null && !value)
                {
                    _selectedCell.IndicatorType = MineCellIndicatorType.Empty;
                    _selectedCell = null;
                }

                _isHoverEnabled = value;
            }
        }

        public event System.Action<MineCell> onPlaceNumber;

        [Inject] IObjectResolver _container;

        MineCell[,] _cells;

        int[,] _bombsCount;

        MineCell _selectedCell;

        bool _isHoverEnabled;

        [ContextMenu("Construct Field")]
        public void ConstructField()
        {
            if (!Application.isPlaying)
                return;

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var dimensions = _MineCellPrefab.Dimensions;
            var fieldWidth = (dimensions.x + _CellGap) * (_Columns - 1);
            var fieldHeight = (dimensions.y + _CellGap) * (_Rows - 1);
            _cells = new MineCell[_Columns, _Rows];

            for (int y = 0; y < _Rows; y++)
            {
                for (int x = 0; x < _Columns; x++)
                {
                    var cell = _container.Instantiate(_MineCellPrefab, transform);
                    var xPosition = -fieldWidth / 2 + dimensions.x * x + x * _CellGap;
                    var yPosition = -fieldHeight / 2 + dimensions.y * y + y * _CellGap;
                    cell.transform.localPosition = new Vector2(xPosition, yPosition);

                    if (Random.Range(0f, 1f) < _BombsPercent)
                    {
                        cell.IsPressed = true;
                        cell.IndicatorType = MineCellIndicatorType.Bomb;
                    }
                    _cells[x, y] = cell;
                }
            }

            CalcBombData();
            OpenEmptyCells();
        }

        void CalcBombData()
        {
            var columns = _cells.GetLength(0);
            var rows = _cells.GetLength(1);

            _bombsCount = new int[columns, rows];

            bool isBomb(int x, int y)
            {
                if (x < 0 || y < 0 || x >= columns || y >= rows)
                    return false;

                return _cells[x, y].IndicatorType == MineCellIndicatorType.Bomb;
            }

            int countNearBombs(int x, int y)
            {
                int count = 0;

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (isBomb(x + i, y + j))
                        {
                            count++;
                        }
                    }
                }

                return count;
            }

            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    var cell = _cells[x, y];

                    if (cell.IndicatorType == MineCellIndicatorType.Bomb)
                        continue;

                    _bombsCount[x, y] = countNearBombs(x, y);
                    cell.BombCount = _bombsCount[x, y];
                }
            }
        }

        void OpenEmptyCells()
        {
            var columns = _cells.GetLength(0);
            var rows = _cells.GetLength(1);

            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    var cell = _cells[x, y];

                    if (cell.IndicatorType == MineCellIndicatorType.Bomb)
                        continue;

                    if (_bombsCount[x, y] == 0)
                    {
                        cell.IsPressed = true;
                        cell.IndicatorType = MineCellIndicatorType.Empty;
                    }
                }
            }
        }

        public List<int> GetNextNumbers()
        {
            var nextNumbers = new List<int>();
            var columns = _cells.GetLength(0);
            var rows = _cells.GetLength(1);

            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    var cell = _cells[x, y];

                    if (cell.IndicatorType == MineCellIndicatorType.Bomb ||
                        (cell.IsPressed & cell.IndicatorType == MineCellIndicatorType.Empty))
                        continue;

                    nextNumbers.Add(cell.BombCount);
                }
            }

            return nextNumbers;
        }

        void Update()
        {
            if (IsHoverEnabled)
            {
                ProcessInput();
            }
        }

        void ProcessInput()
        {
            if (Input.GetMouseButtonUp(0) && _selectedCell != null)
            {
                onPlaceNumber?.Invoke(_selectedCell);
                return;
            }

            var hoveredCell = GetHoveredCell();
            if (hoveredCell != _selectedCell)
            {
                if (_selectedCell != null && !_selectedCell.IsPressed)
                {
                    _selectedCell.IndicatorType = MineCellIndicatorType.Empty;
                }

                if (hoveredCell != null && !hoveredCell.IsPressed)
                {
                    _selectedCell = hoveredCell;
                }
                else
                {
                    _selectedCell = null;
                }

                if (_selectedCell != null)
                {
                    _selectedCell.IndicatorType = MineCellIndicatorType.Number;
                    _selectedCell.Number = SelectedNumber;
                }
            }
        }

        static readonly RaycastHit2D[] s_Cache = new RaycastHit2D[16];

        MineCell GetHoveredCell()
        {
            var count = Physics2D.RaycastNonAlloc(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero, s_Cache);
            for (int i = 0; i < count; i++)
            {
                var cell = s_Cache[i].transform.GetComponentInParent<MineCell>();
                if (cell != null)
                {
                    return cell;
                }
            }

            return null;
        }
    }
}
