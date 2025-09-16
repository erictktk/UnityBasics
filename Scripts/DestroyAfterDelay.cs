// !!!ID: 6574a42a4d1d43b8ae856ec47ebb0dda
using UnityEngine;

/// <summary>
/// Destroys the GameObject after a specified delay.
/// Can start automatically on Start (Automatic mode) or manually via StartCountdown (Set mode).
/// Uses Update to track elapsed time and triggers Destroy when delay is reached.
/// </summary>
namespace Basics
{
    public class DestroyAfterDelay : MonoBehaviour
    {
        public enum TriggerMode { Automatic, Set }
        public TriggerMode mode = TriggerMode.Automatic;
        public float delay = 1f;

        private float timer;
        private bool running;

        void Start()
        {
            if (mode == TriggerMode.Automatic)
                StartCountdown();
        }

        void Update()
        {
            if (!running) return;
            timer += Time.deltaTime;
            if (timer >= delay)
                Destroy(gameObject);
        }

        // Call this to begin the countdown when mode == Set
        public void StartCountdown()
        {
            running = true;
            timer = 0f;
        }
    }
}
