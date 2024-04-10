using UnityEngine;
using UnityEditor;
using System;

namespace Z3.Paths.UnityEditor
{
    public class EditorUtils
    {
        public static void DrawScriptHeader(MonoBehaviour monoBehaviour)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(monoBehaviour), monoBehaviour.GetType(), false);
            GUI.enabled = true;
            EditorGUILayout.Space();
        }

        public static void Separator()
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(7);
            GUI.color = new Color(0, 0, 0, 0.3f);
            GUI.DrawTexture(Rect.MinMaxRect(lastRect.xMin, lastRect.yMax + 4, lastRect.xMax, lastRect.yMax + 6), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        public static ScriptableObject CreateAssetInProject(Type type)
        {
            var path = EditorUtility.SaveFilePanelInProject("Create Asset of type " + type.ToString(), type.Name + ".asset", "asset", "");

            if (string.IsNullOrEmpty(path))
                return null;

            ScriptableObject data = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            return data;
        }
    }
}