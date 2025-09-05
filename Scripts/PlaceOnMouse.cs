using UnityEngine;

namespace Basics {
    public class PlaceOnMouse : MonoBehaviour {
        public Camera cam;

        void Start() {
            if (cam == null) cam = Camera.main;
        }

        void Update() {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(cam.transform.position.z - transform.position.z);
            transform.position = cam.ScreenToWorldPoint(mousePos);
        }
    }
}
