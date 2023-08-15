using UnityEditor;
using UnityEngine;

namespace MR
{
    [CreateAssetMenu(fileName = "DebugConfig", menuName = Consts.MenuItemRoot + "/DebugConfig", order = 1)]
    public class DebugConfig : ScriptableObject
    {

#if UNITY_EDITOR
        [CustomEditor(typeof(DebugConfig))]
        public class DebugConfigEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (GUILayout.Button("Delete All Preferences"))
                {
                    Debug.Log("All preferences have been deleted.");
                    ((DebugConfig)target).DeleteAllProgress();
                }
            }
        }

        public void DeleteAllProgress()
        {
            PlayerPrefs.DeleteAll();
        }
#endif
    }
}
