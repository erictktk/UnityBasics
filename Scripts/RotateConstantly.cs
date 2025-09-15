// !!!ID: 2bb94cf2a4e54d9dbfb08e5017701a78
using log4net.Util;
using UnityEngine;
using static Basics.RotatorStepwise;

/// <summary>
/// Rotates constantly using either Continuous or Discrete mode.
/// </summary>

namespace Basics {
    public class RotateConstantly : MonoBehaviour {
        public enum RotationMode { Continuous, Discrete }

        [Header("Rotation Settings")]
        public RotationMode rotationMode = RotationMode.Continuous;

        [ConditionalHide("rotationMode", 1)]
        public float rotateTick = .5f;
        float lastTickTime = -10f;

        public Vector3 rotationSpeed = new Vector3(0f, 360f, 0f);

        void Update() {
            if (rotationMode == RotationMode.Continuous) {
                transform.Rotate(rotationSpeed * Time.deltaTime);
            }
            else if (rotationMode == RotationMode.Discrete) {
                if (Time.time - lastTickTime >= rotateTick) {
                    transform.Rotate(rotationSpeed * rotateTick);
                    lastTickTime = Time.time;
                }
            }
        }
    }
}
