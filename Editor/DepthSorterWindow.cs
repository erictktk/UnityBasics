using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;


namespace Basics {
    public class DepthSorterWindow: EditorWindow {
        private GameObject targetParent;
        private float minZ = -5f;
        private float maxZ = 5f;

        private enum BoundsMode { MinY, CenterY, MaxY }
        private BoundsMode boundsMode = BoundsMode.CenterY;

        [MenuItem("Tools/Sort Children by Y and Assign Z")]
        public static void ShowWindow() {
            GetWindow<DepthSorterWindow>("Sort Children Z");
        }

        void OnGUI() {
            targetParent = (GameObject)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(GameObject), true);
            boundsMode = (BoundsMode)EditorGUILayout.EnumPopup("Sort By", boundsMode);
            minZ = EditorGUILayout.FloatField("Min Z", minZ);
            maxZ = EditorGUILayout.FloatField("Max Z", maxZ);

            if (GUILayout.Button("Sort and Apply")) {
                if (targetParent == null) {
                    Debug.LogWarning("No parent selected.");
                    return;
                }

                SortChildren();
            }
        }

        void SortChildren() {
            Transform[] children = targetParent.GetComponentsInChildren<Transform>()
                                               .Where(t => t != targetParent)
                                               .ToArray();

            var sorted = children.OrderBy(t => {
                var renderer = t.GetComponent<Renderer>();
                if (renderer == null)
                    return float.MaxValue;

                Bounds b = renderer.bounds;
                return boundsMode switch {
                    BoundsMode.MinY => b.min.y,
                    BoundsMode.CenterY => b.center.y,
                    BoundsMode.MaxY => b.max.y,
                    _ => b.center.y,
                };
            }).ToList();

            float step = (sorted.Count <= 1) ? 0f : (maxZ - minZ) / (sorted.Count - 1);
            for (int i = 0; i < sorted.Count; i++) {
                Vector3 pos = sorted[i].position;
                pos.z = minZ + step * i;
                Undo.RecordObject(sorted[i], "Sort Z");
                sorted[i].position = pos;
            }

            Debug.Log("Sorted and assigned Z positions.");
        }
    }
}

