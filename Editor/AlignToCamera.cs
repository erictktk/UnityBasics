// !!!ID: e775f2090a534c8784250063368d941b
using UnityEngine;
using UnityEditor;


/// <summary>
/// Provides editor menu items for aligning objects and the SceneView to cameras.
/// - Align Selected to Scene Camera: moves selected objects to the SceneView camera's position.
/// - Align Scene Camera To Main Camera: aligns the SceneView camera to the first active camera in the scene.
/// Notes:
/// * "Main Camera" here means the first Camera found with gameObject.activeInHierarchy == true.
/// * Includes Undo support for object alignment and validation for menu availability.
/// </summary>

namespace Basics {
    public static class AlignToCamera {
        [MenuItem("Basics/Camera/Align Game Camera (Selected) to Scene Camera")]
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
                Undo.RecordObject(obj.transform, "Align To Camera");
                obj.transform.SetPositionAndRotation(cam.position, cam.rotation);
            }
        }

        // Optional: disable menu item when nothing is selected
        [MenuItem("Basics/Align Selected to Scene Camera", true)]
        private static bool ValidateAlignSelectedToCamera() => Selection.gameObjects.Length > 0;

        [MenuItem("Basics/Camera/Align Scene Camera To Main (Game) Camera")]
        public static void AlignSceneCameraToMain() {
            Camera[] allCams = Object.FindObjectsOfType<Camera>();
            if (allCams == null || allCams.Length == 0) {
                Debug.LogWarning("No cameras found in the scene.");
                return;
            }

            Camera chosen = null;
            foreach (var cam in allCams) {
                if (cam.gameObject.activeInHierarchy) {
                    chosen = cam;
                    break;
                }
            }

            if (chosen == null) {
                Debug.LogWarning("No active cameras found in the scene.");
                return;
            }

            var sv = SceneView.lastActiveSceneView;
            if (sv == null) {
                Debug.LogWarning("No active SceneView found.");
                return;
            }

            sv.LookAt(
                chosen.transform.position,
                chosen.transform.rotation,
                sv.size,
                sv.orthographic
            );

            sv.Repaint();
        }
    }
}
