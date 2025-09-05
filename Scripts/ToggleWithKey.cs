using UnityEngine;

namespace Basics.InputHandling {
    public class ToggleWithKey : MonoBehaviour {
        public KeyCode key = KeyCode.Space;
        public GameObject target;

        void Update() {
            if (Input.GetKeyDown(key) && target != null)
                target.SetActive(!target.activeSelf);
        }
    }
}
