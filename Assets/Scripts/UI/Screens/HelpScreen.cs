using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class HelpScreen : BaseScreen
    {
        #region Inspector

        [SerializeField] Button _BackButton;

        #endregion

        public Button BackButton => _BackButton;
    }
}
