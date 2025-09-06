// !!!ID: d504c4d3cd22401d8a37e0d949a3e8c3
using UnityEngine;

namespace Basics {
    public class PlaySoundAfterDelay : MonoBehaviour {
        public AudioSource audioSource;
        public float delay = 1f;
        public bool playOnStart = true;

        private float timer;
        private bool playing;

        void Start() {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (playOnStart) StartDelay();
        }

        void Update() {
            if (!playing) return;

            timer += Time.deltaTime;
            if (timer >= delay) {
                audioSource.Play();
                playing = false;
            }
        }

        public void StartDelay() {
            timer = 0f;
            playing = true;
        }
    }
}
