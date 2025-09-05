using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Cycles through a list of mesh GameObjects over time.
/// You can use either a constant interval or per-mesh custom timings.
/// Only one mesh is active at a time.
/// </summary>
/// 

namespace Basics {
    public class MeshCycler : MonoBehaviour {
        public enum TimingMode { Constant, Custom }

        [Tooltip("Timing mode: use a single constant time or a list of custom times per mesh.")]
        public TimingMode timingMode = TimingMode.Constant;

        [Tooltip("List of mesh GameObjects to cycle through. Only one will be active at a time.")]
        public List<GameObject> meshObjects = new List<GameObject>();

        [Tooltip("If using Constant timing mode, this value defines the time per mesh.")]
        public float constantTime = 0.2f;

        [Tooltip("If using Custom timing mode, these times define how long each mesh stays active.")]
        public List<float> customTimes = new List<float>();

        [Tooltip("Toggle to pause/resume the mesh cycling.")]
        public bool paused = false;

        private int index;
        private float timer;

        void Start() {
            SetActiveOnly(index);
        }

        void Update() {
            if (paused || meshObjects.Count == 0) return;

            timer -= Time.deltaTime;
            float currentTime = timingMode == TimingMode.Constant
                ? constantTime
                : customTimes[Mathf.Clamp(index, 0, customTimes.Count - 1)];

            if (timer <= 0f) {
                index = (index + 1) % meshObjects.Count;
                SetActiveOnly(index);
                timer = currentTime;
            }
        }

        void SetActiveOnly(int activeIndex) {
            for (int i = 0; i < meshObjects.Count; i++)
                meshObjects[i].SetActive(i == activeIndex);
        }
    }
}

