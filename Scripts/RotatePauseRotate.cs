// !!!ID: 7ca5cf97dae0415d9f016d54bcf64542
using UnityEngine;

/// <summary>
/// Rotates a GameObject around a chosen axis, alternating between rotation and pause phases.
/// - Axis can be X, Y, Z, or a custom vector.
/// - Rotation speed set by revolutions per second.
/// - rotateDuration controls how long it rotates before pausing.
/// - pauseDuration controls how long it waits before resuming rotation.
/// </summary>
namespace Basics
{
    public class RotatePauseRotate : MonoBehaviour
    {
        public enum Axis { X, Y, Z, Custom }

        public Axis axis = Axis.Z;
        public Vector3 customAxis = Vector3.forward;
        public float revolutionsPerSecond = 1f;
        public float rotateDuration = 1f;
        public float pauseDuration = 1f;

        private float timer;
        private bool rotating = true;
        private Vector3 currentAxis;

        void Start()
        {
            currentAxis = axis switch
            {
                Axis.X => Vector3.right,
                Axis.Y => Vector3.up,
                Axis.Z => Vector3.forward,
                Axis.Custom => customAxis.normalized,
                _ => Vector3.forward
            };
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (rotating)
            {
                float degreesPerSecond = revolutionsPerSecond * 360f;
                transform.Rotate(currentAxis * degreesPerSecond * Time.deltaTime);

                if (timer >= rotateDuration)
                {
                    timer = 0f;
                    rotating = false;
                }
            }
            else
            {
                if (timer >= pauseDuration)
                {
                    timer = 0f;
                    rotating = true;
                }
            }
        }
    }
}

