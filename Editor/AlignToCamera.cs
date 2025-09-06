// !!!ID: e775f2090a534c8784250063368d941b
using UnityEngine;
using UnityEditor;

namespace Basics {
    public static class AlignToCamera{
        [MenuItem("Basics/Camera/Align Selected to Scene Camera")]
        public static void AlignSelectedToCameraMenu() {
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

            //Quaternion zxRotation = Quaternion.LookRotation(zxForward.normalized, Vector3.up);
            //Vector3 camPos = cam.position;

            foreach (GameObject obj in Selection.gameObjects) {
                Undo.RecordObject(obj.transform, "Align To Camer");
                obj.transform.SetPositionAndRotation(cam.position, cam.rotation);
            }
        }

        // Optional: disable menu item when nothing is selected
        [MenuItem("Basics/Align Selected to Scene Camera", true)]
        private static bool ValidateAlignSelectedToCamera() => Selection.gameObjects.Length > 0;
    }
}
