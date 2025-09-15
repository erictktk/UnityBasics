// Assets/Editor/LevelToolsMenu.cs
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Reflection;

public static class LevelToolsMenu {
    [MenuItem("Level Tools/Select Parents")]
    static void SelectParents() {
        if (Selection.transforms.Length == 0) return;
        var parents = new GameObject[Selection.transforms.Length];
        for (int i = 0; i < Selection.transforms.Length; i++) {
            var t = Selection.transforms[i];
            parents[i] = t.parent ? t.parent.gameObject : t.gameObject;
        }
        Selection.objects = parents;
    }

    [MenuItem("Level Tools/Select Topmost Parents")]
    static void SelectTopmostParents() {
        if (Selection.transforms.Length == 0) return;
        var roots = new GameObject[Selection.transforms.Length];
        for (int i = 0; i < Selection.transforms.Length; i++) {
            var t = Selection.transforms[i];
            while (t.parent) t = t.parent;
            roots[i] = t.gameObject;
        }
        Selection.objects = roots;
    }

    [MenuItem("Level Tools/Collapse All In Hierarchy")]
    static void CollapseAll() {
        foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            SetExpandedRecursive(go, false);
    }

    [MenuItem("Level Tools/Expand All In Hierarchy")]
    static void ExpandAll() {
        foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            SetExpandedRecursive(go, true);
    }

    static void SetExpandedRecursive(GameObject go, bool expand) {
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var window = Resources.FindObjectsOfTypeAll(type).Length > 0
            ? (EditorWindow)Resources.FindObjectsOfTypeAll(type)[0]
            : null;
        if (window == null) return;

        var mi = type.GetMethod("SetExpandedRecursive",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (mi == null) return;

        mi.Invoke(window, new object[] { go.GetInstanceID(), expand });
    }
}
