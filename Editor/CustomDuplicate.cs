using UnityEngine;
using UnityEditor;

public static class CustomDuplicate {
    [MenuItem("Edit/Custom Duplicate %d")] // %d = Ctrl+D or Cmd+D
    private static void DuplicateAndInsertAfter() {
        if (Selection.activeGameObject == null) return;

        GameObject original = Selection.activeGameObject;
        Transform parent = original.transform.parent;
        int index = original.transform.GetSiblingIndex();

        GameObject duplicate = Object.Instantiate(original, parent);
        duplicate.name = original.name + " (Duplicate)";

        Undo.RegisterCreatedObjectUndo(duplicate, "Duplicate " + original.name);

        // Insert right after the original
        duplicate.transform.SetSiblingIndex(index + 1);

        Selection.activeGameObject = duplicate;
    }
}

