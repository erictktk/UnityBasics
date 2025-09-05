using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* !!!SUMMARY
 * HealthBar
 * ---------
 * A flexible health bar component for Unity UI.
 *
 * Features:
 * - Scales a RectTransform along X or Y to represent health percentage.
 * - Direct setters:
 *     - SetPercentage(float percent)
 *     - SetFromValues(float current, float total)
 * - Incrementing:
 *     - IncrementByPercentage(float deltaPercent)
 *     - IncrementByValue(float delta, float max)
 * - Smooth changes over time using coroutines:
 *     - SmoothChangeByPercentage(float deltaPercent, float duration)
 *     - SmoothChangeByValue(float delta, float max, float duration)
 *
 * Internally:
 * - Tracks currentHealth and totalHealth.
 * - Updates bar scale automatically when values change.
 */

namespace Basics {
    public class HealthBar : MonoBehaviour {
        public enum ScaleAxis { X, Y }

        public RectTransform bar;
        public float currentHealth;
        public float totalHealth = 100f;
        public ScaleAxis axis = ScaleAxis.X;

        void Start() {
            if (bar == null) bar = GetComponent<RectTransform>();
            UpdateBar();
        }

        // --- Direct setters ---
        public void SetPercentage(float percent) {
            currentHealth = Mathf.Clamp01(percent) * totalHealth;
            UpdateBar();
        }

        public void SetFromValues(float current, float total) {
            currentHealth = Mathf.Clamp(current, 0f, total);
            totalHealth = total;
            UpdateBar();
        }

        // --- Incrementing ---
        public void IncrementByPercentage(float deltaPercent) {
            currentHealth += deltaPercent * totalHealth;
            currentHealth = Mathf.Clamp(currentHealth, 0f, totalHealth);
            UpdateBar();
        }

        public void IncrementByValue(float delta, float max) {
            totalHealth = max;
            currentHealth = Mathf.Clamp(currentHealth + delta, 0f, totalHealth);
            UpdateBar();
        }

        // --- Smooth transition over time ---
        public void SmoothChangeByPercentage(float deltaPercent, float duration) {
            float delta = deltaPercent * totalHealth;
            StartCoroutine(SmoothAdjustHealth(currentHealth + delta, duration));
        }

        public void SmoothChangeByValue(float delta, float max, float duration) {
            totalHealth = max;
            StartCoroutine(SmoothAdjustHealth(currentHealth + delta, duration));
        }

        // --- Internals ---
        private void UpdateBar() {
            float percent = Mathf.Clamp01(currentHealth / totalHealth);
            if (bar != null) {
                Vector3 scale = bar.localScale;
                if (axis == ScaleAxis.X) scale.x = percent;
                else scale.y = percent;
                bar.localScale = scale;
            }
        }

        private IEnumerator SmoothAdjustHealth(float targetHealth, float duration) {
            float start = currentHealth;
            float end = Mathf.Clamp(targetHealth, 0f, totalHealth);
            float t = 0f;
            while (t < 1f) {
                t += Time.deltaTime / duration;
                currentHealth = Mathf.Lerp(start, end, t);
                UpdateBar();
                yield return null;
            }
            currentHealth = end;
            UpdateBar();
        }
    }
}
