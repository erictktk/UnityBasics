// !!!ID: 0d8ec1435bfd466f96e71086038f0306
using UnityEngine;
using UnityEngine.UI;

namespace Basics {
    public class FlashWhiteUI : MonoBehaviour {
        public Graphic target;
        public float flashDuration = 0.5f;
        public float loopDelay = 1f;
        public bool flashOnAwake = true;

        private Color originalColor;
        private float timer;
        private int flashesRemaining = 0;
        private bool isFlashing = false;
        private bool isLooping = false;

        void Start() {
            if (target == null) target = GetComponent<Graphic>();
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
            timer = loopDelay;
        }

        public void StopFlashing() {
            isLooping = false;
        }
    }
}
