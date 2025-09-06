// !!!ID: f519c2390f2042378b2b73d2a2b56a55
using UnityEngine;

namespace Basics {
    public class FlashWhite : MonoBehaviour {
        public SpriteRenderer target;
        public float flashDuration = 0.5f;
        public float loopDelay = 1f;
        public bool flashOnAwake = true;

        private Color originalColor;
        private float timer;
        private int flashesRemaining = 0;
        private bool isFlashing = false;
        private bool isLooping = false;

        void Start() {
            if (target == null) target = GetComponent<SpriteRenderer>();
            originalColor = target.color;
            if (flashOnAwake) Flash(1);
        }

        void Update() {
            if (!isFlashing && isLooping) {
                timer += Time.deltaTime;
                if (timer >= loopDelay) Flash(1);
                return;
            }

            if (isFlashing) {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / flashDuration);
                target.color = Color.Lerp(originalColor, Color.white, t);

                if (timer >= flashDuration) {
                    target.color = originalColor;
                    isFlashing = false;
                    timer = 0f;
                    if (flashesRemaining > 0) Flash(flashesRemaining);
                }
            }
        }

        public void Flash(int n) {
            if (n <= 0) return;
            flashesRemaining = n - 1;
            isFlashing = true;
            timer = 0f;
        }

        public void StartFlashing() {
            isLooping = true;
            timer = loopDelay; // trigger immediately
        }

        public void StopFlashing() {
            isLooping = false;
        }
    }
}
