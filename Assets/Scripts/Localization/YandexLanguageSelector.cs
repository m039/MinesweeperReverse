using m039.BasicLocalization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MR
{
    public class YandexLanguageSelector : BaseLanguageSelector
    {
#if UNITY_EDITOR
        public override float GetElementHeight()
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void DrawElement(Rect rect, bool isActive, bool isFocused)
        {
            EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(nameof(YandexLanguageSelector)));
        }
#endif

        public override BasicLocalizationLanguage GetLanguage(BasicLocalizationProfile profile)
        {
            var yandexGamesManager = GameObject.FindAnyObjectByType<YandexGamesManager>();
            return FindLanguage(profile, yandexGamesManager.GetLangCode());
        }

        BasicLocalizationLanguage FindLanguage(BasicLocalizationProfile profile, string code)
        {
            if (profile.languages == null || string.IsNullOrEmpty(code))
                return null;

            code = code.Trim();

            foreach (var language in profile.languages)
            {
                if (language.locale != null && language.locale.HasCode(code))
                {
                    return language;
                }
            }

            return null;
        }
    }
}
