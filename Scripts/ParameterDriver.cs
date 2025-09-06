// !!!ID: 56f8add2ed8c489aa40aaf6965b4ccbf
using System;
using UnityEngine;

/*
namespace Basics {
    public class ParameterDriver : MonoBehaviour {

        public Behaviour behaviour;
        public Component component;
        public string parameterName;


        public enum DriverType { ShiftConstant, Periodic, Switch, StepSmooth, RandomStep }
        public enum TransitionType { Linear, Smoothstep, Smootherstep, EaseInQuad, EaseOutQuad, EaseInOutCubic, Sigmoid }

        public DriverType driverType;
        public TransitionType transitionType;

        [Header("ShiftConstant")]
        public float sc_Rate;
        public float sc_InitVal;
        public float sc_Delay;

        [Header("Periodic")]
        public float p_Delay;
        public float p_Period;
        public float p_MinVal;
        public float p_MaxVal;
        public float p_TimeOffset;

        [Header("Switch")]
        public float sw_Delay;
        public float sw_InitVal;
        public float sw_MaxVal;

        [Header("StepSmooth")]
        public float ss_Delay;
        public float ss_Duration;
        public float ss_InitVal;
        public float ss_FinalVal;

        [Header("RandomStep")]
        public float rs_Delay;
        public float rs_Period;
        public float rs_MinVal;
        public float rs_MaxVal;
        public float rs_Scale = 1f;

        private float elapsed;
        private float value;
        void SetValue() {
            if (component is RectTransform rt) {
                if (parameterName == "anchoredPosition.x") {
                    var pos = rt.anchoredPosition;
                    pos.x = value;
                    rt.anchoredPosition = pos;
                }
                else if (parameterName == "anchoredPosition.y") {
                    var pos = rt.anchoredPosition;
                    pos.y = value;
                    rt.anchoredPosition = pos;
                }
                else if (parameterName == "localScale.x") {
                    var scale = rt.localScale;
                    scale.x = value;
                    rt.localScale = scale;
                }
                else if (parameterName == "localScale.y") {
                    var scale = rt.localScale;
                    scale.y = value;
                    rt.localScale = scale;
                }
                else if (parameterName == "localScale.z") {
                    var scale = rt.localScale;
                    scale.z = value;
                    rt.localScale = scale;
                }
                else if (parameterName == "rotation.x") {
                    var rot = rt.localEulerAngles;
                    rot.x = value;
                    rt.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.y") {
                    var rot = rt.localEulerAngles;
                    rot.y = value;
                    rt.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.z") {
                    var rot = rt.localEulerAngles;
                    rot.z = value;
                    rt.localEulerAngles = rot;
                }
            }
            else if (component is Transform t) {
                if (parameterName == "position.x") {
                    var pos = t.position;
                    pos.x = value;
                    t.position = pos;
                }
                else if (parameterName == "position.y") {
                    var pos = t.position;
                    pos.y = value;
                    t.position = pos;
                }
                else if (parameterName == "position.z") {
                    var pos = t.position;
                    pos.z = value;
                    t.position = pos;
                }
                else if (parameterName == "localScale.x") {
                    var scale = t.localScale;
                    scale.x = value;
                    t.localScale = scale;
                }
                else if (parameterName == "localScale.y") {
                    var scale = t.localScale;
                    scale.y = value;
                    t.localScale = scale;
                }
                else if (parameterName == "localScale.z") {
                    var scale = t.localScale;
                    scale.z = value;
                    t.localScale = scale;
                }
                else if (parameterName == "rotation.x") {
                    var rot = t.localEulerAngles;
                    rot.x = value;
                    t.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.y") {
                    var rot = t.localEulerAngles;
                    rot.y = value;
                    t.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.z") {
                    var rot = t.localEulerAngles;
                    rot.z = value;
                    t.localEulerAngles = rot;
                }
            }
            else {
                var target = (Object)component ?? behaviour;
                var type = target.GetType();
                var field = type.GetField(parameterName);
                var prop = type.GetProperty(parameterName);

                if (field != null && field.FieldType == typeof(float)) {
                    field.SetValue(target, value);
                }
                else if (prop != null && prop.PropertyType == typeof(float) && prop.CanWrite) {
                    prop.SetValue(target, value, null);
                }
            }
        }


        void Update() {
            elapsed += Time.deltaTime;
            switch (driverType) {
                case DriverType.ShiftConstant:
                    value = elapsed < sc_Delay
                        ? sc_InitVal
                        : sc_InitVal + sc_Rate * (elapsed - sc_Delay);
                    break;

                case DriverType.Periodic:
                    /*
                    //if (elapsed < p_Delay) { value = p_MinVal; break; }
                    //float u = ((elapsed - p_Delay) + p_TimeOffset) / p_Period;
                    //value = p_MinVal + (p_MaxVal - p_MinVal) * (u - Mathf.Floor(u));
                    //break;

                    if (elapsed < p_Delay) {
                        value = p_MinVal;
                    }
                    else {
                        float u = ((elapsed - p_Delay) + p_TimeOffset) / p_Period;
                        float phase = 2f * Mathf.PI * u;
                        float t = (Mathf.Sin(phase) + 1f) * 0.5f; // remap from [-1,1] to [0,1]
                        value = Mathf.Lerp(p_MinVal, p_MaxVal, t);
                    }
                    break;

                case DriverType.Switch:
                    value = elapsed < sw_Delay
                        ? sw_InitVal
                        : sw_MaxVal;
                    break;

                case DriverType.StepSmooth:
                    if (elapsed < ss_Delay) { value = ss_InitVal; break; }
                    float t1 = (elapsed - ss_Delay) / ss_Duration;
                    t1 = Mathf.Clamp01(t1);
                    value = Mathf.Lerp(ss_InitVal, ss_FinalVal, ApplyTransition(t1));
                    break;

                case DriverType.RandomStep:
                    if (elapsed < rs_Delay) { value = rs_MinVal; break; }
                    int steps = Mathf.FloorToInt((elapsed - rs_Delay) / rs_Period);
                    if (steps != Mathf.FloorToInt((elapsed - rs_Delay - Time.deltaTime) / rs_Period))
                        value = Random.Range(rs_MinVal, rs_MaxVal) * rs_Scale;
                    break;
            }

            // example usage:
            // Shader.SetGlobalFloat("_MyParam", value);

            if (!string.IsNullOrEmpty(parameterName) && (component || behaviour)) {
                SetValue();
            }

        }

        float ApplyTransition(float x) {
            x = Mathf.Clamp01(x);
            switch (transitionType) {
                case TransitionType.Linear: return x;
                case TransitionType.Smoothstep: return x * x * (3f - 2f * x);
                case TransitionType.Smootherstep: return x * x * x * (x * (x * 6f - 15f) + 10f);
                case TransitionType.EaseInQuad: return x * x;
                case TransitionType.EaseOutQuad: return x * (2f - x);
                case TransitionType.EaseInOutCubic:
                    return x < .5f
                        ? 4f * x * x * x
                        : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
                case TransitionType.Sigmoid:
                    return 1f / (1f + Mathf.Exp(-10f * (x - .5f)));
            }
            return x;
        }
    }
}*/

/*
using UnityEngine.UI;

namespace Basics {
    public class ParameterDriver : MonoBehaviour {

        public Behaviour behaviour;
        public Component component;
        public string parameterName;

        private Vector2 initialOffsetMin;
        private Vector2 initialOffsetMax;
        private bool initializedOffsets = false;

        public bool useInitialOffsets = false;

        public bool useInitialValue = false;
        private bool capturedInitial = false;
        private float initialValue;

        void Start() {
            if (useInitialValue && !capturedInitial) {
                capturedInitial = true;
                initialValue = GetCurrentParameterValue();
            }
        }

        float GetCurrentParameterValue() {
            if (component is RectTransform rt) {
                if (parameterName == "anchoredPosition.x") return rt.anchoredPosition.x;
                if (parameterName == "anchoredPosition.y") return rt.anchoredPosition.y;
                if (parameterName == "localScale.x") return rt.localScale.x;
                if (parameterName == "localScale.y") return rt.localScale.y;
                if (parameterName == "localScale.z") return rt.localScale.z;
                if (parameterName == "rotation.x") return rt.localEulerAngles.x;
                if (parameterName == "rotation.y") return rt.localEulerAngles.y;
                if (parameterName == "rotation.z") return rt.localEulerAngles.z;
            }
            else if (component is Transform t) {
                if (parameterName == "position.x") return t.localPosition.x;
                if (parameterName == "position.y") return t.localPosition.y;
                if (parameterName == "position.z") return t.localPosition.z;
                if (parameterName == "localScale.x") return t.localScale.x;
                if (parameterName == "localScale.y") return t.localScale.y;
                if (parameterName == "localScale.z") return t.localScale.z;
                if (parameterName == "rotation.x") return t.localEulerAngles.x;
                if (parameterName == "rotation.y") return t.localEulerAngles.y;
                if (parameterName == "rotation.z") return t.localEulerAngles.z;
            }
            else {
                var target = (System.Object)behaviour ?? component;
                var type = target.GetType();
                var field = type.GetField(parameterName);
                var prop = type.GetProperty(parameterName);
                if (field != null && field.FieldType == typeof(float))
                    return (float)field.GetValue(target);
                if (prop != null && prop.PropertyType == typeof(float) && prop.CanRead)
                    return (float)prop.GetValue(target, null);
            }
            return 0f;
        }


        public enum DriverType { ShiftConstant, Modulo, Periodic, Switch, StepSmooth, RandomStep }
        public enum TransitionType { Linear, Smoothstep, Smootherstep, EaseInQuad, EaseOutQuad, EaseInOutCubic, Sigmoid }

        public DriverType driverType;
        public TransitionType transitionType;

        //[Header("ShiftConstant")]
        public float sc_Rate;
        public float sc_InitVal;
        public float sc_Delay;

        //[Header("Modulo")]
        public float m_Period;
        public float m_MinVal;
        public float m_MaxVal;
        public float m_Delay;
        public float m_Offset;

        //[Header("Periodic")]
        public float p_Delay;
        public float p_Period;
        public float p_MinVal;
        public float p_MaxVal;
        public float p_TimeOffset;

        //[Header("Switch")]
        public float sw_Delay;
        public float sw_InitVal;
        public float sw_MaxVal;

        //[Header("StepSmooth")]
        public float ss_Delay;
        public float ss_Duration;
        public float ss_InitVal;
        public float ss_FinalVal;

        //[Header("RandomStep")]
        public float rs_Delay;
        public float rs_Period;
        public float rs_MinVal;
        public float rs_MaxVal;
        public float rs_Scale = 1f;

        private float elapsed;
        private float value;

        void Update() {
            elapsed += Time.deltaTime;

            switch (driverType) {
                case DriverType.ShiftConstant:
                    value = elapsed < sc_Delay
                        ? sc_InitVal
                        : sc_InitVal + sc_Rate * (elapsed - sc_Delay);
                    break;

                case DriverType.Modulo:
                    if (elapsed < m_Delay) {
                        value = m_MinVal;
                        break;
                    }

                    float actualTime = Mathf.Max(elapsed - m_Delay, 0f);
                    float u_mod = (actualTime + m_Offset) / m_Period;
                    u_mod = u_mod - Mathf.Floor(u_mod); // fractional part
                    value = Mathf.Lerp(m_MinVal, m_MaxVal, u_mod);
                    break;

                case DriverType.Periodic:
                    if (elapsed < p_Delay) {
                        value = p_MinVal;
                    }
                    else {
                        float u = ((elapsed - p_Delay) + p_TimeOffset) / p_Period;
                        float phase = 2f * Mathf.PI * u;
                        float t = (Mathf.Sin(phase) + 1f) * 0.5f;
                        value = Mathf.Lerp(p_MinVal, p_MaxVal, t);
                    }
                    break;

                case DriverType.Switch:
                    value = elapsed < sw_Delay
                        ? sw_InitVal
                        : sw_MaxVal;
                    break;

                case DriverType.StepSmooth:
                    if (elapsed < ss_Delay) {
                        value = ss_InitVal;
                        break;
                    }
                    float t1 = (elapsed - ss_Delay) / ss_Duration;
                    t1 = Mathf.Clamp01(t1);
                    value = Mathf.Lerp(ss_InitVal, ss_FinalVal, ApplyTransition(t1));
                    break;

                case DriverType.RandomStep:
                    if (elapsed < rs_Delay) {
                        value = rs_MinVal;
                        break;
                    }
                    int steps = Mathf.FloorToInt((elapsed - rs_Delay) / rs_Period);
                    if (steps != Mathf.FloorToInt((elapsed - rs_Delay - Time.deltaTime) / rs_Period))
                        value = UnityEngine.Random.Range(rs_MinVal, rs_MaxVal) * rs_Scale;
                    break;
            }

            if (!string.IsNullOrEmpty(parameterName) && (component || behaviour)) {
                SetValue();
            }
        }

        float ApplyTransition(float x) {
            x = Mathf.Clamp01(x);
            switch (transitionType) {
                case TransitionType.Linear: return x;
                case TransitionType.Smoothstep: return x * x * (3f - 2f * x);
                case TransitionType.Smootherstep: return x * x * x * (x * (x * 6f - 15f) + 10f);
                case TransitionType.EaseInQuad: return x * x;
                case TransitionType.EaseOutQuad: return x * (2f - x);
                case TransitionType.EaseInOutCubic:
                    return x < .5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
                case TransitionType.Sigmoid:
                    return 1f / (1f + Mathf.Exp(-10f * (x - .5f)));
            }
            return x;
        }

        void SetValue() {
            var pname = parameterName.ToLowerInvariant();
            if (component is RectTransform rt) {
                if (parameterName == "anchoredPosition.x") {
                    var pos = rt.anchoredPosition;
                    pos.x = value;
                    rt.anchoredPosition = pos;
                }
                else if (parameterName == "anchoredPosition.y") {
                    var pos = rt.anchoredPosition;
                    pos.y = value;
                    rt.anchoredPosition = pos;
                }
                else if (parameterName == "localScale.x") {
                    var scale = rt.localScale;
                    scale.x = value;
                    rt.localScale = scale;
                }
                else if (parameterName == "localScale.y") {
                    var scale = rt.localScale;
                    scale.y = value;
                    rt.localScale = scale;
                }
                else if (parameterName == "localScale.z") {
                    var scale = rt.localScale;
                    scale.z = value;
                    rt.localScale = scale;
                }
                else if (parameterName == "rotation.x") {
                    var rot = rt.localEulerAngles;
                    rot.x = value;
                    rt.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.y") {
                    var rot = rt.localEulerAngles;
                    rot.y = value;
                    rt.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.z") {
                    var rot = rt.localEulerAngles;
                    rot.z = value;
                    rt.localEulerAngles = rot;
                }
                /*
                else if (pname == "leftpadding") {
                    rt.offsetMin = new Vector2(initialOffsetMin.x + value, rt.offsetMin.y);
                }
                else if (pname == "bottompadding") {
                    rt.offsetMin = new Vector2(rt.offsetMin.x, initialOffsetMin.y + value);
                }
                else if (pname == "rightpadding") {
                    rt.offsetMax = new Vector2(initialOffsetMax.x - value, rt.offsetMax.y);
                }
                else if (pname == "toppadding") {
                    rt.offsetMax = new Vector2(rt.offsetMax.x, initialOffsetMax.y - value);
                }
                else {
                    // existing position/scale/rotation cases here
                }
            }
            else if (component is RectMask2D rectMask) {
                var paddingField = typeof(RectMask2D).GetField("m_Padding", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (paddingField != null) {
                    //Debug.Log("Found padding field");
                    Vector4 current = (Vector4)paddingField.GetValue(rectMask);
                    //string pname = parameterName != null ? parameterName.ToLowerInvariant() : "";
                    if (pname == "leftpadding") current.x = value;
                    else if (pname == "toppadding") current.y = value;
                    else if (pname == "rightpadding") current.z = value;
                    else if (pname == "bottompadding") current.w = value;
                    paddingField.SetValue(rectMask, current);

                    // Force the mask to update
                    var method = typeof(RectMask2D).GetMethod("PerformClipping", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    method?.Invoke(rectMask, null);
                }
            }
            else if (component is Transform t) {
               bool useLocal = true;
               if (useLocal) {
                    if (parameterName == "position.x") {
                        var pos = t.localPosition;
                        pos.x = value;
                        t.localPosition = pos;
                    }
                    else if (parameterName == "position.y") {
                        var pos = t.localPosition;
                        pos.y = value;
                        t.localPosition = pos;
                    }
                    else if (parameterName == "position.z") {
                        var pos = t.localPosition;
                        pos.z = value;
                        t.localPosition = pos;
                    }
                }
                else {
                    if (parameterName == "position.x") {
                        var pos = t.position;
                        pos.x = value;
                        t.position = pos;
                    }
                    else if (parameterName == "position.y") {
                        var pos = t.position;
                        pos.y = value;
                        t.position = pos;
                    }
                    else if (parameterName == "position.z") {
                        var pos = t.position;
                        pos.z = value;
                        t.position = pos;
                    }
                }
                if (parameterName == "position.x") {

                }
                else if (parameterName == "localScale.x") {
                    var scale = t.localScale;
                    scale.x = value;
                    t.localScale = scale;
                }
                else if (parameterName == "localScale.y") {
                    var scale = t.localScale;
                    scale.y = value;
                    t.localScale = scale;
                }
                else if (parameterName == "localScale.z") {
                    var scale = t.localScale;
                    scale.z = value;
                    t.localScale = scale;
                }
                else if (parameterName == "rotation.x") {
                    var rot = t.localEulerAngles;
                    rot.x = value;
                    t.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.y") {
                    var rot = t.localEulerAngles;
                    rot.y = value;
                    t.localEulerAngles = rot;
                }
                else if (parameterName == "rotation.z") {
                    var rot = t.localEulerAngles;
                    rot.z = value;
                    t.localEulerAngles = rot;
                }
            }
            else {
                Debug.Log("else");
                var target = (System.Object)behaviour ?? component;
                var type = target.GetType();
                var field = type.GetField(parameterName);
                var prop = type.GetProperty(parameterName);

                if (field == null && prop == null) {
                    Debug.LogWarning($"Parameter '{parameterName}' not found in {target.GetType().Name}");
                    return;
                }

                if (field != null && field.FieldType == typeof(float)) {
                    field.SetValue(target, value);
                }
                else if (prop != null && prop.PropertyType == typeof(float) && prop.CanWrite) {
                    prop.SetValue(target, value, null);
                }
            }
        }
    }
}
*/

using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace Basics {
    public class ParameterDriver : MonoBehaviour {

        public Behaviour behaviour;
        public Component component;
        public string parameterName;

        public bool useInitialValue = false;
        private bool capturedInitial = false;
        private float initialValue;

        void Start() {
            if (useInitialValue && !capturedInitial) {
                capturedInitial = true;
                initialValue = GetCurrentParameterValue();
            }
        }

        float GetCurrentParameterValue() {
            var target = (object)behaviour ?? component;
            var type = target.GetType();
            var field = type.GetField(parameterName);
            var prop = type.GetProperty(parameterName);

            if (field != null && field.FieldType == typeof(float))
                return (float)field.GetValue(target);
            if (prop != null && prop.PropertyType == typeof(float) && prop.CanRead)
                return (float)prop.GetValue(target, null);

            Debug.LogError($"Parameter '{parameterName}' not found or not float in {type.Name}");
            return 0f;
        }

        public enum DriverType { ShiftConstant, Modulo, Periodic, Switch, StepSmooth, RandomStep }
        public enum TransitionType { Linear, Smoothstep, Smootherstep, EaseInQuad, EaseOutQuad, EaseInOutCubic, Sigmoid }

        public DriverType driverType;
        public TransitionType transitionType;

        public float stopAfter = -1f;
        public bool setInitAfterDelay = false;

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

        private float elapsed;
        private float value;

        static readonly string[] DisallowedParams = new string[] {
            "position.x", "position.y", "position.z",
            "localPosition.x", "localPosition.y", "localPosition.z",
            "localScale.x", "localScale.y", "localScale.z",
            "rotation.x", "rotation.y", "rotation.z",
            "anchoredPosition.x", "anchoredPosition.y"
        };

        void Update() {
            if (Array.Exists(DisallowedParams, p => string.Equals(p, parameterName, StringComparison.OrdinalIgnoreCase))) {
                throw new InvalidOperationException($"Parameter '{parameterName}' is not allowed in ParameterDriver, use ParameterDriverTransform!.");
            }

            elapsed += Time.deltaTime;

            switch (driverType) {
                case DriverType.ShiftConstant:
                    value = elapsed < sc_Delay ? sc_InitVal : sc_InitVal + sc_Rate * (elapsed - sc_Delay);
                    break;

                case DriverType.Modulo:
                    if (elapsed < m_Delay) { value = m_MinVal; break; }
                    float actualTime = Mathf.Max(elapsed - m_Delay, 0f);
                    float u_mod = (actualTime + m_Offset) / m_Period;
                    u_mod = u_mod - Mathf.Floor(u_mod);
                    value = Mathf.Lerp(m_MinVal, m_MaxVal, u_mod);
                    break;

                case DriverType.Periodic:
                    if (elapsed < p_Delay) {
                        value = p_MinVal;
                    }
                    else {
                        float u = ((elapsed - p_Delay) + p_TimeOffset) / p_Period;
                        float phase = 2f * Mathf.PI * u;
                        float t = (Mathf.Sin(phase) + 1f) * 0.5f;
                        value = Mathf.Lerp(p_MinVal, p_MaxVal, t);
                    }
                    break;

                case DriverType.Switch:
                    value = elapsed < sw_Delay ? sw_InitVal : sw_MaxVal;
                    break;

                case DriverType.StepSmooth:
                    if (elapsed < ss_Delay) { value = ss_InitVal; break; }
                    float t1 = (elapsed - ss_Delay) / ss_Duration;
                    t1 = Mathf.Clamp01(t1);
                    value = Mathf.Lerp(ss_InitVal, ss_FinalVal, ApplyTransition(t1));
                    break;

                case DriverType.RandomStep:
                    if (elapsed < rs_Delay) { value = rs_MinVal; break; }
                    int steps = Mathf.FloorToInt((elapsed - rs_Delay) / rs_Period);
                    if (steps != Mathf.FloorToInt((elapsed - rs_Delay - Time.deltaTime) / rs_Period))
                        value = UnityEngine.Random.Range(rs_MinVal, rs_MaxVal) * rs_Scale;
                    break;
            }

            if (useInitialValue) {
                value += initialValue;
            }

            if (!string.IsNullOrEmpty(parameterName) && (component || behaviour)) {
                SetValue();
            }
        }

        float ApplyTransition(float x) {
            x = Mathf.Clamp01(x);
            switch (transitionType) {
                case TransitionType.Linear: return x;
                case TransitionType.Smoothstep: return x * x * (3f - 2f * x);
                case TransitionType.Smootherstep: return x * x * x * (x * (x * 6f - 15f) + 10f);
                case TransitionType.EaseInQuad: return x * x;
                case TransitionType.EaseOutQuad: return x * (2f - x);
                case TransitionType.EaseInOutCubic:
                    return x < .5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
                case TransitionType.Sigmoid:
                    return 1f / (1f + Mathf.Exp(-10f * (x - .5f)));
            }
            return x;
        }

        float GetTheDelay(){
            // what mode are we in?
            switch (driverType) {
                case DriverType.ShiftConstant:
                    return sc_Delay;
                case DriverType.Modulo:
                    return m_Delay;
                case DriverType.Periodic:
                    return p_Delay;
                case DriverType.Switch:
                    return sw_Delay;
                case DriverType.StepSmooth:
                    return ss_Delay;
                case DriverType.RandomStep:
                    return rs_Delay;
                default:
                    Debug.LogWarning($"Unknown DriverType: {driverType}. Returning 0 for delay.");
                    return 0f;

            }
        }

        void SetValue() {
            if (setInitAfterDelay && elapsed <= GetTheDelay()) {
                //capturedInitial = true;
                // initialValue = GetCurrentParameterValue();
                return;
            }

            var target = (object)behaviour ?? component;
            var type = target.GetType();
            var field = type.GetField(parameterName);
            var prop = type.GetProperty(parameterName);

            if (stopAfter > 0f && elapsed >= stopAfter) {
                Debug.Log($"Stopping ParameterDriver for {parameterName} after {stopAfter} seconds.");
                enabled = false;
                return;
            }

            if (field == null && prop == null) {
                Debug.LogWarning($"Parameter '{parameterName}' not found in {type.Name}");
                return;
            }

            if (field != null && field.FieldType == typeof(float)) {
                field.SetValue(target, value);
            }
            else if (prop != null && prop.PropertyType == typeof(float) && prop.CanWrite) {
                prop.SetValue(target, value, null);
            }
        }
    }
}

