using UnityEngine;

namespace MR
{
    public class SceneData : MonoBehaviour
    {
        #region Inspector

        [SerializeField] bool _IsEasyLevel = true;

        [SerializeField] AudioSource _SoundSource;

        [SerializeField] int _NumberOfTries = 2;

        #endregion

        public bool IsEasyLevel => _IsEasyLevel;

        public AudioSource SoundSource => _SoundSource;

        public int NumberOfTries => _NumberOfTries;
    }
}
