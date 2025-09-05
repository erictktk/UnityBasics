using UnityEngine;

namespace Basics.InputHandling {
    public class LookAtMouse2D : MonoBehaviour {
        public Camera cam;

        void Start() {
            if (cam == null) cam = Camera.main;
        }

        void Update() {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(cam.transform.position.z - transform.position.z);
            Vector3 worldMouse = cam.ScreenToWorldPoint(mousePos);

            Vector2 dir = (worldMouse - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
