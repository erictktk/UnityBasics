using UnityEngine;
using System.Collections;

public class MaterialFloatDriver : MonoBehaviour {
    public enum ParamType { Float, Vector4 }
    public enum DriverType {
        ShiftConstant, Lerp, Modulo, Periodic, Switch, StepSmooth, RandomStep, CustomCurve
    }

    [Header("Target")]
    public string materialParamName = "_MyFloat";
    public ParamType paramType = ParamType.Float;

    [Header("Vector4 Component Toggles (if Vector4)")]
    public bool doX = false;
    public bool doY = false;
    public bool doZ = true;
    public bool doW = false;

    [Header("Driver Type")]
    public DriverType driverType = DriverType.ShiftConstant;

    [Header("Post Lerp")]
    public bool applyLerp = false;
    public float lerpFrom = 0f;
    public float lerpTo = 1f;

    [Header("Post Curve")]
    public bool applyCurve = false;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("ShiftConstant")]
    public float sc_Delay = 0f;
    public float sc_InitVal = 0f;
    public float sc_Rate = 1f;

    [Header("Modulo")]
    public float m_Delay = 0f;
    public float m_MinVal = 0f;
    public float m_MaxVal = 1f;
    public float m_Period = 1f;
    public float m_Offset = 0f;

    [Header("Periodic")]
    public float p_Delay = 0f;
    public float p_MinVal = 0f;
    public float p_MaxVal = 1f;
    public float p_Period = 1f;
    public float p_TimeOffset = 0f;

    [Header("Switch")]
    public float sw_Delay = 0f;
    public float sw_InitVal = 0f;
    public float sw_MaxVal = 1f;

    [Header("StepSmooth")]
    public float ss_Delay = 0f;
    public float ss_InitVal = 0f;
    public float ss_FinalVal = 1f;
    public float ss_Duration = 1f;
    public AnimationCurve ss_Curve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("RandomStep")]
    public float rs_Delay = 0f;
    public float rs_MinVal = 0f;
    public float rs_MaxVal = 1f;
    public float rs_Period = 1f;
    public float rs_Scale = 1f;

    private Material runtimeMaterial;
    private Renderer rend;
    private float timer = 0f;
    private float lastStep = -1f;
    private float randomVal = 0f;

    void Start() {
        rend = GetComponent<Renderer>();
        if (rend) runtimeMaterial = rend.material;
    }

    void OnEnable() {
        timer = 0f;
        StartCoroutine(Animate());
    }

    IEnumerator Animate() {
        while (true) {
            timer += Time.deltaTime;
            float elapsed = timer;
            float value = 0f;

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
                    float sinT = (Mathf.Sin(phase) + 1f) * 0.5f;
                    value = Mathf.Lerp(p_MinVal, p_MaxVal, sinT);
                    break;

                case DriverType.Switch:
                    value = elapsed < sw_Delay ? sw_InitVal : sw_MaxVal;
                    break;

                case DriverType.StepSmooth:
                    if (elapsed < ss_Delay) { value = ss_InitVal; break; }
                    float t1 = Mathf.Clamp01((elapsed - ss_Delay) / ss_Duration);
                    value = Mathf.Lerp(ss_InitVal, ss_FinalVal, ss_Curve.Evaluate(t1));
                    break;

                case DriverType.RandomStep:
                    if (elapsed < rs_Delay) { value = rs_MinVal; break; }
                    float currentStep = Mathf.Floor((elapsed - rs_Delay) / rs_Period);
                    if (currentStep != lastStep) {
                        lastStep = currentStep;
                        randomVal = Random.Range(rs_MinVal, rs_MaxVal) * rs_Scale;
                    }
                    value = randomVal;
                    break;
            }

            if (applyLerp)
                value = Mathf.Lerp(lerpFrom, lerpTo, value);

            if (applyCurve)
                value = curve.Evaluate(value);

            if (runtimeMaterial) {
                if (paramType == ParamType.Float) {
                    runtimeMaterial.SetFloat(materialParamName, value);
                }
                else {
                    Vector4 vec = runtimeMaterial.GetVector(materialParamName);
                    if (doX) vec.x = value;
                    if (doY) vec.y = value;
                    if (doZ) vec.z = value;
                    if (doW) vec.w = value;
                    runtimeMaterial.SetVector(materialParamName, vec);
                }
            }

            yield return null;
        }
    }
}
