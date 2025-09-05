using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class CenterChildrenToParent : EditorWindow {
    [MenuItem("Tools/Center Parent To Children")]
    public static void ShowWindow() {
        GetWindow<CenterChildrenToParent>("Center Parent");
    }

    void OnGUI() {
        if (GUILayout.Button("Center Selected Parent")) {
            var parentGO = Selection.activeGameObject;
            if (parentGO == null) {
                EditorUtility.DisplayDialog("No GameObject Selected",
                    "Please select a GameObject with children.", "OK");
                return;
            }

            var parentT = parentGO.transform;
            int count = parentT.childCount;
            if (count == 0) {
                EditorUtility.DisplayDialog("No Children",
                    "Selected GameObject has no children.", "OK");
                return;
            }

            // gather children and compute world‐space centroid
            Vector3 centroid = Vector3.zero;
            var children = new List<Transform>();
            for (int i = 0; i < count; i++) {
                var c = parentT.GetChild(i);
                children.Add(c);
                centroid += c.position;
            }
            centroid /= count;

            // record undo
            Undo.RecordObject(parentT, "Center Parent To Children");
            foreach (var c in children)
                Undo.RecordObject(c, "Center Parent To Children");

            // move parent, then offset children by the same delta
            Vector3 oldPos = parentT.position;
            parentT.position = centroid;
            Vector3 delta = centroid - oldPos;
            foreach (var c in children)
                c.position -= delta;

            // mark scene dirty
            EditorSceneManager.MarkSceneDirty(parentGO.scene);
        }
    }
}
