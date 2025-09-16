// !!!ID: 010157ad6d014e70bb4067048d8887f3
using UnityEngine;

namespace Basics {
    public class FollowTarget : MonoBehaviour {
        public Transform target;
        public float speed = 1f;

        public bool isOn = true;

        void Update() {
            if (target == null) return;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
