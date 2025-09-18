// !!!ID: d64640de54394f778ee7bfdea7f28afd
using UnityEngine;


/// <summary>
/// Pulses a material’s HSL property between base and peak luminance  
/// Supports single pulse or repeat mode with a set repeat count  
/// Uses coroutine timing based on a configurable period  
/// Resets property to its base value when finished  
/// </summary>

namespace Basics {
    public class HSLImpulseDriver : MonoBehaviour {
        public enum Mode { Once, Repeat }

        public Renderer targetRenderer;
        public string shaderProperty = "_Luminance";
        public float peakLuminance = 2f;
        public float period = 1f;
        public Mode mode = Mode.Once;
        public int repeatCount = 1;

        private Material _mat;
        private int _propertyID;
        private float baseLuminance;
        private int _doneCount = 0;
        private bool _running = false;

        void Start() {
            if (targetRenderer == null) {
                Debug.LogWarning("No Renderer assigned to HSLImpulseDriver.");
                return;
            }
            _mat = targetRenderer.material;
            _propertyID = Shader.PropertyToID(shaderProperty);
            baseLuminance = _mat.GetFloat(_propertyID);
            StartPulse();
        }

        public void StartPulse() {
            if (_running || _mat == null) return;
            _running = true;
            _doneCount = 0;
            StartCoroutine(PulseRoutine());
        }

        private System.Collections.IEnumerator PulseRoutine() {
            do {
                float t = 0f;
                while (t < 1f) {
                    t += Time.deltaTime / (period * 0.5f);
                    float val = Mathf.Lerp(baseLuminance, peakLuminance, t);
                    _mat.SetFloat(_propertyID, val);
                    yield return null;
                }

                t = 0f;
                while (t < 1f) {
                    t += Time.deltaTime / (period * 0.5f);
                    float val = Mathf.Lerp(peakLuminance, baseLuminance, t);
                    _mat.SetFloat(_propertyID, val);
                    yield return null;
                }

                _doneCount++;
            } while (mode == Mode.Repeat && _doneCount < repeatCount);

            _mat.SetFloat(_propertyID, baseLuminance);
            _running = false;
        }
    }
}
