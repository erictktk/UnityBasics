// !!!ID: 99cc0f2b380e4dac9e72524e64008c8e
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

namespace Basics {
    public class ParameterDriverTransform : MonoBehaviour {

        public bool resetOnDisable;

        public enum TargetType { Transform, RectTransform }
        public enum Parameter {
            PosX, PosY, PosZ,
            LocalScaleX, LocalScaleY, LocalScaleZ,
            LocalUniformScale,
            RotX, RotY, RotZ,
            AnchoredPosX, AnchoredPosY
        }
        public enum DriverType { ShiftConstant, Modulo, Periodic, Switch, StepSmooth, RandomStep }
        public enum TransitionType { Linear, Smoothstep, Smootherstep, EaseInQuad, EaseOutQuad, EaseInOutCubic, Sigmoid }

        public TargetType targetType = TargetType.Transform;
        public Parameter parameter;

        public DriverType driverType;
        public TransitionType transitionType;

        #region Parameters
        public float sc_Rate;
        public float sc_InitVal;
        public float sc_Delay;

        public float m_Period;
        public float m_MinVal;
        public float m_MaxVal;
        public float m_Delay;
        public float m_Offset;

        public float p_Delay;
        public float p_Period;
        public float p_MinVal;
        public float p_MaxVal;
        public float p_TimeOffset;

        public float sw_Delay;
        public float sw_InitVal;
        public float sw_MaxVal;

        public float ss_Delay;
        public float ss_Duration;
        public float ss_InitVal;
        public float ss_FinalVal;

        public float rs_Delay;
        public float rs_Period;
        public float rs_MinVal;
        public float rs_MaxVal;
        public float rs_Scale = 1f;
        #endregion

        public bool useInitialValue = false;

        private float elapsed;
        private float value;
        private float initial;
        private Vector3 initialVector;
        private bool initialized = false;

        void Start() {
            CacheInitial();
           // initialVector = transform.localScale; // Cache the initial scale vector
        }

        void CacheInitial() {
            //if (initialized || !useInitialValue) return;

            if (targetType == TargetType.Transform) {
                var t = transform;
                if (parameter == Parameter.PosX) initial = t.localPosition.x;
                else if (parameter == Parameter.PosY) initial = t.localPosition.y;
                else if (parameter == Parameter.PosZ) initial = t.localPosition.z;
                else if (parameter == Parameter.LocalScaleX) initial = t.localScale.x;
                else if (parameter == Parameter.LocalScaleY) initial = t.localScale.y;
                else if (parameter == Parameter.LocalScaleZ) initial = t.localScale.z;
                else if (parameter == Parameter.LocalUniformScale) initialVector = t.localScale;
                else if (parameter == Parameter.RotX) initial = t.localEulerAngles.x;
                else if (parameter == Parameter.RotY) initial = t.localEulerAngles.y;
                else if (parameter == Parameter.RotZ) initial = t.localEulerAngles.z;
            }
            else if (targetType == TargetType.RectTransform) {
                var rt = transform as RectTransform;
                if (parameter == Parameter.AnchoredPosX) initial = rt.anchoredPosition.x;
                    else if (parameter == Parameter.AnchoredPosY) initial = rt.anchoredPosition.y;
                    else if (parameter == Parameter.LocalScaleX) initial = rt.localScale.x;
                    else if (parameter == Parameter.LocalScaleY) initial = rt.localScale.y;
                    else if (parameter == Parameter.LocalScaleZ) initial = rt.localScale.z;
                    else if (parameter == Parameter.LocalUniformScale) initialVector = rt.localScale;
                    else if (parameter == Parameter.RotX) initial = rt.localEulerAngles.x;
                    else if (parameter == Parameter.RotY) initial = rt.localEulerAngles.y;
                    else if (parameter == Parameter.RotZ) initial = rt.localEulerAngles.z;
            }

            initialized = true;
        }

        void Update() {
            elapsed += Time.deltaTime;

            switch (driverType) {
                case DriverType.ShiftConstant:
                    value = elapsed < sc_Delay ? sc_InitVal : sc_InitVal + sc_Rate * (elapsed - sc_Delay);
                    break;

                case DriverType.Modulo:
                    if (elapsed < m_Delay) { value = m_MinVal; break; }
                    float modTime = (elapsed - m_Delay + m_Offset) / m_Period;
                    modTime = modTime - Mathf.Floor(modTime);
                    value = Mathf.Lerp(m_MinVal, m_MaxVal, modTime);
                    break;

                case DriverType.Periodic:
                    if (elapsed < p_Delay) { value = p_MinVal; break; }
                    float u = ((elapsed - p_Delay) + p_TimeOffset) / p_Period;
                    float phase = 2f * Mathf.PI * u;
                    float t = (Mathf.Sin(phase) + 1f) * 0.5f;
                    value = Mathf.Lerp(p_MinVal, p_MaxVal, t);
                    break;

                case DriverType.Switch:
                    value = elapsed < sw_Delay ? sw_InitVal : sw_MaxVal;
                    break;

                case DriverType.StepSmooth:
                    if (elapsed < ss_Delay) { value = ss_InitVal; break; }
                    float t1 = Mathf.Clamp01((elapsed - ss_Delay) / ss_Duration);
                    value = Mathf.Lerp(ss_InitVal, ss_FinalVal, ApplyTransition(t1));
                    break;

                case DriverType.RandomStep:
                    if (elapsed < rs_Delay) { value = rs_MinVal; break; }
                    int step = Mathf.FloorToInt((elapsed - rs_Delay) / rs_Period);
                    if (step != Mathf.FloorToInt((elapsed - rs_Delay - Time.deltaTime) / rs_Period))
                        value = Random.Range(rs_MinVal, rs_MaxVal) * rs_Scale;
                    break;
            }

            ApplyValue(null, null);
        }

        // edited so 
        void ApplyValue(float ? passedInVal, Vector3 ? passedInVector ) {
            float v;
            if (passedInVal == null) {
                v = useInitialValue ? initial + value : value;
            }
            else {
                v = (float)passedInVal;
            }
            Vector3 theVector;
            if (passedInVector == null) {
                theVector = useInitialValue ? initialVector + new Vector3(value, value, value) : new Vector3(value, value, value);
            }
            else {
                theVector = (Vector3)passedInVector;
                Debug.Log(theVector);
            }


            if (targetType == TargetType.Transform) {
                var t = transform;
                if (parameter == Parameter.PosX) {
                    var p = t.localPosition;
                    p.x = v;
                    t.localPosition = p;
                }
                else if (parameter == Parameter.PosY) {
                    var p = t.localPosition;
                    p.y = v;
                    t.localPosition = p;
                }
                else if (parameter == Parameter.PosZ) {
                    var p = t.localPosition;
                    p.z = v;
                    t.localPosition = p;
                }
                else if (parameter == Parameter.LocalScaleX) {
                    var s = t.localScale;
                    s.x = v;
                    t.localScale = s;
                }
                else if (parameter == Parameter.LocalScaleY) {
                    var s = t.localScale;
                    s.y = v;
                    t.localScale = s;
                }
                else if (parameter == Parameter.LocalScaleZ) {
                    var s = t.localScale;
                    s.z = v;
                    t.localScale = s;
                }
                else if (parameter == Parameter.LocalUniformScale) {
                    var s = t.localScale;
                    // can't use v with 
                    /*
                    if (useInitialValue) {
                        s.x = value + initialVector.x;
                        s.y = value + initialVector.y;
                        s.z = value + initialVector.z;
                    }
                    else {
                        s.x = value;
                        s.y = value;
                        s.z = value;
                    }
                    t.localScale = s;
                     */

                    t.localScale = theVector;

                    //Debug.Log("using uniform scale: " + v);
                }
                else if (parameter == Parameter.RotX) {
                    var r = t.localEulerAngles;
                    r.x = v;
                    t.localEulerAngles = r;
                }
                else if (parameter == Parameter.RotY) {
                    var r = t.localEulerAngles;
                    r.y = v;
                    t.localEulerAngles = r;
                }
                else if (parameter == Parameter.RotZ) {
                    var r = t.localEulerAngles;
                    r.z = v;
                    t.localEulerAngles = r;
                }
            }
            else if (targetType == TargetType.RectTransform) {
                var rt = transform as RectTransform;
                if (parameter == Parameter.AnchoredPosX) {
                    var a = rt.anchoredPosition;
                    a.x = v;
                    rt.anchoredPosition = a;
                }
                else if (parameter == Parameter.AnchoredPosY) {
                    var a = rt.anchoredPosition;
                    a.y = v;
                    rt.anchoredPosition = a;
                }
                else if (parameter == Parameter.LocalScaleX) {
                    var s = rt.localScale;
                    s.x = v;
                    rt.localScale = s;
                }
                else if (parameter == Parameter.LocalScaleY) {
                    var s = rt.localScale;
                    s.y = v;
                    rt.localScale = s;
                }
                else if (parameter == Parameter.LocalUniformScale) {
                    var s = rt.localScale;
                    // can't use v with 
                    /*
                    if (useInitialValue) {
                        s.x = value + initialVector.x;
                        s.y = value + initialVector.y;
                        s.z = value + initialVector.z;
                    }
                    else {
                        s.x = value;
                        s.y = value;
                        s.z = value;
                    }
                    rt.localScale = s;
                     */

                    rt.localScale = theVector;

                }
                else if (parameter == Parameter.LocalScaleZ) {
                    var s = rt.localScale;
                    s.z = v;
                    rt.localScale = s;
                }
                else if (parameter == Parameter.RotX) {
                    var r = rt.localEulerAngles;
                    r.x = v;
                    rt.localEulerAngles = r;
                }
                else if (parameter == Parameter.RotY) {
                    var r = rt.localEulerAngles;
                    r.y = v;
                    rt.localEulerAngles = r;
                }
                else if (parameter == Parameter.RotZ) {
                    var r = rt.localEulerAngles;
                    r.z = v;
                    rt.localEulerAngles = r;
                }
            }
        }

        float ApplyTransition(float x) {
            x = Mathf.Clamp01(x);
            if (transitionType == TransitionType.Linear) return x;
            else if (transitionType == TransitionType.Smoothstep) return x * x * (3f - 2f * x);
            else if (transitionType == TransitionType.Smootherstep) return x * x * x * (x * (x * 6f - 15f) + 10f);
            else if (transitionType == TransitionType.EaseInQuad) return x * x;
            else if (transitionType == TransitionType.EaseOutQuad) return x * (2f - x);
            else if (transitionType == TransitionType.EaseInOutCubic)
                return x < .5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
            else if (transitionType == TransitionType.Sigmoid)
                return 1f / (1f + Mathf.Exp(-10f * (x - .5f)));
            return x;
        }

        void OnDisable() {
            if (resetOnDisable) {
                Debug.Log(initialVector);
                if (parameter == Parameter.LocalUniformScale) {
                    ApplyValue(null, initialVector);
                }
                else {
                    ApplyValue(initial, null);
                }
            }
        }
    }
}
