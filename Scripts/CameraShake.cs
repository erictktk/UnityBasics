// !!!ID: 887beb33893144b28f7fa17f771e554b
using UnityEngine;

/// <summary>
/// Shakes the camera by displacing its localPosition randomly each frame,
/// with magnitude decaying smoothly over time.
/// Safe with domain reload disabled.
/// </summary>
public class CameraShake : MonoBehaviour {
    public static CameraShake Instance;

    private Vector3 originalPos;
    private float shakeMagnitude = 0f;
    public float dampingSpeed = 1.0f;
    private float originalDampingSpeed;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        originalDampingSpeed = dampingSpeed;
    }

    void OnEnable() {
        originalPos = transform.localPosition;
    }

    void OnDisable() {
        if (Instance == this)
            Instance = null;
    }

    void Update() {
        if (shakeMagnitude > 0.001f) {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0f, Time.deltaTime * dampingSpeed);
        }
        else {
            shakeMagnitude = 0f;
            transform.localPosition = originalPos;
        }
    }

    public void ResetDampingSpeed() {
        dampingSpeed = originalDampingSpeed;
    }

    /// <summary>
    /// Trigger a camera shake.
    /// </summary>
    /// <param name="magnitude">How strong the shake starts.</param>
    public void Shake(float magnitude, float? dampingSpeed = null)
    {
        shakeMagnitude = magnitude;
        if (dampingSpeed.HasValue)
            this.dampingSpeed = dampingSpeed.Value;
    }
}
