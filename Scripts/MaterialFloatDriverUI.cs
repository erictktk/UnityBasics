// !!!ID: 3edc2fbd1c624a73a96295a38810dfe1
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaterialFloatDriverUI : MonoBehaviour {
    public enum EaseType {
        Linear, EaseIn, EaseOut, EaseInOut,
        EaseOutCubic, EaseOutQuart, EaseOutQuint,
        CustomCurve
    }

    [Header("Target")]
    public Graphic uiTarget;
    public string materialParamName = "_MyFloat";

    [Header("Timing")]
    public float delay = 0f;
    public float duration = 1f;
    public float autoStopTime = 5f;

    [Header("Animation")]
    public EaseType easeType = EaseType.Linear;
    public AnimationCurve customCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Output Range")]
    public float from = 0f;
    public float to = 1f;

    private float timer;
    private float stopTimer;

    void OnEnable() {
        StartCoroutine(Animate());
    }

    IEnumerator Animate() {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        timer = 0f;
        stopTimer = 0f;

        while (stopTimer < autoStopTime) {
            timer += Time.deltaTime;
            stopTimer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            float eased = ApplyEase(t);
            float val = Mathf.Lerp(from, to, eased);

            if (uiTarget && uiTarget.material)
                uiTarget.material.SetFloat(materialParamName, val);

            yield return null;
        }
    }

    float ApplyEase(float t) {
        switch (easeType) {
            case EaseType.Linear: return t;
            case EaseType.EaseIn: return t * t;
            case EaseType.EaseOut: return t * (2f - t);
            case EaseType.EaseInOut: return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
            case EaseType.EaseOutCubic: return 1f - Mathf.Pow(1f - t, 3f);
            case EaseType.EaseOutQuart: return 1f - Mathf.Pow(1f - t, 4f);
            case EaseType.EaseOutQuint: return 1f - Mathf.Pow(1f - t, 5f);
            case EaseType.CustomCurve: return customCurve.Evaluate(t);
            default: return t;
        }
    }
}
