using UnityEngine;

namespace MR
{
    public class SceneData : MonoBehaviour
    {
        #region Inspector

        [SerializeField] bool _IsEasyLevel = true;

        [SerializeField] AudioSource _SoundSource;

        #endregion

        public bool IsEasyLevel => _IsEasyLevel;

        public AudioSource SoundSource => _SoundSource;
    }
}
