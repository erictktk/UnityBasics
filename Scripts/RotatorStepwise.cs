// !!!ID: dda475f764aa47b1bbfdf2ff57613cab
using UnityEngine;

namespace Basics {
    public class RotatorStepwise : MonoBehaviour {
        public enum Axis { X, Y, Z }
        public enum RotationMode { FixedAngle, RandomRange }

        public Axis axis = Axis.Y;
        public RotationMode mode = RotationMode.FixedAngle;

        public float pauseDuration = 1f;

        [Header("Fixed Angle")]
        public float fixedAngle = 45f;

        [Header("Random Range")]
        public float minAngle = -90f;
        public float maxAngle = 90f;

        private float timer;

        void Update() {
            timer += Time.deltaTime;
            if (timer >= pauseDuration) {
                float angle = mode == RotationMode.FixedAngle
                    ? fixedAngle
                    : Random.Range(minAngle, maxAngle);

                ApplyRotation(angle);
                timer = 0f;
            }
        }

        void ApplyRotation(float angle) {
            Vector3 euler = transform.eulerAngles;
            switch (axis) {
                case Axis.X: euler.x += angle; break;
                case Axis.Y: euler.y += angle; break;
                case Axis.Z: euler.z += angle; break;
            }
            transform.eulerAngles = euler;
        }
    }
}
