// !!!ID: 4dfd56e48d0b4b5bbf0b900f7c3041d5
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component that flashes the alpha of a Graphic element.
/// Supports single flashes, repeated flashes, and looping with delay.
/// Can start flashing automatically on Awake if enabled.
/// Uses a sawtooth interpolation to fade between original alpha and target alpha.
/// </summary>
namespace Basics
{
    public class FlashAlphaUI : MonoBehaviour
    {
        public Graphic target;
        public float flashDuration = 0.5f;
        public float loopDelay = 1f;
        public bool flashOnAwake = true;
        public float alpha = .2f;

        private Color originalColor;
        private float timer;
        private int flashesRemaining = 0;
        private bool isFlashing = false;
        private bool isLooping = false;
        private Color targetColor;

        void Start()
        {
            if (target == null) target = GetComponent<Graphic>();
            originalColor = target.color;
            targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            if (flashOnAwake) Flash(1);


        }

        void Update()
        {
            if (!isFlashing && isLooping)
            {
                timer += Time.deltaTime;
                if (timer >= loopDelay) Flash(1);
                return;
            }

            if (isFlashing)
            {
                timer += Time.deltaTime;
                // we need to make this saw tooth
                float t = Mathf.Clamp01(timer / flashDuration);
                if (t > 0.5f) t = 1f - t;
                // scale to 0-1
                target.color = Color.Lerp(originalColor, targetColor, t * 2f);

                if (timer >= flashDuration)
                {
                    target.color = originalColor;
                    isFlashing = false;
                    timer = 0f;
                    if (flashesRemaining > 0) Flash(flashesRemaining);
                }
            }
        }

        public void Flash(int n)
        {
            if (n <= 0) return;
            flashesRemaining = n - 1;
            isFlashing = true;
            timer = 0f;
        }

        public void StartFlashing()
        {
            isLooping = true;
            timer = loopDelay;
        }

        public void StopFlashing()
        {
            isLooping = false;
        }
    }
}
