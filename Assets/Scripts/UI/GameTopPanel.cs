using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class GameTopPanel : MonoBehaviour
    {
        #region Inspector

        [SerializeField] Button _QuestionButton;

        [SerializeField] Button _MenuButton;

        #endregion

        public Button QuestionButton => _QuestionButton;

        public Button MenuButton => _MenuButton;
    }
}
