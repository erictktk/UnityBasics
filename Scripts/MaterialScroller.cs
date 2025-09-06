// !!!ID: 8f9244a42986497ea57541d3da0d0317
using UnityEngine;

namespace Basics {
    public class MaterialScroller : MonoBehaviour {
        public enum RepetitionMode { Once, Number, Infinite }

        [Tooltip("Name of the material float property to animate")]
        public string propertyName = "_GleamOffset";


        public bool doSetColor = false;
        public Color Color = Color.white;

        public float initValue = 0f;
        public float finalValue = 1f;
        public float period = 1f;
        public float delay = 0f;
        public RepetitionMode repetition = RepetitionMode.Once;
        public int numberOfCycles = 1;

        private Material mat;
        private float elapsed;
        private bool started;
        private int cyclesDone;

        /*
        void Start() {
            var renderer = GetComponent<Renderer>();
            var image = GetComponent<UnityEngine.UI.Image>();

            if (renderer) mat = renderer.material;
            else if (image) {
                mat = Instantiate(image.material);
                image.material = mat;
            }

            if (mat && mat.HasProperty(propertyName))
                mat.SetFloat(propertyName, initValue);
        }*/

        void Start() {
            var renderer = GetComponent<Renderer>();
            var image = GetComponent<UnityEngine.UI.Image>();

            if (renderer) {
                mat = renderer.material;
            }
            else if (image) {
                // Only assign if image.material is still the shared default
                if (!image.material || image.material == image.defaultMaterial) {
                    //mat = Instantiate(image.material);
                    //image.material = mat;
                    mat = image.material;
                }
                else {
                    mat = image.material;
                }
            }
            else {
                Debug.LogWarning("MaterialScroller: No Renderer or Image found on the GameObject.");
                return;
            }

            if (mat && mat.HasProperty(propertyName))
                mat.SetFloat(propertyName, initValue);
        }

        void Update() {
            if (!mat || !mat.HasProperty(propertyName)) return;

            elapsed += Time.deltaTime;

            if (!started) {
                if (elapsed < delay) return;
                elapsed -= delay;
                started = true;
            }

            if (period <= 0f) return;

            float cycleT = (elapsed % period) / period;
            float value = Mathf.Lerp(initValue, finalValue, cycleT);
            mat.SetFloat(propertyName, value);

            if (doSetColor) {
                mat.SetColor("_Color", Color);
            }

            if (repetition == RepetitionMode.Once && elapsed >= period) {
                mat.SetFloat(propertyName, finalValue);
                enabled = false;
            }
            else if (repetition == RepetitionMode.Number) {
                int newCycles = Mathf.FloorToInt(elapsed / period);
                if (newCycles >= numberOfCycles) {
                    mat.SetFloat(propertyName, finalValue);
                    enabled = false;
                }
            }

            var renderer = GetComponent<Renderer>();
            var image = GetComponent<UnityEngine.UI.Image>();
            if (renderer) {
                if (renderer.material != mat) {
                    renderer.material = mat;
                }
            }
            else if (image) {
                if (image.material != mat) {
                    image.material = mat;
                }
            }
        }
    }
}
