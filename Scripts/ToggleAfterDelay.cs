// !!!ID: d211d4b52335417396dc87355bf90e48
using UnityEngine;

namespace Basics {
    public class ToggleAfterDelay : MonoBehaviour {
        public GameObject target;
        public float onDelay = 1f;
        public float offDelay = 2f;

        void Start() {
            if (target != null)
                target.SetActive(false);
            Invoke(nameof(TurnOn), onDelay);
            Invoke(nameof(TurnOff), offDelay);
        }

        void TurnOn() {
            if (target != null)
                target.SetActive(true);
        }

        void TurnOff() {
            if (target != null)
                target.SetActive(false);
        }
    }
}
