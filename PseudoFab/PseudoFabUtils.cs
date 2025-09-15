#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

public static class PseudoFabUtils {
    static readonly System.Type T_SHW =
        typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");

    static PropertyInfo _propSceneHierarchy;
    static MethodInfo _miSetExpandedRecursive;

    static PseudoFabUtils() {
        _propSceneHierarchy = T_SHW?.GetProperty("sceneHierarchy",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var tSceneHierarchy = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchy");
        _miSetExpandedRecursive = tSceneHierarchy?.GetMethod("SetExpandedRecursive",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, new[] { typeof(int), typeof(bool) }, null);
    }

    public static void Collapse(GameObject go) {
        if (go == null || T_SHW == null || _propSceneHierarchy == null || _miSetExpandedRecursive == null) return;
        var win = EditorWindow.GetWindow(T_SHW);
        if (win == null) return;

        var sceneHierarchy = _propSceneHierarchy.GetValue(win);
        if (sceneHierarchy == null) return;

        // collapse this node and its descendants
        _miSetExpandedRecursive.Invoke(sceneHierarchy, new object[] { go.GetInstanceID(), false });
        win.Repaint();
        EditorApplication.RepaintHierarchyWindow();
    }
}
#endif
