// !!!ID: c66efab2e46341aaabb330952e627f90
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
