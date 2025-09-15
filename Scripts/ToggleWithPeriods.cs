using System.Collections.Generic;
using UnityEngine;

namespace Basics {
    public class ToggleWithPeriods : MonoBehaviour {
        [Header("Timing")]
        public float offPeriod = 1f;
        public List<float> onPeriods = new List<float>() { 1f, 2f, 0.5f };
        public bool randomOffset = false;

        [Header("Target")]
        public GameObject target;

        int currentOnIndex = 0;
        float timer = 0f;
        bool isOn = false;

        private void Start() {
            if (randomOffset) {
                timer = Random.Range(0f, offPeriod);
            }
            else {
                timer = offPeriod;
            }
            SetActive(false);
            isOn = false;
        }

        void Update() {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                if (isOn) {
                    // switch off
                    SetActive(false);
                    isOn = false;
                    timer = offPeriod;
                }
                else {
                    // switch on
                    if (onPeriods.Count > 0) {
                        timer = onPeriods[currentOnIndex];
                        currentOnIndex = (currentOnIndex + 1) % onPeriods.Count;
                    }
                    else {
                        timer = 1f; // fallback
                    }
                    SetActive(true);
                    isOn = true;
                }
            }
        }

        void SetActive(bool state) {
            if (target != null) {
                target.SetActive(state);
            }
            else {
                gameObject.SetActive(state);
            }
        }
    }
}
