using UnityEngine;

namespace Basics.Player {
    public class TopDownMovement2D : MonoBehaviour {
        public float speed = 5f;

        void Update() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector2 dir = new Vector2(h, v).normalized;
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }
}
