using UnityEngine;

namespace Basics {
    public class DestroyAfterDelay : MonoBehaviour {
        public enum TriggerMode { Automatic, Set }
        public TriggerMode mode = TriggerMode.Automatic;
        public float delay = 1f;

        private float timer;
        private bool running;

        void Start() {
            if (mode == TriggerMode.Automatic)
                StartCountdown();
        }

        void Update() {
            if (!running) return;
            timer += Time.deltaTime;
            if (timer >= delay)
                Destroy(gameObject);
        }

        // Call this to begin the countdown when mode == Set
        public void StartCountdown() {
            running = true;
            timer = 0f;
        }
    }
}
