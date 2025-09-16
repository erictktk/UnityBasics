// !!!ID: 303f59fec6184aad95fbef0277395d9f
using UnityEngine;

/// <summary>
/// Moves a GameObject to the mouse position in world space.
/// - Converts screen coordinates to world using the assigned camera (defaults to Camera.main).
/// - Keeps correct depth by using the distance between camera and object.
/// </summary>
namespace Basics
{
    public class PlaceOnMouse : MonoBehaviour
    {
        public Camera cam;

        void Start()
        {
            if (cam == null) cam = Camera.main;
        }

        void Update()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(cam.transform.position.z - transform.position.z);
            transform.position = cam.ScreenToWorldPoint(mousePos);
        }
    }
}
