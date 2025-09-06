// !!!ID: 4ff4de7a1f344edaacdb2331c3b69632
using UnityEngine;


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
