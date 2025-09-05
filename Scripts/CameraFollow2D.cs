using UnityEngine;

namespace Basics {
    public class CameraFollow2D : MonoBehaviour {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 0f, -10f);

        [Header("Lag Settings")]
        public bool useLag = false;
        public float lagSpeed = 5f;

        void LateUpdate() {
            if (target == null) return;

            Vector3 desired = target.position + offset;
            if (useLag)
                transform.position = Vector3.Lerp(transform.position, desired, lagSpeed * Time.deltaTime);
            else
                transform.position = desired;
        }
    }
}
