using UnityEngine;

namespace Assets.Sources.Editor
{
    public static class GizmosUtils
    {
        public static void DrawText(GUISkin guiSkin, string text, Vector3 position, Color? color = null, int fontSizeDenominator = 0, float yOffset = 0)
        {
#if UNITY_EDITOR
            var prevSkin = GUI.skin;
            if (guiSkin == null)
                Debug.LogWarning("editor warning: guiSkin parameter is null");
            else
                GUI.skin = guiSkin;

            GUIContent textContent = new GUIContent(text);

            GUIStyle style = guiSkin != null ? new GUIStyle(guiSkin.GetStyle("Label")) : new GUIStyle();
            if (color != null)
                style.normal.textColor = (Color)color;

            fontSizeDenominator = (int)(fontSizeDenominator / Vector3.Distance(Camera.current.transform.position, position));

            if (fontSizeDenominator > 0)
                style.fontSize = fontSizeDenominator;

            Vector2 textSize = style.CalcSize(textContent);
            Vector3 screenPoint = Camera.current.WorldToScreenPoint(position);

            if (screenPoint.z > 0)
            {
                var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
                UnityEditor.Handles.Label(worldPosition, textContent, style);
            }

            GUI.skin = prevSkin;
#endif
        }
    }
}