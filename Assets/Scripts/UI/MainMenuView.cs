using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MR
{
    public class MainMenuView : MonoBehaviour
    {
        #region Inspector

        [SerializeField] TMP_Text _Title;

        [SerializeField] Button _PlayEasyButton;

        [SerializeField] Button _PlayHardButton;

        [SerializeField] Button _NextLanguageButton;

        [SerializeField] Button _PreviousLanguageButton;

        [SerializeField] RectTransform _BestTimeEasyGroup;

        [SerializeField] TMP_Text _BestTimeEasyText;

        [SerializeField] RectTransform _BestTimeHardGroup;

        [SerializeField] TMP_Text _BestTimeHardText;

        #endregion

        public TMP_Text Title => _Title;

        public Button PlayEasyButton => _PlayEasyButton;

        public Button PlayHardButton => _PlayHardButton;

        public Button NextLanguageButton => _NextLanguageButton;

        public Button PreviousLanguageButton => _PreviousLanguageButton;

        public RectTransform BestTimeEasyGroup => _BestTimeEasyGroup;

        public TMP_Text BestTimeEasyText => _BestTimeEasyText;

        public RectTransform BestTimeHardGroup => _BestTimeHardGroup;

        public TMP_Text BestTimeHardText => _BestTimeHardText;
    }
}
