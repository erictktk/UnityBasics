using UnityEngine;

namespace Basics {
    public class RotateConstantly : MonoBehaviour {
        public Vector3 rotationSpeed = new Vector3(0f, 360f, 0f);

        void Update() {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}
