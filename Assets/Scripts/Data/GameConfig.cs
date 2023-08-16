using System.Collections.Generic;
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

        [System.Serializable]
        public class AudioMapping
        {
            public SoundId soundId;
            public AudioClip audioClip;
        }

        #region Inspector

        [SerializeField] NumberColorMapping[] _NumberColorMappings;

        [SerializeField] AudioMapping[] _AudioMappings;

        #endregion

        public IReadOnlyList<AudioMapping> AudioMappings => _AudioMappings;

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
