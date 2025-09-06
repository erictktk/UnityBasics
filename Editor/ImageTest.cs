// !!!ID: 7b8da63584bf4dd2a5fbb7040cfe9e56
using UnityEngine;
using UnityEditor;

public class BasicsImageWindow : EditorWindow {
    Texture2D headerImage;

    [MenuItem("Basics/Show Image")]
    static void Open() {
        GetWindow<BasicsImageWindow>("Unity Basics");
    }

    void OnEnable() {
        headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/basics/unity basics.png");
    }

    void OnGUI() {
        if (headerImage == null) return;

        // Aspect ratio
        float aspect = (float)headerImage.width / headerImage.height;

        // Max width is the window width minus some padding
        float maxWidth = position.width - 10f;
        float width = Mathf.Min(maxWidth, headerImage.width);
        float height = width / aspect;

        // Reserve rect with exact size
        Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

        // Draw without stretching
        EditorGUI.DrawPreviewTexture(rect, headerImage, null, ScaleMode.ScaleToFit);
    }
}
