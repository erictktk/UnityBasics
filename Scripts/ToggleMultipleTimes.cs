// !!!ID: 6476d4f2cdfe486e8936af3656343f79
using UnityEngine;

namespace Basics {
    public class ToggleMultipleTimes: MonoBehaviour {
        public enum ToggleMode { Infinite, Finite }
        public ToggleMode mode = ToggleMode.Infinite;
        public int toggleCount = 4; // used if mode == Finite
        public Behaviour target;
        public float delay = 1f;

        private float timer;
        private int toggles;
        private bool state;

        void Start() {
            if (target == null) target = GetComponent<Behaviour>();
            state = target ? target.enabled : false;
        }

        void Update() {
            if (mode == ToggleMode.Finite && toggles >= toggleCount) return;

            timer += Time.deltaTime;
            if (timer >= delay) {
                state = !state;
                if (target) target.enabled = state;
                toggles++;
                timer = 0f;
            }
        }
    }
}

