// !!!ID: c7dd0bf3b5e14cdebf17i715a75dbb75
using UnityEngine;

/// <summary>
/// Moves a GameObject in a given direction with inertial slowdown.
/// - Starts at the given speed and applies exponential drag over time.
/// - Uses Update to move the transform position each frame.
/// - DragCoefficient controls how quickly the object slows down.
/// </summary>
namespace Basics
{
    public class MoverInert : MonoBehaviour
    {
        public Vector3 direction = Vector3.right;
        public float speed = 1f;
        public float dragCoefficient = 0.1f;

        float actualSpeed;
        float startTime;

        void Start()
        {
            startTime = Time.time;
            actualSpeed = speed;
        }

        void Update()
        {
            //do some inertial slowdown
            float elapsed = Time.time - startTime;
            //do physics based slowdown
            actualSpeed *= Mathf.Exp(-dragCoefficient * Time.deltaTime);
            transform.position += direction.normalized * actualSpeed * Time.deltaTime;
        }
    }
}
