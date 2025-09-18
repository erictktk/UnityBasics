// !!!ID: 9c65abeb46fa40d8938c929b4143e15c
using UnityEngine;

/// <summary>
/// Moves the GameObject in 2D space using horizontal and vertical input  
/// Applies (normalized by default) direction multiplied by speed and deltaTime  
/// </summary>
namespace Basics.Player {
    public class TopDownMovement2D : MonoBehaviour {
        public float speed = 5f;
        public bool normalize = true;
        void Update() {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector2 dir;
            if (normalize)
                dir = new Vector2(h, v).normalized;
            else
                dir = new Vector2(h, v);
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }
}
