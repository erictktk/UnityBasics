// !!!ID: c7dd0bf3b5e14cdebf17i715a75dbb75
using UnityEngine;

namespace Basics {
    public class MoverInert : MonoBehaviour {
        public Vector3 direction = Vector3.right;
        public float speed = 1f;
        public float dragCoefficient = 0.1f;

        float actualSpeed;
        float startTime;

        void Start() {
            startTime = Time.time;
            actualSpeed = speed;
        }

        void Update() {
            //do some inertial slowdown
            float elapsed = Time.time - startTime;
            //do physics based slowdown
            actualSpeed *= Mathf.Exp(-dragCoefficient * Time.deltaTime);
            transform.position += direction.normalized * actualSpeed * Time.deltaTime;
        }
    }
}
