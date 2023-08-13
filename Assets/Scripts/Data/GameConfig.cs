using UnityEngine;

namespace MR
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = Consts.MenuItemRoot + "/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [System.Serializable]
        public class NumberColorMapping
        {
            public int number;
            public Color color;
        }

        #region Inspector

        [SerializeField] NumberColorMapping[] _NumberColorMappings;

        #endregion

        public Color FindColorForNumber(int number)
        {
            if (_NumberColorMappings == null)
                return Color.black;

            foreach (var mapping in _NumberColorMappings)
            {
                if (mapping.number == number)
                    return mapping.color;
            }

            return Color.black;
        }
    }
}
