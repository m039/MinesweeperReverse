using UnityEngine;

namespace MR
{
    public class SceneData : MonoBehaviour
    {
        #region Inspector

        [SerializeField] bool _IsEasyLevel = true;

        #endregion

        public bool IsEasyLevel => _IsEasyLevel;
    }
}
