using UnityEngine;

namespace Basics {
    public class FollowTarget : MonoBehaviour {
        public Transform target;
        public float speed = 1f;

        void Update() {
            if (target == null) return;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
