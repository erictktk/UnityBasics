using UnityEngine;
using UnityEditor;

namespace Basics {
    public static class AlignToCameraZXMenu {
        [MenuItem("Basics/Camera/Align Selected to Scene Camera (ZX rotation, XYZ position)")]
        public static void AlignSelectedToCameraZXMenu() {
            var sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null || sceneView.camera == null) {
                Debug.LogWarning("No active SceneView camera found.");
                return;
            }

            Transform cam = sceneView.camera.transform;

            // Yaw-only from camera (project forward onto ZX), full XYZ position from camera
            Vector3 zxForward = new Vector3(cam.forward.x, 0f, cam.forward.z);
            if (zxForward.sqrMagnitude < 1e-6f) {
                Debug.LogWarning("Camera forward too vertical to project onto ZX plane.");
                return;
            }
            Quaternion zxRotation = Quaternion.LookRotation(zxForward.normalized, Vector3.up);
            Vector3 camPos = cam.position;

            foreach (GameObject obj in Selection.gameObjects) {
                Undo.RecordObject(obj.transform, "Align To Camera ZX");
                obj.transform.SetPositionAndRotation(camPos, zxRotation);
            }
        }

        // Optional: disable menu item when nothing is selected
        [MenuItem("Basics/Align Selected to Scene Camera (ZX rotation, XYZ position)", true)]
        private static bool ValidateAlignSelectedToCameraZX() => Selection.gameObjects.Length > 0;
    }
}
