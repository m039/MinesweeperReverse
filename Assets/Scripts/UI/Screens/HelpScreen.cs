using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class HelpScreen : BaseScreen
    {
        #region Inspector

        [SerializeField] Button _BackButton;

        #endregion

        protected override void Awake()
        {
            base.Awake();

            _BackButton.onClick.AddListener(OnHelpBackClicked);
        }

        protected override void OnDestroy()
        {
            _BackButton.onClick.RemoveListener(OnHelpBackClicked);
        }

        void OnHelpBackClicked()
        {
            Hide();
        }
    }
}
