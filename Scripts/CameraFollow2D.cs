// !!!ID: e97ae292805e4610b5645ba998f78761
using UnityEngine;

/// <summary>
/// Simple 2D camera follow script. Follows a target with an optional offset,
/// and can smoothly lag behind using interpolation if enabled.
/// </summary>
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
