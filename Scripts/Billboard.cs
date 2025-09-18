// !!!ID: 4ff4de7a1f344edaacdb2331c3b69632
using UnityEngine;


/// <summary>
/// Makes the GameObject always face the main camera by matching its forward
/// vector to the camera’s forward direction. Runs in both play mode and edit mode.
/// </summary>
namespace Basics {
    [ExecuteAlways]
    public class Billboard : MonoBehaviour {
        private Camera mainCamera;

        void Start() {
            mainCamera = Camera.main;
        }

        void LateUpdate() {
            if (mainCamera == null) return;
            transform.forward = mainCamera.transform.forward;
        }
    }
}
