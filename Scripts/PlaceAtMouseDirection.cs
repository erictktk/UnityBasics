// !!!ID: 0b3c4e4bdf384fec9c39b04c269d510e
using UnityEngine;

namespace Basics {
    public class PlaceAtMouseDirection : MonoBehaviour {
        [Tooltip("Origin point from which direction is measured")]
        public Transform origin;
        [Tooltip("Maximum distance from origin")]
        public float maxDistance = 5f;
        [Tooltip("Camera used for screen-to-world conversion")]
        public Camera cam;

        void Start() {
            if (origin == null) origin = transform;
            if (cam == null) cam = Camera.main;
        }

        void Update() {
            Vector3 mousePos = Input.mousePosition;
            // Set z so ScreenToWorldPoint projects onto plane of origin
            mousePos.z = Mathf.Abs(cam.transform.position.z - origin.position.z);
            Vector3 worldMouse = cam.ScreenToWorldPoint(mousePos);

            Vector3 dir = worldMouse - origin.position;
            float dist = Mathf.Min(dir.magnitude, maxDistance);
            transform.position = origin.position + dir.normalized * dist;
        }
    }
}

