// !!!ID: c7dd0bf3b5e14cdebf170715275dbb75
using UnityEngine;

/// <summary>
/// Simple mover component for translating a GameObject in a given direction.
/// Modes:
/// - Simple: moves using its own direction and speed.
/// - UseReference: moves using the direction of another MoverSimple reference.
/// Updates position every frame in Update.
/// </summary>
namespace Basics
{
    public class MoverSimple : MonoBehaviour
    {

        [Tooltip("Use reference to just update movement from a referent script")]
        public enum MoveMode { Simple, UseReference };
        public MoveMode moveMode = MoveMode.Simple;
        [ConditionalHide("moveMode", (int)MoveMode.UseReference)]
        public MoverSimple reference;

        public Vector3 direction = Vector3.right;
        public float speed = 1f;

        void Update()
        {
            if (moveMode == MoveMode.Simple)
            {
                transform.position += direction.normalized * speed * Time.deltaTime;
            }
            else if (moveMode == MoveMode.UseReference)
            {

                transform.position += reference.direction.normalized * speed * Time.deltaTime;
            }
        }
    }
}

