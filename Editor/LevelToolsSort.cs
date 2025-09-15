// Assets/Editor/LevelToolsSort.cs
using UnityEngine;
using UnityEditor;

public static class LevelToolsSort {
    [MenuItem("Level Tools/Hierarchy/Sort Children By X Position (Asc)")]
    private static void SortChildrenAsc() {
        SortChildren(true);
    }

    [MenuItem("Level Tools/Hierarchy/Sort Children By X Position (Desc)")]
    private static void SortChildrenDesc() {
        SortChildren(false);
    }

    private static void SortChildren(bool ascending) {
        if (Selection.transforms.Length == 0) {
            Debug.LogWarning("Select at least one parent GameObject.");
            return;
        }

        foreach (var parent in Selection.transforms) {
            // Only act on direct children (depth 1)
            var children = new System.Collections.Generic.List<Transform>();
            foreach (Transform child in parent)
                children.Add(child);

            if (ascending)
                children.Sort((a, b) => a.localPosition.x.CompareTo(b.localPosition.x));
            else
                children.Sort((a, b) => b.localPosition.x.CompareTo(a.localPosition.x));

            // Reorder siblings
            for (int i = 0; i < children.Count; i++)
                children[i].SetSiblingIndex(i);

            Debug.Log($"[LevelToolsSort] Sorted {children.Count} children of {parent.name} ({(ascending ? "Asc" : "Desc")}).");
        }
    }

    [MenuItem("Level Tools/Hierarchy/Move Selected To Top")]
    public static void MoveSelectedToTop() {
        if (Selection.activeTransform == null) return;

        var t = Selection.activeTransform;
        t.SetSiblingIndex(0); // puts it at the top (index 0)
    }
}
