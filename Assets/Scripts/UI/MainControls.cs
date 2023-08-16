using UnityEngine;
using UnityEngine.UI;

namespace MR
{
    public class MainControls : MonoBehaviour
    {
        #region Inspector

        [SerializeField] Button _QuestionButton;

        [SerializeField] Button _MenuButton;

        [SerializeField] HealthCounter _HealthCounter;

        [SerializeField] GameTimer _GameTimer;

        #endregion

        public Button QuestionButton => _QuestionButton;

        public Button MenuButton => _MenuButton;

        public HealthCounter HealthCounter => _HealthCounter;

        public GameTimer GameTimer => _GameTimer;
    }
}
